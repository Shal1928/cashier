using UseAbilities.IoC.Stores;

namespace ASofCP.Cashier.Stores.Base
{
    public class ServerStore<T> : StoreBase<T>
    {
        public virtual string URL
        {
            get; 
            set;
        }
    }

    //public class ServerStore : StoreBase<object>
    //{
    //    //public override string Load()
    //    //{
    //    //    //var file = new StreamReader(FileName);
    //    //    //var fileData = file.ReadLine();

    //    //    //file.Close();
    //    //    return fileData;
    //    //}
    //}
}
