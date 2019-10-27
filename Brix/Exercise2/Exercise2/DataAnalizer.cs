using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise2
{
    public static class DataAnalizer
    {
        public static Dictionary<String, List<String>> AnalizeRawData(List<String> lines)
        {
            var analizedLines = new Dictionary<String, List<String>>();

            foreach (var line in lines)
            {
                AnalizeRow(line, analizedLines);
            }

            return analizedLines;
        }

        public static IEnumerable<String> SearchInAnalizedData(String searchFor, Dictionary<String, List<String>> analizedData)
        {
            var modifiedChars = searchFor.Trim().ToUpper().ToArray();
            Array.Sort(modifiedChars);
            var analizedSearchFor = new string(modifiedChars.ToArray());

            return analizedData.ContainsKey(analizedSearchFor) ? analizedData[analizedSearchFor]
                                                               : Enumerable.Empty<string>();
        }

        private static void AnalizeRow(String lineToAnalize, Dictionary<String, List<String>> analizedData)
        {
            var modifiedChars = lineToAnalize.Trim().ToUpper().ToArray();
            Array.Sort(modifiedChars);
            var analizedLine = new string(modifiedChars.ToArray());

            if(analizedData.ContainsKey(analizedLine))
            {
                analizedData[analizedLine].Add(lineToAnalize);
            }
            else
            {
                analizedData.Add(analizedLine, new List<String> { lineToAnalize });
            }
        }
    }
}
