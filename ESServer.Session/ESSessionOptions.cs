using System;

namespace com.erinus.ESServer
{
    public class ESSessionOptions
    {
        public enum StoreType { Memory, MongoDB };

        public StoreType Store;
    }
}
