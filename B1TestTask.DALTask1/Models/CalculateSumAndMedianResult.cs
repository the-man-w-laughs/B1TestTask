namespace B1TestTask.DALTask1.Models
{
    // Класс для хранения результатов расчета суммы и медианы
    public class CalculateSumAndMedianResult
    {
        // Идентификатор
        public int Id { get; set; }

        // Сумма целых чисел
        public decimal? SumOfIntegers { get; set; }

        // Медиана десятичных чисел
        public decimal? MedianOfDecimals { get; set; }
    }

}
