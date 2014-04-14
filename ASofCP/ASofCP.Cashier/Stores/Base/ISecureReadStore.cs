using UseAbilities.IoC.Stores;

namespace ASofCP.Cashier.Stores.Base
{
    public interface ISecureReadStore<T> : IReadStore<T>
    {
        void Logon(string login, string password);
    }
}
