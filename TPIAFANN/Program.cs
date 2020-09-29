using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FANNCSharp;

namespace TPIAFANN
{
    public class Program
    {

        static void Main(string[] args)
        {

            Dictionary<char, float> dic = GetFrequencyFromText(@"D:\francais.txt");
            foreach (KeyValuePair<char, float> entry in dic)
            {
                Console.WriteLine(entry.Key + " : " + entry.Value);
            }


            Console.ReadLine();
        }

        public static Dictionary<char, float> GetFrequencyFromText(string path)
        {
            String s = File.ReadAllText(path);

            Dictionary<char, int> characterCount = new Dictionary<char, int>();

            for (char c = 'A'; c <= 'Z'; c++)
            {
                characterCount.Add(c, 0);
            }

            foreach (char c in s)
            {
                if(Char.ToUpper(c) >= 'A' && Char.ToUpper(c) <= 'Z')
                {
                    if (characterCount.ContainsKey(Char.ToUpper(c)))
                    {
                        characterCount[Char.ToUpper(c)]++;
                    }
                    else
                    {
                        characterCount[Char.ToUpper(c)] = 1;
                    }
                }

            }

            Dictionary<char, float> result = new Dictionary<char, float>();
            foreach (KeyValuePair<char, int> entry in characterCount)
            {
                float freq = (float)entry.Value / s.Length;
                result.Add(entry.Key, freq);
            }

            return result;
        }
    }
}
