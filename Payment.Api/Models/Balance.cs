using SQLite;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace BalanceApi.Models
{
    public class Balance
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Unique]
        public int UserId  { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}
