using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Customers
{
    public class CustomerLoyalty
    {
        public int CustomerId { get; set; }

        public int SavingPoints { get; set; }

        public virtual Customer Customer { get; set; }

        public void IncreasePoints(int amount)
        {
            SavingPoints += amount;
        }

        public void DecreasePoints(int amount)
        {
            int newBalance = SavingPoints - amount;
            if (newBalance < 0)
                throw new InvalidOperationException("Balance not enough.");

            SavingPoints = newBalance;
        }
    }
}