using System.ComponentModel.DataAnnotations;

namespace TopUpAPI.Models
{
    public class TopUpOption
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
}
