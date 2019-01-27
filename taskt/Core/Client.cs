using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    public static class Client
    {


        private static bool engineBusy;
        public static bool EngineBusy
        {
            get
            {
                return engineBusy;
            }
            set
            {
                engineBusy = value;
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
