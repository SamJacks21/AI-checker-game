using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA__window_
{
    public class AIMove
    {
        public int moveFromX;
        public int moveFromY;
        public int moveToX;
        public int moveToY;
        public bool jump;
        public bool king;
        public int value;

        public AIMove()
        {
            moveFromX = 0;
            moveFromY = 0;
            moveToX = 0;
            moveToY = 0;
            jump = false;
            king = false;
            value = 0;
        }



        public AIMove(int x, int y, int xx, int yy, bool jumps, bool kings, int v)
        {
            moveFromX = x;
            moveFromY = y;
            moveToX = xx;
            moveToY = yy;
            jump = jumps;
            king = kings;
            value = v;
        }
    }
}
