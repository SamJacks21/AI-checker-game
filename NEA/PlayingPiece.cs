using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA__window_
{
    public class PlayingPiece
    {
        public bool king;
        public int player;
        public int x;
        public int y;


        public void setPlayer(int inputP)
        {
            player = inputP;
        }



        public PlayingPiece()
        {
            x = 0;
            y = 0;
            king = false;
        }



        public PlayingPiece(int xx, int yy, bool k)
        {
            x = xx;
            y = yy;
            king = k;
        }
    }
}
