namespace B1TestTask.BLLTask1.Contracts
{
    public interface ITask1FileService
    {
        void CombineAndRemoveLines(string inputPath, string outputPath, string substring);
        void GenerateTextFiles(string outputPath);
    }
}
