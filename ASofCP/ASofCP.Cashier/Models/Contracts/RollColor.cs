using System;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
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
