namespace taskt.Core.Common
{
    public static class Client
    {
        private static bool _engineBusy;
        public static bool EngineBusy
        {
            get
            {
                return _engineBusy;
            }
            set
            {
                _engineBusy = value;
               // Core.Sockets.SocketClient.SendMessage("CLIENT_STATUS=" + ClientStatus);
            }
        }

        public static string ClientStatus
        {
            get
            {
                if (EngineBusy)
                {
                    return "Client is Busy";
                }
                else
                {
                    return "Client is Available";
                }
            }
        }
    }
}
