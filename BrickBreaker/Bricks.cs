using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrickBreaker
{
    internal class Bricks
    {
        //Variables
        public int x, y;
        public int width, height;
        public Color color;
        public int hitPoints; // The hit points of the brick

        //constructor
        public Bricks(int _x, int _y, int _width, int _height, Color _color, int _hitPoints)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            color = _color;
            hitPoints = _hitPoints;
        }
        //returns the rectangle of the brick
        public Rectangle GetRectangle()
        {
            return new Rectangle(x, y, width, height);
        }

        //Check for collision with the ball
        public bool BrickCollision(Ball ball)
        {
            Rectangle ballRect = new Rectangle(ball.x, ball.y, ball.size, ball.size);

            if (GetRectangle().IntersectsWith(ballRect))
            {      
                return true;
            }
            return false;
        }
    }
}
