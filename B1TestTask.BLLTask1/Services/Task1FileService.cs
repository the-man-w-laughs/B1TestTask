using B1TestTask.BLLTask1.Contracts;
using System.Text;

namespace B1TestTask.BLLTask1.Services
{
    public class Task1FileService : ITask1FileService
    {
        private const int NumberOfFiles = 100;
        private const int LinesPerFile = 100000;
        private const int MaxYearsBack = 5;
        private const int MaxEvenIntegerValue = 100000000;
        private const int LatinCharsLength = 10;
        private const int RussianCharsLength = 10;
        private const double MaxDecimalValue = 20.0;
        private const string LatinChars = "abcdefghijklmnopqrstuvwxyz";
        private const string RussianChars = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        private readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random());

        public void GenerateTextFiles(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            Parallel.For(1, NumberOfFiles + 1, fileIndex =>
            {
                string filePath = Path.Combine(outputPath, $"TextFile_{fileIndex}.txt");

                using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8, 8192))
                {
                    StringBuilder lineBuilder = new StringBuilder();

                    for (int lineIndex = 1; lineIndex <= LinesPerFile; lineIndex++)
                    {
                        lineBuilder.Clear();

                        string randomDate = GenerateRandomDate().ToString("dd.MM.yyyy");
                        string randomLatinChars = GenerateRandomString(LatinChars, LatinCharsLength);
                        string randomRussianChars = GenerateRandomString(RussianChars, RussianCharsLength);
                        int randomEvenInteger = random.Value.Next(1, MaxEvenIntegerValue) * 2;
                        double randomDecimal = random.Value.NextDouble() * MaxDecimalValue;

                        lineBuilder
                            .Append(randomDate).Append("||")
                            .Append(randomLatinChars).Append("||")
                            .Append(randomRussianChars).Append("||")
                            .Append(randomEvenInteger).Append("||")
                            .Append($"{randomDecimal:F8}||");

                        writer.WriteLine(lineBuilder.ToString());
                    }
                }
            });
        }

        public void CombineAndRemoveLines(string inputPath, string outputPath, string substring)
        {
            if (!Directory.Exists(inputPath))
            {
                throw new DirectoryNotFoundException($"Input directory not found: {inputPath}");
            }

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            int totalDeletedLines = 0;

            string combinedFilePath = Path.Combine(outputPath, "CombinedFile.txt");
            object lockObject = new object();

            Parallel.ForEach(Directory.EnumerateFiles(inputPath), filePath =>
            {
                string[] lines = File.ReadAllLines(filePath);
                int deletedLines = lines.Count(line => line.Contains(substring));
                totalDeletedLines += deletedLines;

                lock (lockObject)
                {
                    File.AppendAllLines(combinedFilePath, lines.Where(line => !line.Contains(substring)));
                }
            });

            Console.WriteLine($"Total deleted lines with {substring}: {totalDeletedLines}");
        }


        private DateTime GenerateRandomDate()
        {
            DateTime startDate = DateTime.Now.AddYears(-MaxYearsBack);
            int range = (DateTime.Today - startDate).Days;
            return startDate.AddDays(random.Value.Next(range));
        }

        private string GenerateRandomString(string characterSet, int length)
        {
            string upperCaseCharacterSet = characterSet + characterSet.ToUpper();

            return new string(Enumerable.Repeat(upperCaseCharacterSet, length)
                .Select(s => s[random.Value.Next(s.Length)]).ToArray());
        }
    }

}
