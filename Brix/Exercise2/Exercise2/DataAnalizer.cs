using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise2
{
    public class FileDataAnalizer
    {
        private List<string> fileLines;
        private Dictionary<string, List<string>> analizedFileData;

        public FileDataAnalizer(List<string> fileLines)
        {
            this.fileLines = fileLines;
        }         

        public void Analize()
        {
            analizedFileData = new Dictionary<string, List<string>>();
            fileLines.ForEach(line =>
            {
                try
                {
                    AnalizeRow(line);
                }
                catch (Exception e)
                {
                    throw new Exception($"Error while analizing line: {line}", e);
                }                
            });
        }

        public List<string> SearchInAnalizedData(string searchFor)
        {
            var analizedSearchFor = string.Concat(searchFor.Trim().ToUpper().OrderBy(chr => chr));

            return analizedFileData.ContainsKey(analizedSearchFor) ? analizedFileData[analizedSearchFor] : new List<string>();
        }

        private void AnalizeRow(string lineToAnalize)
        {
            var analizedLine = string.Concat(lineToAnalize.Trim().ToUpper().OrderBy(chr => chr));

            if(analizedFileData.ContainsKey(analizedLine))
            {
                analizedFileData[analizedLine].Add(lineToAnalize);
            }
            else
            {
                analizedFileData.Add(analizedLine, new List<string> { lineToAnalize });
            }
        }
    }
}
