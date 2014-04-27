using System.Collections.Generic;
using System.Runtime.Serialization;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.Models
{
    [DataContract]
    public class ChequeQueue
    {
        [DataMember]
        public List<Cheque> Cheques { get; set; } 
    }
}
