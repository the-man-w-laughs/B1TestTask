namespace B1TestTask.BLLTask2.Dtos
{
    // DTO для содержимого строки файла
    public class RowContentDto
    {
        // Номер счета
        public string AccountNumber { get; set; } = string.Empty;

        // Входящее активное сальдо
        public decimal IncomingActive { get; set; }

        // Входящее пассивное сальдо
        public decimal IncomingPassive { get; set; }

        // Обороты по дебету
        public decimal TurnoverDebit { get; set; }

        // Обороты по кредиту
        public decimal TurnoverCredit { get; set; }

        // Исходящее активное сальдо
        public decimal OutgoingActive { get; set; }

        // Исходящее пассивное сальдо
        public decimal OutgoingPassive { get; set; }
    }
}
