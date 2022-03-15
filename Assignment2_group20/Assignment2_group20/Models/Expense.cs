using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2_group20.Models
{
    public class Expense
    {
        public long ExpenseId { get; set; }
        public long ModelId { get; set; }
        public long JobId { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public string? Text { get; set; }
        [Column(TypeName = "decimal(9,2)")]
        public decimal amount { get; set; }

        public override string ToString()
        {
            string result = "";

            result = $"ExpenseId: {ExpenseId}, \n" +
                     $"ModelId: {ModelId}, \n" +
                     $"JobId: {JobId}, \n" +
                     $"Date: {Date}, \n" +
                     $"Text: {Text}, \n" +
                     $"Amount: {amount}, \n";

            return result;
        }
    }
}
