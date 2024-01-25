namespace B1TestTask.BLLTask1.Contracts
{
    public interface ITask1FileService
    {
        int CombineAndRemoveLines(string inputPath, string outputPath, string substring);
        void GenerateTextFiles(string outputPath);
        Task ImportDataToDatabase(string combinedFilePath, IProgress<(int, int)> progress);
    }
}
