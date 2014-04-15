using System;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    public class RollInfo
    {
        public String Series;

        public long NextTicket;

        public bool CanBeActivated;

        public bool IsActiveOnStation;

        public RollColor Color;

        public long TicketsLeft;

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
