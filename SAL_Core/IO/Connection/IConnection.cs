

namespace SAL_Core.IO.Connection
{
    interface IConnection
    {
        void Close();
        void Send(byte[] data);
        byte[] Receive(bool wait = false);
    }
}
