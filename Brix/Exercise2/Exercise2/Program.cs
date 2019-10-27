using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise2
{
    class Program
    {
        public const String FILE_NAME = "test-file.txt";

        static void Main(string[] args)
        {
            // IMPORTANT: skip the file creation block after the first run, since it takes a very long time!
            Console.WriteLine("Creating file.....");
            FileUtils.GenerateFile(FILE_NAME);
            Console.WriteLine("File created.");

            List<String> rawFileLines = FileUtils.readFile(FILE_NAME);
            Dictionary<String, List<String>> analizedData = DataAnalizer.AnalizeRawData(rawFileLines);

            while (true)
            {
                Console.WriteLine("\nPlease enter a sequence of 5 alphanumerical characters");
                string userInput = Console.ReadLine();

                IEnumerable<string> results = DataAnalizer.SearchInAnalizedData(userInput, analizedData);
                StringBuilder formattedResults = new StringBuilder();
                results.ToList().ForEach(result => formattedResults.Append($"{result}, "));
                if (formattedResults.Length > 0)
                {
                    // remove last ", "
                    formattedResults.Remove(formattedResults.Length - 2, 2);
                }
                Console.WriteLine($"You searced for: {userInput}\nThe matches are: {formattedResults}");
            }
        }
    }
}
