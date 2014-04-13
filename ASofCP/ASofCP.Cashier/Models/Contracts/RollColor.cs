using System;

namespace ASofCP.Cashier.Models.Contracts
{
    public class RollColor
    {
        public String Color;
        public long Id;

        public override string ToString()
        {
            return "RollColor{" +
                "Color='" + Color + '\'' +
                ", Id=" + Id +
                '}';
        }
    }
}
