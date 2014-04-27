using System;
using System.Runtime.Serialization;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    [DataContract]
    public class RollInfo
    {
        [DataMember]
        public long Id;

        [DataMember]
        public String Series;

        [DataMember]
        public long NextTicket;

        [DataMember]
        public bool CanBeActivated;

        [DataMember]
        public bool IsActiveOnStation;

        [DataMember]
        public RollColor Color;

        [DataMember]
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
