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

    //<ModuleSettings xmlns="http://schemas.datacontract.org/2004/07/ASofCP.Cashier.Models" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
    //    <IP>pl-e.ru</IP>
    //    <Id>02</Id>
    //    <Port>9898</Port>
    //    <PrinterName>Zebra</PrinterName>
    //    <PathToTemplate>PrintTemplate.xml</PathToTemplate>
    //    <PathToZpl>ZPLTemplate.zpl</PathToZpl>
    //</ModuleSettings>
}
