////using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA__window_
{
    public class ValidMove
    {
        public int x;
        public int y;
        public bool jump;

        public ValidMove()
        {
            x = 0;
            y = 0;
            jump = false;
        }



        public ValidMove(int xx, int yy, bool jumps)
        {
            x = xx;
            y = yy;
            jump = jumps;
        }
    }
}
