namespace B1TestTask.DALTask1.Models
{
    // Модель данных для хранения сгенерированных данных
    public class GeneratedDataModel
    {
        // Идентификатор
        public int Id { get; set; }

        // Случайная дата
        public DateTime RandomDate { get; set; }

        // Случайные латинские символы
        public string RandomLatinChars { get; set; } = string.Empty;

        // Случайные русские символы
        public string RandomRussianChars { get; set; } = string.Empty;

        // Случайное четное целое число
        public int RandomEvenInteger { get; set; }

        // Случайное десятичное число
        public double RandomDecimal { get; set; }
    }

}
