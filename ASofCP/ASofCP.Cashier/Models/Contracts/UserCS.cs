using System;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    public class UserCS
    {
        public String Login;
        public String UserDisplayName;

        public String DisplayName
        {
            get { return UserDisplayName; }
        }

        public override string ToString()
        {
            return "UserCS{" +
                "Login='" + Login + '\'' +
                ", UserDisplayName='" + UserDisplayName + '\'' +
                '}';
        }
    }
}
