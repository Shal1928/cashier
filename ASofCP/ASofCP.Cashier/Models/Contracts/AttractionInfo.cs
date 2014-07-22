using System;
using System.Runtime.Serialization;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    [DataContract]
    public class AttractionInfo
    {
        [DataMember]
        public long Price;

        [DataMember]
        public long MinPrice;

        [DataMember]
        public String DisplayName;

        [DataMember]
        public String PrintName;

        [DataMember]
        public String VersionSeries;

        [DataMember]
        public String Code;

        [DataMember]
        public long Id;

        [DataMember]
        public long CategoryId;

        [DataMember]
        public String BackgroundColor;

        [DataMember]
        public String ForegroundColor;

        [DataMember]
        public long Number;



        public override String ToString()
        {
            return "AttractionInfo{" +
                    "Price=" + Price +
                    ", MinPrice=" + MinPrice +
                    ", DisplayName='" + DisplayName + '\'' +
                    ", PrintName='" + PrintName + '\'' +
                    ", VersionSeries='" + VersionSeries + '\'' +
                    ", Code='" + Code + '\'' +
                    ", Id=" + Id +
                    ", CategoryId=" + CategoryId +
                    ", BackgroundColor='" + BackgroundColor + '\'' +
                    ", ForegroundColor='" + ForegroundColor + '\'' +
                    ", Number=" + Number +
                    '}';
        }
    }
}
