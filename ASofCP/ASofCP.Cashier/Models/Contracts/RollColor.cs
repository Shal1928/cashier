using System;
using System.Runtime.Serialization;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    [DataContract]
    public class RollColor
    {
        [DataMember]
        public String Color;

        [DataMember]
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
