using System;

namespace ASofCP.Cashier.Models.Contracts
{
    public class AttractionInfo
    {
        public long Price;

        public long MinPrice;

        public String DisplayName;

        public String PrintName;

        public String VersionSeries;

        public String Code;

        public long Id;


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
                    '}';
        }
    }
}
