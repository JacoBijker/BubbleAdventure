using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Player
{
    public class Player
    {
        public int Level { get; set; }
        public int Lives { get; set; }
        public int Score { get; set; }

        public int BubbleSize { get; set; }
        public int BubbleDuration { get; set; }

        public Player()
        {
            Lives = 4;
            Score = 0;
            Level = 1;
            BubbleDuration = 0;
            BubbleSize = 0;
        }
    }
}
