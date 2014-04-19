using System;
using System.Runtime.Serialization;
using ASofCP.Cashier.Helpers;

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


        public override int GetHashCode()
        {
            return String.Format("{0}{1}{2}{3}{4}{5}", IP, Port, Id, PrinterName, PathToTemplate, PathToZpl).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var that = obj as ModuleSettings;
            if (that.IsNull()) return false;

            // ReSharper disable PossibleNullReferenceException
            return Equals(IP, that.IP) && Equals(Port, that.Port) && Equals(Id, that.Id) && Equals(PrinterName, that.PrinterName) &&
                   Equals(PathToTemplate, that.PathToTemplate) && Equals(PathToZpl, that.PathToZpl);
            // ReSharper restore PossibleNullReferenceException
        }
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
