using System;
using hessiancsharp.client;

namespace ASofCP.Cashier.Stores.Base
{
    public class CHessianStore<T> : ServerStore<T>
    {
        private readonly CHessianProxyFactory _factory;

        public CHessianStore(string url)
        {
            URL = url;
            _factory = new CHessianProxyFactory();
        }

        public CHessianStore(string url, string userName, string password)
        {
            URL = url;
            _factory = new CHessianProxyFactory(userName, password);
        } 

        public override void Save(T storeObject)
        {
            
            base.Save(storeObject);
        }

        public override T Load()
        {
            if (String.IsNullOrWhiteSpace(URL)) throw new Exception(String.Format("URL: \"{0}\" сервера не корректен!", URL));
            return (T)_factory.Create(typeof (T), URL);
        }
    }
}
