using System;
using System.ComponentModel;
using System.Runtime.Serialization;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    [DataContract]
    public class RollColor
    {
        public static RollColor Default = new RollColor{Id = -1}; 

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
