using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WinAnyInARowGameConsole
{
    public enum Player { None, Human, AI }

    public class AnyInARowPlayer
    {
        public Player Player { get; }
        public string PlayerMarker { get; }
        public int PlayerId { get; }

        public AnyInARowPlayer(string playerMarker, int playerId, Player player)
        {
            PlayerMarker = playerMarker;
            PlayerId = playerId;
            Player = player;
        }
    }
}
