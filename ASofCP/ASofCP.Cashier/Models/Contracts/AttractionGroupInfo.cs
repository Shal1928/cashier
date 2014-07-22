using System;
// ReSharper disable CheckNamespace


namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    public class AttractionGroupInfo
    {
        public long Id;

        public String Title;

        public String BackgroundColor;

        public String ForegroundColor;

        public long Number;

        public int Type;



        public override String ToString()
        {
            return "AttractionGroupInfo{" +
                    "Id=" + Id +
                    ", Title='" + Title + '\'' +
                    ", BackgroundColor='" + BackgroundColor + '\'' +
                    ", ForegroundColor='" + ForegroundColor + '\'' +
                    ", Number=" + Number +
                    ", Type=" + Type +
                    '}';
        }
    }
}
