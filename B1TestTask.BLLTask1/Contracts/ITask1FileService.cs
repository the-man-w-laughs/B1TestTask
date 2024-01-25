namespace B1TestTask.BLLTask1.Contracts
{
    public interface ITask1FileService
    {
        Task<Tuple<decimal, decimal>> CalculateSumAndMedianAsync();
        int CombineAndRemoveLines(string inputPath, string outputPath, string substring);
        void GenerateTextFiles(string outputPath);
        Task ImportDataToDatabaseAsync(string combinedFilePath, IProgress<(int, int)> progress);
    }
}
