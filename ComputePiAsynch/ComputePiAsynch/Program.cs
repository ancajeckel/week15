using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace ComputePiAsynch
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsynchMode ram = new RunAsynchMode();
            Stopwatch stop = new Stopwatch();

            //classic
            stop.Start();
            var pi1 = ram.ComputePi();
            stop.Stop();
            Console.WriteLine($"Pi={pi1} (classic way in {stop.ElapsedMilliseconds} ms)");

            //async task
            stop.Restart();
            var pi2 = ram.ComputePiAsync().Result;
            stop.Stop();
            Console.WriteLine($"Pi={pi2} (async way in {stop.ElapsedMilliseconds} ms)");

            //parallel computation
            stop.Restart();
            var pi3 = ram.ComputePiParallel();
            stop.Stop();
            Console.WriteLine($"Pi={pi3} (parallel comp in {stop.ElapsedMilliseconds} ms)");

            Console.ReadKey();
        }
    }

    public class RunAsynchMode
    {
        public double ComputePi()
        {
            var sum = 0.0;
            var step = 1e-9;

            for (var i = 0; i < 1000000000; i++)
            {
                var x = (i + 0.5) * step;
                sum += 4.0 / (1.0 + x * x);
            }

            return sum * step;
        }

        public async Task<double> ComputePiAsync()
        {
            var pi = await Task.Run(() => ComputePi());
            return pi;
        }

        public double ComputePiParallel()
        {
            var sum = 0.0;
            var step = 1e-9;
            object obj = new object();

            Parallel.ForEach(
                Partitioner.Create(0, 1000000000),
                () => 0.0,
                (range, state, partial) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        double x = (i + 0.5) * step;
                        partial += 4.0 / (1.0 + x * x);
                    }

                    return partial;
                },
                partial => { lock (obj) sum += partial; });

            return step * sum;
        }
    }
}
