using System.ComponentModel.DataAnnotations;

namespace Assignment2_group20.Models
{
    public class JobNoListsOrCustomerOrJobId
    {
        public DateTimeOffset StartDate { get; set; }
        public int Days { get; set; }
        [MaxLength(128)]
        public string? Location { get; set; }
        [MaxLength(2000)]
        public string? Comments { get; set; }
    }
}
