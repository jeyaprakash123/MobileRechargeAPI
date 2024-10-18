using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRecharge.Domain.Configuration
{
    public class Appsettings
    {
        public int MaxBeneficiaryPerUser { get; set; }
        public decimal UserMonthlyTopUpLimit { get; set; }
        public int ChargeFee { get; set; }
        public decimal MaximumRechargePerMonthForVerifiedUser { get; set; }
        public decimal MaximumRechargePerMonthForNotverifiedUser { get; set; }
    }
}
