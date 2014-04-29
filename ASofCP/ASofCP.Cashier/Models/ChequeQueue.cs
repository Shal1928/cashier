using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ASofCP.Cashier.Helpers;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.Models
{
    [DataContract]
    public class ChequeQueue
    {
        [DataMember]
        public List<ChequeQueueElement> Elements { get; set; }

        public void Add(Cheque cheque)
        {
            var guid = Guid.NewGuid().ToString();
            Add(new ChequeQueueElement(guid, cheque));
        }

        public void Add(ChequeQueueElement element)
        {
            if(element.IsNull()) return;
            Elements = Elements ?? new List<ChequeQueueElement>();

            if (Elements.NotNull() && !Elements.Exists(e => Equals(e, element))) Elements.Add(element);
            else Elements[Elements.IndexOf(element)] = element;
        }

        public void AddRange(IEnumerable<ChequeQueueElement> elements)
        {
            var chequeQueueElements = elements as IList<ChequeQueueElement> ?? elements.ToList();
            if (chequeQueueElements.IsNullOrEmpty()) return;

            foreach (var element in chequeQueueElements)
                Add(element);
        }

        //public ChequeQueue New(List<Cheque> cheques)
        //{
        //    var guid = Guid.NewGuid().ToString();
        //    return new ChequeQueue
        //    {
        //        Elements = new List<ChequeQueueElement>{new ChequeQueueElement(guid, cheques)}
        //    };
        //}

        //public ChequeQueue New(Cheque cheque)
        //{
        //    var guid = Guid.NewGuid().ToString();
        //    return new ChequeQueue
        //    {
        //        Elements = new List<ChequeQueueElement> { new ChequeQueueElement(guid, cheque) }
        //    };
        //}

        public IEnumerable<ChequeQueue> GetAllNew()
        {
            return Elements.IsNullOrEmpty() ? null : Elements.Where(e => e.IsNew).Select(element => new ChequeQueue {Elements = new List<ChequeQueueElement> {element}}).ToList();
        }

        public IEnumerable<ChequeQueue> GetAllDelete()
        {
            return Elements.IsNullOrEmpty() ? null : Elements.Where(e => e.IsDelete).Select(element => new ChequeQueue { Elements = new List<ChequeQueueElement> { element } }).ToList();
        }

        public string GetGuid()
        {
            if(Elements.IsNullOrEmpty()) throw new NullReferenceException("При запросе индентификатора, очередь не была опеределена!");
            if(Elements.Count >= 2) throw new Exception("Запрос идентификатора не возможен, т. к. коллекция элементов состоит больше чем из одного элемента!");

            return Elements.FirstOrDefault().Guid;
        }

        public void SetAllOld()
        {
            if (Elements.IsNullOrEmpty()) return;
            foreach (var element in Elements)
                element.IsNew = false;
        }

        public void RemoveAllDeleted()
        {
            Elements.RemoveAll(e => e.IsDelete);
        }

        //public List<ChequeQueueElement> GetElementsForShift(Shift shift)
        //{
        //    if (Elements.IsNullOrEmpty()) return null;

        //    return (from element in Elements 
        //            from cheque in element.Cheques 
        //            where cheque.Shift.Id == shift.Id 
        //            select element).ToList();
        //}
    }

    [DataContract]
    public class ChequeQueueElement
    {
        public ChequeQueueElement()
        {
            Cheques = new List<Cheque>();
            IsNew = false;
        }

        public ChequeQueueElement(string guid, Cheque cheque)
        {
            Guid = guid;
            Cheques = new List<Cheque>{cheque};
            IsNew = true;
        }

        public ChequeQueueElement(string guid, List<Cheque> cheques)
        {
            Guid = guid;
            Cheques = cheques;
            IsNew = true;
        }

        [DataMember]
        public List<Cheque> Cheques { get; set; }

        [DataMember]
        public string Guid { get; set; }

        public bool IsNew { get; set; }

        public bool IsDelete { get; set; }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var that = obj as ChequeQueueElement;
            if (that.IsNull()) return false;
            
            return Guid == that.Guid;
        }
    }
}
