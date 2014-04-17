using System;
using System.Collections.Generic;
using System.Linq;
using ASofCP.Cashier.Models.Base;

namespace ASofCP.Cashier.Models
{
    public class CashVoucher<T> : List<T> where T : class, ICashVoucherItem
    {
        public CashVoucher()
        {
            //
        }

        public CashVoucher(IEnumerable<T> collection)
        {
            AddAll(collection);
        }

        public new void Add(T item)
        {
            if (!Contains(item))
            {
                base.Add(item);
                return;
            }

            foreach (var i in this.Where(i => Equals(i, item)))
            {
                i.Count += item.Count;
                i.Order = item.Order;
                break;
            }
        }

        public void AddAll(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                Add(item);
        }

        public double GetTotal()
        {
            var t = this.Sum(item => item.Total);
            return t;
        }

        public int Number
        {
            get; 
            set;
        }

        public ICashVoucherItem Get(T item)
        {
            return !Contains(item) ? null : this.FirstOrDefault(i => Equals(i, item));
        }

        public ICashVoucherItem Get(string title)
        {
            return this.FirstOrDefault(i => string.Equals(i.Title, title, StringComparison.OrdinalIgnoreCase));
        }

        public CashVoucher<T> Clone()
        {
            var clone = new CashVoucher<T>();
            foreach (var original in this)
                clone.Add((T)original.Clone());

            return clone;
        }
    }
}
