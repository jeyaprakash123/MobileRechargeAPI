using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TopUpAPI.Models
{
    public class Beneficiary
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [MaxLength(20)]
        public string Nickname { get; set; }
        [Range(0, 1000)]
        public decimal MonthlyTopUpLimit { get; set; }

        [JsonIgnore]
        public virtual IEnumerable<BeneficiaryTopUpDetails> BeneficiaryTopUp { get; set; }    
    }
}
