using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TopUpAPI.DataMapper
{
    public class BeneficiaryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public decimal MonthlyTopUpLimit { get; set; }
        public decimal MonthlyTopUpTotal { get; set; }
    }
}
