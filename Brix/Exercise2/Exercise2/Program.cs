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
            GenerateFile();

            List<string> rawFileLines = ReadFile();       

            FileDataAnalizer analizer = new FileDataAnalizer(rawFileLines);

            try
            {
                analizer.Analize();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                Environment.Exit(-1);
            }            

            while (true)
            {
                Console.WriteLine("\nPlease enter a sequence of 5 alphanumerical characters");
                string userInput = Console.ReadLine();

                List<string> results = analizer.SearchInAnalizedData(userInput);
                String formattedResults = String.Join(", ", results);

                Console.WriteLine($"You searced for: {userInput}\nThe matches are: {formattedResults}");
            }
        }

        private static void GenerateFile()
        {
            try
            {
                Console.WriteLine("Creating file.....");
                FileUtils.GenerateFile(FILE_NAME);
                Console.WriteLine("File created.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while generating file: {e}");
                Console.ReadKey();
                Environment.Exit(-1);
            }           
        }

        private static List<string> ReadFile()
        {
            try
            {
                return FileUtils.readFile(FILE_NAME);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading file: {e}");
                Console.ReadKey();
                Environment.Exit(-1);
                // silence the compiler
                return null;
            }            
        }

    }
}
