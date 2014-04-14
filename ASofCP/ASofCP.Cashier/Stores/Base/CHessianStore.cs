using System;
using ASofCP.Cashier.Helpers;
using hessiancsharp.client;

namespace ASofCP.Cashier.Stores.Base
{
    public abstract class CHessianStore<T> : ISecureReadStore<T>
    {
        private CHessianProxyFactory _factory;

        public abstract string URL { get; }

        protected CHessianStore()
        {
            _factory = new CHessianProxyFactory();
        }

        public T Load()
        {
            if (URL.IsNullOrEmptyOrSpaces()) throw new Exception(String.Format("URL: \"{0}\" сервера не корректен!", URL));
            return (T)_factory.Create(typeof (T), URL);
        }

        public T Load(int key)
        {
            throw new NotImplementedException();
        }

        #region Implementation of ISecureReadStore

        public void Logon(string login, string password)
        {
            _factory = new CHessianProxyFactory(login, password);
        }

        #endregion
    }
}
