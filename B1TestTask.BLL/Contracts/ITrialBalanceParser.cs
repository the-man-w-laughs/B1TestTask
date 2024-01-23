using B1TestTask.BLLTask2.Dtos;

namespace B1TestTask.BLLTask2.Contracts
{
    public interface ITrialBalanceParser
    {
        FileContentDto ParseTrialBalance(string filePath);
    }
}
