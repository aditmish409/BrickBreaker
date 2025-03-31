using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    internal class Ball
    {
        //Variables
        public int x, y;
        public int size = 10;
        public int xSpeed, ySpeed;
        public bool isMoving = false;

        //Constructor
        public Ball(int _x, int _y, int _xSpeed, int _ySpeed)
        {
            x = _x;
            y = _y;
            xSpeed = _xSpeed;
            ySpeed = _ySpeed;
        }

        //Moves the ball
        public void Move()
        {
            x += xSpeed;
            y += ySpeed;

            if (x < 0 || x > GameScreen.gameScreenWidth - size)
            {
                xSpeed = -xSpeed;
            }
            if (y < 0 )
            {
                ySpeed = -ySpeed;
            }
        }
    }
}
