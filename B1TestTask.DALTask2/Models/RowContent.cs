namespace B1TestTask.DALTask2.Models
{
    // Класс, представляющий данные для строки бухгалтерского баланса
    public class RowContent
    {
        // Входящее активное сальдо
        public decimal IncomingActive { get; set; }

        // Входящее пассивное сальдо
        public decimal IncomingPassive { get; set; }

        // Оборот по дебету
        public decimal TurnoverDebit { get; set; }

        // Оборот по кредиту
        public decimal TurnoverCredit { get; set; }

        // Исходящее активное сальдо
        public decimal OutgoingActive { get; set; }

        // Исходящее пассивное сальдо
        public decimal OutgoingPassive { get; set; }
    }

}
