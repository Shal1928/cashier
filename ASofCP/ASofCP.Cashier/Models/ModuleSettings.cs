using System;
using System.Runtime.Serialization;

namespace ASofCP.Cashier.Models
{
    [DataContract]
    public class ModuleSettings
    {
        [DataMember]
        public String IP { get; set; }

        [DataMember]
        public String Port { get; set; }

        [DataMember]
        public String Id { get; set; }

        [DataMember]
        public String PrinterName { get; set; }

        [DataMember]
        public String PathToTemplate { get; set; }

        [DataMember]
        public String PathToZpl { get; set; }
    }
}
