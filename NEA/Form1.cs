using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA__window_
{

    // Sam Jacks
    // Minimax checkers


    public partial class Form1 : Form
    {
        // Setup classes, lists and arrays that are needed
        public static char[,] Board = new char[8, 8];
        public static List<PlayingPiece> P1 = new List<PlayingPiece>();
        public static List<PlayingPiece> P2 = new List<PlayingPiece>();
        public static List<PlayingPiece> P1Tester = new List<PlayingPiece>();
        public static List<PlayingPiece> P2Tester = new List<PlayingPiece>();
        public static List<ValidMove> valid = new List<ValidMove>();
        public static List<AIMove> Ai = new List<AIMove>();
        public static List<AIMove> AiTwoPoint = new List<AIMove>();
        public static List<placeNumbers> place = new List<placeNumbers>();

        // Setup of global variables that are needed throughout but cannot be defined within the functions they are used 
        public static int savedCoorX;
        public static int savedCoorY;
        public static int placeNumber;
        public static bool isKing;
        public static bool isJump;
        public static bool yourMove = true;
        public static bool click = false;

        public Form1()
        {
            InitializeComponent();


            // Calls the function to create the starting pieces
            boardSetup();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {


            // Stops the paint looking pixalated
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            // Sets up the border and counter to draw the board
            int counter = 9;
            int Y1 = 10;
            int Y2 = 10;
            int X1 = 10;
            int X2 = 10;
            int oakY = 90;
            int oakX = 10;
            int biegeY = 10;
            int biegeX = 10;


            // Seting up the colours to create the board
            SolidBrush beige = new SolidBrush(Color.FromArgb(255, 207, 185, 151));
            SolidBrush oak = new SolidBrush(Color.FromArgb(255, 120, 81, 45));
            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));


            // Setting up the colours to create the pieces 
            SolidBrush black = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            SolidBrush red = new SolidBrush(Color.FromArgb(255, 255, 0, 0));
            SolidBrush white = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            SolidBrush blue = new SolidBrush(Color.FromArgb(255, 0, 0, 255));
            Pen P1King = new Pen(Color.FromArgb(255, 0, 0, 0), 10);
            Pen P2King = new Pen(Color.FromArgb(255, 255, 0, 0), 10);


            // Creates all the black and white squares
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    e.Graphics.FillRectangle(beige, biegeY, biegeX, 80, 80);
                    biegeY = biegeY + 160;
                    e.Graphics.FillRectangle(oak, oakY, oakX, 80, 80);
                    oakY = oakY + 160;
                }
                biegeY = 90;
                oakY = 10;

                if (i % 2 != 0)
                {
                    biegeY = 10;
                    oakY = 90;
                }
                biegeX = biegeX + 80;
                oakX = oakX + 80;
            }


            // Creates the black outline to make it more pleasing on the eyes
            for (int i = 0; i < counter; i++)
            {
                e.Graphics.DrawLine(pen, 10, Y1, 650, Y2);
                Y1 = Y1 + 80;
                Y2 = Y2 + 80;
            }
            for (int x = 0; x < counter; x++)
            {
                e.Graphics.DrawLine(pen, X1, 10, X2, 650);
                X1 = X1 + 80;
                X2 = X2 + 80;
            }


            // Goes through the previously created P1 piece lsit and draws all the pieces
            counter = 0;
            foreach (var CoordX in P1)
            {
                int x = P1[counter].x;
                x = x * 80;
                x = x + 15;

                int y = P1[counter].y;
                y = y * 80;
                y = y + 15;

                bool king = P1[counter].king;

                if (king == true)
                {
                    e.Graphics.DrawEllipse(P1King, x, y, 70, 70);
                    e.Graphics.FillEllipse(white, x, y, 70, 70);
                }
                else
                {
                    e.Graphics.DrawEllipse(pen, x, y, 70, 70);
                    e.Graphics.FillEllipse(black, x, y, 70, 70);
                }
                counter++;
            }


            // Goes through the previously created P2 piece lsit and draws all the pieces
            counter = 0;
            foreach (var CoordX in P2)
            {
                int x = P2[counter].x;
                x = x * 80;
                x = x + 15;

                int y = P2[counter].y;
                y = y * 80;
                y = y + 15;

                bool king = P2[counter].king;

                if (king == true)
                {
                    e.Graphics.DrawEllipse(P2King, x, y, 70, 70);
                    e.Graphics.FillEllipse(white, x, y, 70, 70);
                }
                else
                {
                    e.Graphics.DrawEllipse(pen, x, y, 70, 70);
                    e.Graphics.FillEllipse(red, x, y, 70, 70);
                }
                counter++;
            }


            // Draws all the valid moves
            counter = 0;
            foreach (var item in valid)
            {
                int x = valid[counter].x;
                x = x * 80;
                x = x + 15;

                int y = valid[counter].y;
                y = y * 80;
                y = y + 15;

                e.Graphics.FillEllipse(blue, x, y, 70, 70);

                counter++;
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

            int CoorX = 0;
            int CoorY = 0;
            int newCoorX = 0;
            int newCoorY = 0;
            int validX = 0;
            int validY = 0;


            bool validPlayerOneMove;
            bool validPlayerTwoMove;
            bool validMove;
            bool king;

            Func<int, int, int> plus = (a, b) => a + b;
            Func<int, int, int> sub = (a, b) => a - b;


            // Turns the gotton coordinates into the coordinates used in the list for simplicicity
            CoorX = e.X;
            CoorY = e.Y;
            CoorX = CoorX - 10;
            CoorX = CoorX / 80;
            CoorY = CoorY - 10;
            CoorY = CoorY / 80;
            int count = 0;

            // Checks the valid moves
            int unusedCount = 0;
            foreach (var item in P1)
            {
                int x = P1[unusedCount].x;
                int y = P1[unusedCount].y;
                king = P1[unusedCount].king;


                if ((CoorX == x) && (CoorY == y))
                {
                    savedCoorX = CoorX;
                    savedCoorY = CoorY;
                    valid.Clear();
                    int mainCounter = 0;
                    validX = 0;
                    validY = 0;
                    int jumpX = 0;
                    int jumpY = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == 0)
                        {
                            validX = (sub(CoorX, 1));
                            validY = (sub(CoorY, 1));
                            jumpX = (sub(CoorX, 2));
                            jumpY = (sub(CoorY, 2));
                            mainCounter = 0;
                        }
                        if (i == 1)
                        {
                            validX = (plus(CoorX, 1));
                            validY = (sub(CoorY, 1));
                            jumpX = (plus(CoorX, 2));
                            jumpY = (sub(CoorY, 2));
                            mainCounter = 0;
                        }
                        if (i == 2)
                        {
                            if (king == false)
                            {
                                break;
                            }
                            validX = (plus(CoorX, 1));
                            validY = (plus(CoorY, 1));
                            jumpX = (plus(CoorX, 2));
                            jumpY = (plus(CoorY, 2));
                            mainCounter = 0;
                        }
                        if (i == 3)
                        {
                            if (king == false)
                            {
                                break;
                            }
                            validX = (sub(CoorX, 1));
                            validY = (plus(CoorY, 1));
                            jumpX = (sub(CoorX, 2));
                            jumpY = (plus(CoorY, 2));
                            mainCounter = 0;
                        }
                        foreach (var Coord in P1)
                        {
                            x = P1[mainCounter].x;
                            y = P1[mainCounter].y;
                            if (x == CoorX && y == CoorY)
                            {
                                validPlayerOneMove = true;
                                validPlayerTwoMove = true;
                                validMove = true;
                                int counter = 0;
                                foreach (var entity in P1)
                                {
                                    x = P1[counter].x;
                                    y = P1[counter].y;
                                    if ((x == validX) && (y == validY))
                                    {
                                        validPlayerOneMove = false;
                                    }
                                    counter++;
                                }
                                counter = 0;
                                foreach (var entity in P2)
                                {
                                    x = P2[counter].x;
                                    y = P2[counter].y;
                                    if ((x == validX) && (y == validY))
                                    {
                                        validPlayerTwoMove = false;
                                    }
                                    counter++;
                                }
                                if ((validPlayerOneMove == true) && (validPlayerTwoMove == true))
                                {
                                    if ((validX >= 0) && (validX <= 7) && (validY >= 0) && (validY <= 7))
                                    {
                                        valid.Add(new ValidMove(validX, validY, false));
                                    }
                                }
                                if ((validPlayerOneMove == true) && (validPlayerTwoMove == false))
                                {
                                    validMove = true;
                                    counter = 0;
                                    foreach (var entity in P1)
                                    {
                                        x = P1[counter].x;
                                        y = P1[counter].y;
                                        if ((x == jumpX) && (y == jumpY))
                                        {
                                            validMove = false;
                                        }
                                        counter++;
                                    }

                                    counter = 0;
                                    foreach (var entity in P2)
                                    {
                                        x = P2[counter].x;
                                        y = P2[counter].y;
                                        if ((x == jumpX) && (y == jumpY))
                                        {
                                            validMove = false;
                                        }
                                        counter++;
                                    }
                                    if (validMove == true)
                                    {
                                        if ((jumpX >= 0) && (jumpX <= 7) && (jumpY >= 0) && (jumpY <= 7))
                                        {
                                            valid.Add(new ValidMove(jumpX, jumpY, true));
                                        }
                                    }
                                    counter++;
                                }

                            }
                            mainCounter++;
                        }
                    }
                }
                unusedCount++;
            }

            // Makes sure the second click is a valid move and executes the move
            unusedCount = 0;
            foreach (var item in valid)
            {
                int x = valid[unusedCount].x;
                int y = valid[unusedCount].y;

                if ((CoorX == x) && (CoorY == y))
                {
                    newCoorX = CoorX;
                    newCoorY = CoorY;
                    CoorX = savedCoorX;
                    CoorY = savedCoorY;

                    if ((newCoorX != CoorX) && (newCoorY != CoorY))
                    {
                        int mainCounter = 0;
                        foreach (var coord in valid)
                        {
                            validMove = false;
                            validX = valid[mainCounter].x;
                            validY = valid[mainCounter].y;
                            bool jump = valid[mainCounter].jump;

                            if ((validX == newCoorX) & (validY == newCoorY))
                            {
                                validMove = true;
                                int counter = 0;
                                foreach (var entity in P1)
                                {
                                    x = P1[counter].x;
                                    y = P1[counter].y;
                                    king = P1[counter].king;

                                    if ((x == CoorX) && (y == CoorY))
                                    {
                                        isKing = king;
                                        isJump = jump;
                                    }
                                    counter++;
                                }
                            }

                            mainCounter++;
                            if (validMove == true)
                            {
                                if (isJump == true)
                                {
                                    king = isKing;
                                    count = 0;
                                    foreach (var entity in P1)
                                    {
                                        x = P1[count].x;
                                        y = P1[count].y;

                                        if (x == CoorX && y == CoorY)
                                        {
                                            placeNumber = count;
                                            break;
                                        }
                                        count++;

                                    }

                                    if (CoorX - newCoorX == -2 && CoorY - newCoorY == 2)
                                    {
                                        if (king == true)
                                        {
                                            P1.RemoveAt(placeNumber);
                                            P1.Add(new PlayingPiece(newCoorX, newCoorY, true));
                                        }
                                        else
                                        {
                                            P1.RemoveAt(placeNumber);
                                            P1.Add(new PlayingPiece(newCoorX, newCoorY, false));
                                        }

                                        count = 0;
                                        foreach (var entity in P2)
                                        {
                                            x = P2[count].x;
                                            y = P2[count].y;
                                            if (x == newCoorX - 1 && y == newCoorY + 1)
                                            {
                                                P2.RemoveAt(count);
                                                Refresh();
                                                yourMove = false;
                                                break;
                                            }
                                            count++;
                                        }
                                    }
                                    if (CoorX - newCoorX == 2 && CoorY - newCoorY == 2)
                                    {
                                        if (king == true)
                                        {
                                            P1.RemoveAt(placeNumber);
                                            P1.Add(new PlayingPiece(newCoorX, newCoorY, true));
                                        }
                                        else
                                        {
                                            P1.RemoveAt(placeNumber);
                                            P1.Add(new PlayingPiece(newCoorX, newCoorY, false));
                                        }

                                        count = 0;
                                        foreach (var entity in P2)
                                        {
                                            x = P2[count].x;
                                            y = P2[count].y;
                                            if (x == newCoorX + 1 && y == newCoorY + 1)
                                            {
                                                P2.RemoveAt(count);
                                                Refresh();
                                                yourMove = false;
                                                break;
                                            }
                                            count++;
                                        }
                                    }
                                    if (CoorX - newCoorX == -2 && CoorY - newCoorY == -2)
                                    {
                                        if (king == true)
                                        {
                                            P1.RemoveAt(placeNumber);
                                            P1.Add(new PlayingPiece(newCoorX, newCoorY, true));
                                        }
                                        else
                                        {
                                            P1.RemoveAt(placeNumber);
                                            P1.Add(new PlayingPiece(newCoorX, newCoorY, false));
                                        }

                                        count = 0;
                                        foreach (var entity in P2)
                                        {
                                            x = P2[count].x;
                                            y = P2[count].y;
                                            if (x == newCoorX - 1 && y == newCoorY - 1)
                                            {
                                                P2.RemoveAt(count);
                                                Refresh();
                                                yourMove = false;
                                                break;
                                            }
                                            count++;
                                        }
                                    }
                                    if (CoorX - newCoorX == 2 && CoorY - newCoorY == -2)
                                    {
                                        if (king == true)
                                        {
                                            P1.RemoveAt(placeNumber);
                                            P1.Add(new PlayingPiece(newCoorX, newCoorY, true));
                                        }
                                        else
                                        {
                                            P1.RemoveAt(placeNumber);
                                            P1.Add(new PlayingPiece(newCoorX, newCoorY, false));
                                        }

                                        count = 0;
                                        foreach (var entity in P2)
                                        {
                                            x = P2[count].x;
                                            y = P2[count].y;
                                            if (x == newCoorX + 1 && y == newCoorY - 1)
                                            {
                                                P2.RemoveAt(count);
                                                Refresh();
                                                yourMove = false;
                                                break;
                                            }
                                            count++;
                                        }
                                    }
                                }
                                if (isJump == false)
                                {
                                    count = 0;
                                    foreach (var entity in P1)
                                    {
                                        x = P1[count].x;
                                        y = P1[count].y;

                                        if (x == CoorX && y == CoorY)
                                        {
                                            placeNumber = count;
                                            break;
                                        }
                                        count++;
                                    }

                                    if (isKing == true)
                                    {
                                        P1.RemoveAt(placeNumber);
                                        P1.Add(new PlayingPiece(newCoorX, newCoorY, true));
                                        Refresh();
                                        yourMove = false;
                                    }
                                    else
                                    {
                                        P1.RemoveAt(placeNumber);
                                        P1.Add(new PlayingPiece(newCoorX, newCoorY, false));
                                        Refresh();
                                        yourMove = false;
                                    }
                                }
                            }
                        }
                    }
                }
                unusedCount++;
            }

            // Win check
            count = P2.Count();
            if (count == 0)
            {
                MessageBox.Show("Congratulations you beat the computer! ");
            }

            // Once the valid move has been done it sets a your move boolean to false
            if (yourMove == false)
            {
                valid.Clear();
                Refresh();
                artificialIntelligence(Board);

                // King check
                count = P2.Count();
                count--;
                for (int i = count; i != -1; i--)
                {
                    int x = P2[i].x;
                    int y = P2[i].y;
                    king = P2[i].king;
                    if (y == 7)
                    {
                        P2.RemoveAt(i);
                        P2.Add(new PlayingPiece(x, y, true));
                    }
                    count++;
                }

                System.Threading.Thread.Sleep(1000);
                Refresh();

            }

            // Win check
            count = P1.Count();
            if (count == 0)
            {
                MessageBox.Show("Unfortunetly the computer beat you! ");
            }

            // King check
            count = P1.Count();
            count--;
            for (int i = count; i != -1; i--)
            {
                int x = P1[i].x;
                int y = P1[i].y;
                king = P1[i].king;
                if (y == 0)
                {
                    P1.RemoveAt(i);
                    P1.Add(new PlayingPiece(x, y, true));
                }
                count++;
            }



            Refresh();
        }

        public static void boardSetup()
        {

            P1.Clear();
            P2.Clear();


            // This is where the lists of pieces are set
            int p1x = 0;
            int p1y = 7;
            int p2y = 0;
            int p2x = 1;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    P1.Add(new PlayingPiece(p1x, p1y, false));
                    p1x = p1x + 2;
                    P2.Add(new PlayingPiece(p2x, p2y, false));
                    p2x = p2x + 2;
                }
                p1y = p1y - 1;
                p1x = 0;
                if (p1y == 6)
                {
                    p1x = 1;
                }

                p2y = p2y + 1;
                p2x = 1;
                if (p2y == 1)
                {
                    p2x = 0;
                }
            }

        }

        public static void artificialIntelligence(char[,] Board)
        {
            int CoorX = 0;
            int CoorY = 0;
            int validY = 0;
            int validX = 0;
            int jumpX = 0;
            int jumpY = 0;

            int counter = 0;
            int count = 0;

            Ai.Clear();
            AiTwoPoint.Clear();
            place.Clear();


            Func<int, int, int> plus = (a, b) => a + b;
            Func<int, int, int> sub = (a, b) => a - b;


            int mainsCounter = 0;


            // Checks through every Player 2 piece and out puts all valid moves
            foreach (var entity in P2)
            {
                CoorX = P2[mainsCounter].x;
                CoorY = P2[mainsCounter].y;
                int P2X = P2[mainsCounter].x;
                int P2Y = P2[mainsCounter].y;
                bool king = P2[mainsCounter].king;
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        validX = (sub(CoorX, 1));
                        validY = (plus(CoorY, 1));
                        jumpX = (sub(CoorX, 2));
                        jumpY = (plus(CoorY, 2));

                    }
                    if (i == 1)
                    {
                        validX = (plus(CoorX, 1));
                        validY = (plus(CoorY, 1));
                        jumpX = (plus(CoorX, 2));
                        jumpY = (plus(CoorY, 2));
                    }
                    if (i == 2)
                    {
                        if (king == false)
                        {
                            break;
                        }
                        validX = (sub(CoorX, 1));
                        validY = (sub(CoorY, 1));
                        jumpX = (sub(CoorX, 2));
                        jumpY = (sub(CoorY, 2));
                    }
                    if (i == 3)
                    {
                        if (king == false)
                        {
                            break;
                        }
                        validX = (plus(CoorX, 1));
                        validY = (sub(CoorY, 1));
                        jumpX = (plus(CoorX, 2));
                        jumpY = (sub(CoorY, 2));
                    }
                    if (P2X == CoorX && P2Y == CoorY)
                    {
                        bool validPlayerOneMove = true;
                        bool validPlayerTwoMove = true;
                        bool validMove = true;
                        counter = 0;
                        foreach (var item in P1)
                        {
                            int x = P1[counter].x;
                            int y = P1[counter].y;
                            if ((x == validX) && (y == validY))
                            {
                                validPlayerOneMove = false;
                            }
                            counter++;
                        }
                        counter = 0;
                        foreach (var item in P2)
                        {
                            int x = P2[counter].x;
                            int y = P2[counter].y;
                            if ((x == validX) && (y == validY))
                            {
                                validPlayerTwoMove = false;
                            }
                            counter++;
                        }
                        if ((validPlayerOneMove == true) && (validPlayerTwoMove == true))
                        {
                            if ((validX >= 0) && (validX <= 7))
                            {
                                if ((validY >= 0) && (validY <= 7))
                                {
                                    Ai.Add(new AIMove(CoorX, CoorY, validX, validY, false, king, 0));
                                }

                            }
                        }
                        if ((validPlayerOneMove == false) && (validPlayerTwoMove == true))
                        {
                            validMove = true;
                            counter = 0;
                            foreach (var item in P1)
                            {
                                int x = P1[counter].x;
                                int y = P1[counter].y;
                                if ((x == jumpX) && (y == jumpY))
                                {
                                    validMove = false;
                                }
                                counter++;
                            }

                            counter = 0;
                            foreach (var item in P2)
                            {
                                int x = P2[counter].x;
                                int y = P2[counter].y;
                                if ((x == jumpX) && (y == jumpY))
                                {
                                    validMove = false;
                                }
                                counter++;
                            }
                            if (validMove == true)
                            {
                                if ((jumpX >= 0) && (jumpX <= 7) && (jumpY >= 0) && (jumpY <= 7))
                                {
                                    Ai.Add(new AIMove(CoorX, CoorY, jumpX, jumpY, true, king, 0));
                                }

                            }
                            counter++;
                        }
                    }

                }
                mainsCounter++;
            }

            // minimax where the computer simulates each move and gives it a value rating ranging between 1 and 4
            int maincounter = 0;
            foreach (var item in Ai)
            {
                int fromX = Ai[maincounter].moveFromX;
                int fromY = Ai[maincounter].moveFromY;
                CoorX = Ai[maincounter].moveToX;
                CoorY = Ai[maincounter].moveToY;
                bool king = Ai[maincounter].king;
                bool jump = Ai[maincounter].jump;
                count = 0;

                P2Tester.Clear();
                P1Tester.Clear();




                foreach (var q in P1)
                {
                    int x = P1[count].x;
                    int y = P1[count].y;
                    king = P1[count].king;

                    P1Tester.Add(new PlayingPiece(x, y, king));
                    count++;
                }

                count = 0;
                foreach (var q in P2)
                {
                    int x = P2[count].x;
                    int y = P2[count].y;
                    king = P2[count].king;

                    P2Tester.Add(new PlayingPiece(x, y, king));
                    count++;
                }

                count = 0;
                foreach (var q in P2Tester)
                {
                    int x = P2Tester[count].x;
                    int y = P2Tester[count].y;

                    if (fromX == x && fromY == y)
                    {

                        P2Tester.RemoveAt(count);
                        P2Tester.Add(new PlayingPiece(CoorX, CoorY, king));
                        break;
                    }
                    count++;
                }


                int counterX = 0;
                int counterY = 0;
                count = 0;
                foreach (var q in Board)
                {
                    Board[counterX, counterY] = '-';
                    counterY++;
                    if (counterY % 8 == 0)
                    {
                        counterX++;
                        counterY = 0;
                    }
                }

                foreach (var iatem in P1Tester)
                {
                    int x = P1Tester[count].x;
                    int y = P1Tester[count].y;
                    king = P1Tester[count].king;

                    if (king == true)
                    {
                        Board[x, y] = 'P';
                    }
                    else
                    {
                        Board[x, y] = 'p';
                    }
                    count++;
                }

                count = 0;
                foreach (var q in P2Tester)
                {
                    CoorX = P2Tester[count].x;
                    CoorY = P2Tester[count].y;

                    if (CoorX >= 0 || CoorX <= 7 || CoorY >= 0 || CoorY <= 7)
                    {
                        place.Add(new placeNumbers(count));
                    }
                    count++;
                }
                count = place.Count();

                for (int i = count; i < -1; i--)
                {
                    int x = place[i].x;
                    P2Tester.RemoveAt(x);
                }

                count = 0;
                foreach (var w in P2Tester)
                {
                    int x = P2Tester[count].x;
                    int y = P2Tester[count].y;
                    king = P2Tester[count].king;



                    if (king == true)
                    {
                        Board[x, y] = 'A';
                    }
                    if (king != true)
                    {
                        Board[x, y] = 'a';
                    }
                    count++;
                }

                count = 0;
                if (jump == true)
                {

                    if (fromX - CoorX == -2 && fromY - CoorY == 2)
                    {
                        if (king == true)
                        {
                            P2Tester.Add(new PlayingPiece(CoorX, CoorY, true));
                        }
                        else
                        {
                            P2Tester.Add(new PlayingPiece(CoorX, CoorY, false));
                        }

                        count = 0;
                        foreach (var entity in P1Tester)
                        {
                            fromX = P1Tester[count].x;
                            fromY = P1Tester[count].y;
                            if (fromX == CoorX - 1 && fromY == CoorY + 1)
                            {
                                P1Tester.RemoveAt(count);
                                break;
                            }
                            count++;
                        }
                    }
                    if (fromX - CoorX == 2 && fromY - CoorY == 2)
                    {
                        if (king == true)
                        {
                            P2Tester.Add(new PlayingPiece(CoorX, CoorY, true));
                        }
                        else
                        {
                            P2Tester.Add(new PlayingPiece(CoorX, CoorY, false));
                        }
                        count = 0;
                        foreach (var z in P1Tester)
                        {
                            fromX = P1Tester[count].x;
                            fromY = P1Tester[count].y;
                            if (fromX == CoorX + 1 && fromY == CoorY + 1)
                            {
                                P1Tester.RemoveAt(count);
                                break;
                            }
                            count++;
                        }


                    }
                    if (fromX - CoorX == -2 && fromY - CoorY == -2)
                    {
                        if (king == true)
                        {
                            P2Tester.Add(new PlayingPiece(CoorX, CoorY, true));
                        }
                        else
                        {
                            P2Tester.Add(new PlayingPiece(CoorX, CoorY, false));
                        }

                        count = 0;
                        foreach (var a in P1Tester)
                        {
                            fromX = P1Tester[count].x;
                            fromY = P1Tester[count].y;
                            if (fromX == CoorX - 1 && fromY == CoorY - 1)
                            {
                                P1Tester.RemoveAt(count);
                                break;
                            }
                            count++;
                        }



                    }
                    if (fromX - CoorX == 2 && fromY - CoorY == -2)
                    {
                        if (king == true)
                        {
                            P2Tester.Add(new PlayingPiece(CoorX, CoorY, true));
                        }
                        else
                        {
                            P2Tester.Add(new PlayingPiece(CoorX, CoorY, false));
                        }

                        count = 0;

                        foreach (var a in P1Tester)
                        {
                            fromX = P1Tester[count].x;
                            fromY = P1Tester[count].y;
                            if (fromX == CoorX + 1 && fromY == CoorY - 1)
                            {
                                P1Tester.RemoveAt(count);
                                break;
                            }
                            count++;
                        }
                    }



                    counterX = 0;
                    counterY = 0;
                    count = 0;
                    foreach (var q in Board)
                    {
                        Board[counterX, counterY] = '-';
                        counterY++;
                        if (counterY % 8 == 0)
                        {
                            counterX++;
                            counterY = 0;
                        }
                    }

                    foreach (var iatem in P1Tester)
                    {
                        int x = P1Tester[count].x;
                        int y = P1Tester[count].y;
                        king = P1Tester[count].king;

                        if (king == true)
                        {
                            Board[x, y] = 'P';
                        }
                        else
                        {
                            Board[x, y] = 'p';
                        }
                        count++;
                    }

                    count = 0;
                    foreach (var w in P2Tester)
                    {
                        int x = P2Tester[count].x;
                        int y = P2Tester[count].y;
                        king = P2Tester[count].king;

                        if (king == true)
                        {
                            Board[x, y] = 'A';
                        }
                        else
                        {
                            Board[x, y] = 'a';
                        }
                        count++;
                    }

                    bool one = true;
                    bool two = true;
                    bool three = true;
                    bool four = true;
                    int i = 0;
                    char symbol;
                    char sign;
                    if (i == 0)
                    {
                        bool validP = true;
                        bool validDash = true;

                        int validx = (plus(CoorX, 1));
                        int validy = (plus(CoorY, 1));
                        if (validx <= 7 && validx >= 0 && validy <= 7 && validy >= 0)
                        {
                            sign = Board[validx, validy];

                            if (sign == 'p' || sign == 'P')
                            {
                                validP = false;
                            }
                        }

                        int jumpx = (sub(CoorX, 1));
                        int jumpy = (sub(CoorY, 1));
                        if (jumpx <= 7 && jumpx >= 0 && jumpy <= 7 && jumpy >= 0)
                        {
                            symbol = Board[jumpx, jumpy];

                            if (symbol == '-')
                            {
                                validDash = false;
                            }
                        }

                        if (validP == false && validDash == false)
                        {
                            one = true;
                        }
                        else
                        {
                            one = false;
                        }
                        i++;
                    }

                    if (i == 1)
                    {
                        bool validP = true;
                        bool validDash = true;

                        int validx = (plus(CoorX, 1));
                        int validy = (sub(CoorY, 1));
                        if (validx <= 7 && validx >= 0 && validy <= 7 && validy >= 0)
                        {
                            sign = Board[validx, validy];

                            if (sign == 'p' || sign == 'P')
                            {
                                validP = false;
                            }
                        }

                        int jumpx = (sub(CoorX, 1));
                        int jumpy = (plus(CoorY, 1));
                        if (jumpx <= 7 && jumpx >= 0 && jumpy <= 7 && jumpy >= 0)
                        {
                            symbol = Board[jumpx, jumpy];

                            if (symbol == '-')
                            {
                                validDash = false;
                            }
                        }


                        if (validP == false && validDash == false)
                        {
                            two = true;
                        }
                        else
                        {
                            two = false;
                        }
                        i++;
                    }

                    if (i == 2)
                    {
                        bool validP = true;
                        bool validDash = true;

                        int validx = (plus(CoorX, 1));
                        int validy = (plus(CoorY, 1));
                        if (validx <= 7 && validx >= 0 && validy <= 7 && validy >= 0)
                        {
                            sign = Board[validx, validy];

                            if (sign == 'P')
                            {
                                validP = false;
                            }
                        }

                        int jumpx = (sub(CoorX, 1));
                        int jumpy = (sub(CoorY, 1));
                        if (jumpx <= 7 && jumpx >= 0 && jumpy <= 7 && jumpy >= 0)
                        {
                            symbol = Board[jumpx, jumpy];

                            if (symbol == '-')
                            {
                                validDash = false;
                            }
                        }
                        if (validP == false && validDash == false)
                        {
                            three = true;
                        }
                        else
                        {
                            three = false;
                        }
                        i++;
                    }

                    if (i == 3)
                    {
                        bool validP = true;
                        bool validDash = true;

                        int validx = (sub(CoorX, 1));
                        int validy = (plus(CoorY, 1));
                        if (validx <= 7 && validx >= 0 && validy <= 7 && validy >= 0)
                        {
                            sign = Board[validx, validy];

                            if (sign == 'P')
                            {
                                validP = false;
                            }
                        }

                        int jumpx = (plus(CoorX, 1));
                        int jumpy = (sub(CoorY, 1));
                        if (jumpx <= 7 && jumpx >= 0 && jumpy <= 7 && jumpy >= 0)
                        {
                            symbol = Board[jumpx, jumpy];

                            if (symbol == '-')
                            {
                                validDash = false;
                            }
                        }
                        if (validP == false && validDash == false)
                        {
                            four = true;
                        }
                        else
                        {
                            four = false;
                        }
                        i++;
                    }
                    if (one == true || two == true || three == true || four == true)
                    {
                        Ai[maincounter].value = 2;
                    }
                    else
                    {
                        Ai[maincounter].value = 4;
                    }
                }
                else
                {
                    bool one = true;
                    bool two = true;
                    bool three = true;
                    bool four = true;
                    int i = 0;
                    char symbol;
                    char sign;
                    if (i == 0)
                    {
                        bool validP = true;
                        bool validDash = true;

                        int validx = (plus(CoorX, 1));
                        int validy = (plus(CoorY, 1));
                        if (validx <= 7 && validx >= 0 && validy <= 7 && validy >= 0)
                        {
                            sign = Board[validx, validy];

                            if (sign == 'p' || sign == 'P')
                            {
                                validP = false;
                            }
                        }


                        int jumpx = (sub(CoorX, 1));
                        int jumpy = (sub(CoorY, 1));
                        if (jumpx <= 7 && jumpx >= 0 && jumpy <= 7 && jumpy >= 0)
                        {
                            symbol = Board[jumpx, jumpy];

                            if (symbol == '-')
                            {
                                validDash = false;
                            }
                        }
                        if (validP == false && validDash == false)
                        {
                            one = true;
                        }
                        else
                        {
                            one = false;
                        }
                        i++;
                    }

                    if (i == 1)
                    {
                        bool validP = true;
                        bool validDash = true;

                        int validx = (plus(CoorX, 1));
                        int validy = (sub(CoorY, 1));
                        if (validx <= 7 && validx >= 0 && validy <= 7 && validy >= 0)
                        {
                            sign = Board[validx, validy];

                            if (sign == 'p' || sign == 'P')
                            {
                                validP = false;
                            }
                        }
                        int jumpx = (sub(CoorX, 1));
                        int jumpy = (plus(CoorY, 1));
                        if (jumpx <= 7 && jumpx >= 0 && jumpy <= 7 && jumpy >= 0)
                        {
                            symbol = Board[jumpx, jumpy];

                            if (symbol == '-')
                            {
                                validDash = false;
                            }
                        }

                        if (validP == false && validDash == false)
                        {
                            two = true;
                        }
                        else
                        {
                            two = false;
                        }
                        i++;
                    }

                    if (i == 2)
                    {
                        bool validP = true;
                        bool validDash = true;

                        int validx = (sub(CoorX, 1));
                        int validy = (sub(CoorY, 1));
                        if (validx <= 7 && validx >= 0 && validy <= 7 && validy >= 0)
                        {
                            sign = Board[validx, validy];

                            if (sign == 'P')
                            {
                                validP = false;
                            }
                        }

                        int jumpx = (sub(CoorX, 1));
                        int jumpy = (sub(CoorY, 1));
                        if (jumpx <= 7 && jumpx >= 0 && jumpy <= 7 && jumpy >= 0)
                        {
                            symbol = Board[jumpx, jumpy];

                            if (symbol == '-')
                            {
                                validDash = false;
                            }
                        }

                        if (validP == false && validDash == false)
                        {
                            three = true;
                        }
                        else
                        {
                            three = false;
                        }
                        i++;
                    }

                    if (i == 3)
                    {
                        bool validP = true;
                        bool validDash = true;

                        int validx = (sub(CoorX, 1));
                        int validy = (plus(CoorY, 1));
                        if (validx <= 7 && validx >= 0 && validy <= 7 && validy >= 0)
                        {
                            sign = Board[validx, validy];

                            if (sign == 'P')
                            {
                                validP = false;
                            }
                        }
                        int jumpx = (sub(CoorX, 1));
                        int jumpy = (plus(CoorY, 1));
                        if (jumpx <= 7 && jumpx >= 0 && jumpy <= 7 && jumpy >= 0)
                        {
                            symbol = Board[jumpx, jumpy];

                            if (symbol == '-')
                            {
                                validDash = false;
                            }
                        }
                        if (validP == false && validDash == false)
                        {
                            four = true;
                        }
                        else
                        {
                            four = false;
                        }
                        i++;
                    }
                    if (one == true || two == true || three == true || four == true)
                    {
                        Ai[maincounter].value = 1;
                    }
                    else
                    {
                        Ai[maincounter].value = 3;
                    }
                }

                maincounter++;
            }

            // Checkes to see what the highest value is and pulls them out and puts them into a different list
            int high = 0;
            count = 0;
            foreach (var item in Ai)
            {
                int v = Ai[count].value;

                if (v > high)
                {
                    high = v;
                }
                count++;
            }

            count = 0;
            foreach (var item in Ai)
            {
                int fromX = Ai[count].moveFromX;
                int fromY = Ai[count].moveFromY;
                CoorX = Ai[count].moveToX;
                CoorY = Ai[count].moveToY;
                bool king = Ai[count].king;
                bool jump = Ai[count].jump;
                int v = Ai[count].value;

                if (v == high)
                {
                    AiTwoPoint.Add(new AIMove(fromX, fromY, CoorX, CoorY, jump, king, 0));
                }
                count++;
            }

            // Chooses a random move out of those withthe highest values and executes it

            count = AiTwoPoint.Count();
            count--;


            if (count >= 0)
            {
                Random rnd = new Random();
                int move = 0;
                if (count > 1)
                {
                    move = rnd.Next(0, count);
                }
                int x = AiTwoPoint[move].moveFromX;
                int y = AiTwoPoint[move].moveFromY;
                int xx = AiTwoPoint[move].moveToX;
                int yy = AiTwoPoint[move].moveToY;
                bool king = AiTwoPoint[move].king;
                count = 0;
                foreach (var item in P2)
                {
                    int xxx = P2[count].x;
                    int yyy = P2[count].y;

                    if (xxx == x && yyy == y)
                    {
                        placeNumber = count;
                        break;
                    }
                    count++;

                }

                if (x - xx == -2 && y - yy == 2)
                {
                    if (king == true)
                    {
                        P2.RemoveAt(placeNumber);
                        P2.Add(new PlayingPiece(xx, yy, true));
                    }
                    else
                    {
                        P2.RemoveAt(placeNumber);
                        P2.Add(new PlayingPiece(xx, yy, false));
                    }

                    count = 0;
                    foreach (var entity in P1)
                    {
                        x = P1[count].x;
                        y = P1[count].y;
                        if (x == xx - 1 && y == yy + 1)
                        {
                            P1.RemoveAt(count);
                            break;
                        }
                        count++;
                    }
                }
                else if (x - xx == 2 && y - yy == 2)
                {
                    if (king == true)
                    {
                        P2.RemoveAt(placeNumber);
                        P2.Add(new PlayingPiece(xx, yy, true));
                    }
                    else
                    {
                        P2.RemoveAt(placeNumber);
                        P2.Add(new PlayingPiece(xx, yy, false));
                    }

                    count = 0;
                    foreach (var entity in P1)
                    {
                        x = P1[count].x;
                        y = P1[count].y;
                        if (x == xx + 1 && y == yy + 1)
                        {
                            P1.RemoveAt(count);
                            break;
                        }
                        count++;
                    }
                }
                else if (x - xx == -2 && y - yy == -2)
                {
                    if (king == true)
                    {
                        P2.RemoveAt(placeNumber);
                        P2.Add(new PlayingPiece(xx, yy, true));
                    }
                    else
                    {
                        P2.RemoveAt(placeNumber);
                        P2.Add(new PlayingPiece(xx, yy, false));
                    }

                    count = 0;
                    foreach (var entity in P1)
                    {
                        x = P1[count].x;
                        y = P1[count].y;
                        if (x == xx - 1 && y == yy - 1)
                        {
                            P1.RemoveAt(count);
                            break;
                        }
                        count++;
                    }
                }
                else if (x - xx == 2 && y - yy == -2)
                {
                    if (king == true)
                    {
                        P2.RemoveAt(placeNumber);
                        P2.Add(new PlayingPiece(xx, yy, true));
                    }
                    else
                    {
                        P2.RemoveAt(placeNumber);
                        P2.Add(new PlayingPiece(xx, yy, false));
                    }

                    count = 0;
                    foreach (var entity in P1)
                    {
                        x = P1[count].x;
                        y = P1[count].y;
                        if (x == xx + 1 && y == yy - 1)
                        {
                            P1.RemoveAt(count);
                            break;
                        }
                        count++;

                    }
                }

                else
                {
                    x = Ai[move].moveFromX;
                    y = Ai[move].moveFromY;
                    int xxx = Ai[move].moveToX;
                    int yyy = Ai[move].moveToY;
                    king = Ai[move].king;
                    count = 0;
                    foreach (var item in P2)
                    {
                        xx = P2[count].x;
                        yy = P2[count].y;
                        if (x == xx && y == yy)
                        {
                            placeNumber = count;
                            break;
                        }
                        count++;
                    }
                    P2.RemoveAt(placeNumber);
                    P2.Add(new PlayingPiece(xxx, yyy, king));
                }
            }


            yourMove = true;
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            valid.Clear();
            yourMove = true;
            boardSetup();
            Refresh();
        }

        private void Rules_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can only move your pieces," +
                "\nThe aim of the game is to eliminate all the enemy pieces" +
                "\nYou can take an enemy pieces by jumping over them " +
                "\nThe blue markers will show you the valid moves of the selected piece");
        }
    }
}