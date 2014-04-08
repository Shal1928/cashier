using System;
using UseAbilities.IoC.Stores;
using hessiancsharp.client;

namespace ASofCP.Cashier.Stores.Base
{
    public class CHessianStore<T> : IReadStore<T>
    {
        private readonly CHessianProxyFactory _factory;

        public string URL
        {
            get;
            set;
        }

        public CHessianStore()
        {
            _factory = new CHessianProxyFactory();
        }

        public T Load()
        {
            if (String.IsNullOrWhiteSpace(URL)) throw new Exception(String.Format("URL: \"{0}\" сервера не корректен!", URL));
            return (T)_factory.Create(typeof (T), URL);
        }

        public T Load(int key)
        {
            throw new NotImplementedException();
        }
    }
}
