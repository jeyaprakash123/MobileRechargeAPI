using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TopUpAPI.Models
{
    public class BeneficiaryTopUpDetails
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Beneficiary")]
        public int BeneficiaryId { get; set; }

        public int MonthWise { get; set; }

        public int YearWise { get; set; }
        [Range(0, 1000)]
        public decimal Amount { get; set; }
        public decimal Charge { get; set; }
    }
}
