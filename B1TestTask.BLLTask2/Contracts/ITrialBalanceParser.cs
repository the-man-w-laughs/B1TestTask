using B1TestTask.BLLTask2.Dtos;

namespace B1TestTask.BLLTask2.Contracts
{
    // Интерфейс для парсера ОСВ
    public interface ITrialBalanceParser
    {
        // Метод для парсинга ОСВ и возвращения DTO с содержимым файла
        FileContentDto ParseTrialBalance(string filePath);
    }

}
