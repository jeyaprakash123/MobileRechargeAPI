using TopUpAPI.Models;

namespace TopUpAPI.DataMapper
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsVerified { get; set; }
        public decimal TotalTopUpLimit { get; set; }
        public decimal AvailableBalance { get; set; }

        public List<Beneficiary> Beneficiary { get; set; }
    }

}
