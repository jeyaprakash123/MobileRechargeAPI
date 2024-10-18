using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TopUpAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsVerified { get; set; }
        public decimal TotalTopUpLimit { get; set; }
        public decimal AvailableBalance { get; set; }
        public virtual ICollection<Beneficiary> Beneficiaries { get; set; }
    }
}
