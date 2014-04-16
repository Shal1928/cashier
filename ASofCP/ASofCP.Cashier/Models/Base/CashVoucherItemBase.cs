using it.q02.asocp.api.data;

namespace ASofCP.Cashier.Models.Base
{
    public abstract class CashVoucherItemBase : ICashVoucherItem
    {
        #region Implementation of ICashVoucherItem

        public virtual string Title { get; set; }
        public virtual string PrintTitle { get; set; }

        public virtual double Price
        {
            get; 
            set;
        }
        
        private int _count;
        public virtual int Count
        {
            get
            {
                return _count;
            } 
            set
            {
                _count = value < 0 ? 0 : value;
            }
        }
        
        public virtual double Total
        {
            get
            {
                return Price * Count;
            }
        }

        public AttractionInfo AttractionInfo { get; protected set; }

        #endregion


        #region Overrided Methods

        public override string ToString()
        {
            return Title;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }

        public abstract object Clone();

        public override bool Equals(object obj)
        {
            if(obj == null || !(obj is ICashVoucherItem)) return false;

            var that = (ICashVoucherItem)obj;
            return Equals(Title, that.Title) && Equals(Price, that.Price);
        }

        #endregion
    }
}
