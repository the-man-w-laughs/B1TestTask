namespace B1TestTask.BLLTask1.Contracts
{
    public interface ITask1FileService
    {
        // Асинхронный метод для расчета суммы и медианы
        Task<Tuple<decimal, decimal>> CalculateSumAndMedianAsync();

        // Метод для объединения файлов и удаления строк с указанной подстрокой
        int CombineAndRemoveLines(string inputPath, string outputPath, string substring);

        // Метод для генерации текстовых файлов
        void GenerateTextFiles(string outputPath);

        // Асинхронный метод для импорта данных в базу данных с отслеживанием прогресса
        Task ImportDataToDatabaseAsync(string combinedFilePath, IProgress<(int, int)> progress);
    }
}
