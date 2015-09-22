using System;

namespace com.erinus.ESServer
{
    public class ESSessionOptions
    {
        public enum StoreType { Memory, MongoDB, Redis };

        public StoreType Store;
    }
}
