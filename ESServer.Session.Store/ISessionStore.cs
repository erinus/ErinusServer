using System;

namespace com.erinus.ESServer.Session.Store
{
    public interface ISessionStore
    {
        Boolean Has(String sessionKey);
        ISession Add(String sessionKey);
        ISession Get(String sessionKey);
    }
}
