using System;

namespace com.erinus.ESServer.Session.Store
{
    public interface ISession
    {
        Boolean Has(String key);
        T Get<T>(String key) where T : class;
        Boolean Set(String key, dynamic value);
    }
}
