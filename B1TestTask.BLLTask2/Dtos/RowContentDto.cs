namespace B1TestTask.BLLTask2.Dtos
{
    public class RowContentDto
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal IncomingActive { get; set; }
        public decimal IncomingPassive { get; set; }
        public decimal TurnoverDebit { get; set; }
        public decimal TurnoverCredit { get; set; }
        public decimal OutgoingActive { get; set; }
        public decimal OutgoingPassive { get; set; }
    }
}
