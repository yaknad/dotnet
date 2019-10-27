using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise2
{
    public static class FileUtils
    {
        public static void GenerateFile(String url)
        {
            // create ASCII code ranges of alphnumerical characters
            var ranges = new[] { /*numbers*/ Tuple.Create(48, 57), /*uppercase letters*/ Tuple.Create(65, 90), /*lowercase letters*/ Tuple.Create(97, 122) };
            var possibleCharacters = ranges.Select(range => Enumerable.Range(range.Item1, range.Item2 - range.Item1 + 1))
                                           .SelectMany(x => x)
                                           .Distinct();

            string[] lines = new string[1000000];
            var rnd = new Random();

            for (int i = 0; i < lines.Length; i++)
            {
                StringBuilder sb = new StringBuilder();

                for (int j = 0; j < 5; j++)
                {
                    var asciiValue = possibleCharacters.Skip(rnd.Next(0, possibleCharacters.Count())).First();
                    sb.Append((char)asciiValue);
                }

                lines[i] = sb.ToString();
            }

            File.WriteAllLines(url, lines, Encoding.ASCII);
        }

        public static List<String> readFile(String url)
        {
            return new List<String>(File.ReadAllLines(url, Encoding.ASCII));
        }
    }
}
