using System;

namespace ASofCP.Cashier.Models.Contracts
{
    public class RollInfo
    {
        public String Series;

        public long NextTicket;

        public bool CanBeActivated;

        public bool IsActiveOnStation;

        public RollColor Color;


        public override string ToString()
        {
            return "RollInfo{" +
                "Series='" + Series + '\'' +
                ", NextTicket=" + NextTicket +
                ", CanBeActivated=" + CanBeActivated +
                ", IsActiveOnStation=" + IsActiveOnStation +
                ", Color=" + Color +
                '}';
        }
    }
}
