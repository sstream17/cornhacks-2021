using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderRoyale.Hubs
{
    public static class Lobby
    {
        public static bool inLobby = true;
        public static Dictionary<string, string> Players = new Dictionary<string, string>();
    }
}
