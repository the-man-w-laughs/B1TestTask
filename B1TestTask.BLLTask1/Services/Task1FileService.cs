﻿using B1TestTask.BLLTask1.Contracts;
using B1TestTask.DALTask1.Contracts;
using B1TestTask.DALTask1.Models;
using System.Globalization;
using System.Text;

namespace B1TestTask.BLLTask1.Services
{
    // Класс, предоставляющий функциональность по работе с файлами для задачи 1
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

        // Создаем экземпляр ThreadLocal<Random> для генерации случайных чисел в каждом потоке
        private readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random());


        private readonly IGeneratedDataModelRepository _generatedDataModelRepository;

        public Task1FileService(IGeneratedDataModelRepository generatedDataModelRepository)
        {
            _generatedDataModelRepository = generatedDataModelRepository;
        }

        public void GenerateTextFiles(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            Parallel.For(1, NumberOfFiles + 1, fileIndex =>
            {
                string filePath = Path.Combine(outputPath, $"TextFile_{fileIndex}.txt");

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

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

                        // Построение строки для записи в файл, содержащей случайные данные
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

        public int CombineAndRemoveLines(string inputPath, string outputPath, string substring)
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

            // Объект для блокировки доступа к общему файлу из нескольких потоков
            object lockObject = new object();

            // Параллельная обработка файлов входной директории
            Parallel.ForEach(Directory.EnumerateFiles(inputPath), filePath =>
            {
                string[] lines = File.ReadAllLines(filePath);
                int deletedLines = lines.Count(line => line.Contains(substring));

                // Атомарное увеличение общего количества удаленных строк
                Interlocked.Add(ref totalDeletedLines, deletedLines);

                // Записываем строки без подстроки в комбинированный файл, используя блокировку
                lock (lockObject)
                {
                    File.AppendAllLines(combinedFilePath, lines.Where(line => !line.Contains(substring)));
                }
            });

            return totalDeletedLines;
        }


        public async Task ImportDataToDatabaseAsync(string combinedFilePath, IProgress<(int, int)> progress)
        {
            if (!File.Exists(combinedFilePath))
            {
                throw new FileNotFoundException($"Combined file not found: {combinedFilePath}");
            }

            using (var reader = new StreamReader(combinedFilePath))
            {
                // Определение общего количества строк в файле асинхронно
                var totalLines = await Task.Run(() => CountLinesInFile(combinedFilePath));

                // Устанавливаем позицию чтения в начало файла
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                for (int i = 0; i < totalLines; i++)
                {
                    // Асинхронное чтение строки из файла
                    var line = await reader.ReadLineAsync();
                    if (line == null)
                    {
                        // Прерываем цикл, если достигнут конец файла
                        break;
                    }

                    var generatedData = ParseLineToGeneratedData(line);

                    await _generatedDataModelRepository.AddAsync(generatedData);

                    // Сохраняем изменения в репозитории и сообщаем о прогрессе каждые 5000 строк
                    if (i % 5000 == 0)
                    {
                        await _generatedDataModelRepository.SaveAsync();
                        await Task.Delay(1).ConfigureAwait(true);
                        progress.Report((i, totalLines - i));
                    }
                }
            }
        }

        public async Task<Tuple<decimal, decimal>> CalculateSumAndMedianAsync()
        {
            return await _generatedDataModelRepository.CallCalculateSumAndMedianAsync();
        }

        private int CountLinesInFile(string filePath)
        {
            using (FileStream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                return CountLinesInFile(reader);
            }
        }

        // Метод для подсчета количества строк в потоке
        private int CountLinesInFile(Stream stream)
        {
            // Проверяем, что поток не является пустым
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var lineCount = 0;
            const int bufferSize = 1024 * 1024;
            var buffer = new byte[bufferSize];
            var prevByte = -1;
            var pendingTermination = false;

            int bytesRead;

            // Чтение данных из потока в буфер
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    var currentByte = buffer[i];

                    // Обработка символов конца строки
                    switch (currentByte)
                    {
                        case 0:
                        case (byte)'\n' when prevByte == (byte)'\r':
                            continue;
                        case (byte)'\r':
                        case (byte)'\n' when prevByte != (byte)'\r':
                            lineCount++;
                            pendingTermination = false;
                            break;
                        default:
                            if (!pendingTermination)
                            {
                                pendingTermination = true;
                            }
                            break;
                    }
                    prevByte = currentByte;
                }
            }

            // Проверка, завершается ли последняя строка в потоке
            if (pendingTermination)
            {
                lineCount++;
            }

            return lineCount;
        }
        private GeneratedDataModel ParseLineToGeneratedData(string line)
        {
            string[] parts = line.Split("||");

            return new GeneratedDataModel
            {
                RandomDate = DateTime.ParseExact(parts[0], "dd.MM.yyyy", CultureInfo.InvariantCulture),
                RandomLatinChars = parts[1],
                RandomRussianChars = parts[2],
                RandomEvenInteger = int.Parse(parts[3]),
                RandomDecimal = double.Parse(parts[4], CultureInfo.InvariantCulture)
            };
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
