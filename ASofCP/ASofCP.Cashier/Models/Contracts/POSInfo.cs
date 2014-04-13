using System;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    public class POSInfo
    {
        public String GUID;
        public String DisplayName;

        public UserCS[] AvailableUsers;


        public override string ToString()
        {
            return "POSInfo{" +
                "GUID='" + GUID + '\'' +
                ", DisplayName='" + DisplayName + '\'' +
                ", AvailableUsers=" + AvailableUsers +
                '}';
        }
    }
}
