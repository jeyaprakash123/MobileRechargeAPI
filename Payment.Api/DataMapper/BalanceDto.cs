using SQLite;
using System.ComponentModel.DataAnnotations;

namespace BalanceApi.DataMapper
{
    public class BalanceDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}