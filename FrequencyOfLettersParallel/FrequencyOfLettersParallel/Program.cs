using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyOfLettersParallel
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputStr = "SampleTextForFrequencyOfLetters";

            var strArr = inputStr.ToLower().ToCharArray();
            Dictionary<char, int> dict = new Dictionary<char, int>();

            dict = strArr
                .AsParallel()
                .Where(l => true)
                .GroupBy(c => c)
                .ToDictionary(group => group.Key, group => group.Count());

            var dict2 = dict.OrderByDescending(d => d.Value);

            foreach (var item in dict2)
            {
                Console.WriteLine($"{item.Key} - {item.Value} times");
            }

            Console.ReadKey();
        }
    }
}
