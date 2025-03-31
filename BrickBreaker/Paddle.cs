using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    internal class Paddle
    {
        //Variables
        public int x, y;
        public int paddleWidth = 80, paddleHeight = 15;
        public int paddleSpeed = 8;

        //Initializes the paddle
        public Paddle() 
        {
            x = GameScreen.gameScreenWidth / 2 - paddleWidth / 2;
            y = GameScreen.gameScreenHeight - 30;
        }
        //Moves the paddle
        public void Move(string direction)
        {
            if (direction == "right" && x < GameScreen.gameScreenWidth - paddleWidth)
            {
                x += paddleSpeed;
            }
            if (direction == "left" && x > 0)
            {
                x -= paddleSpeed;
            }
        }
        //Checks for collision with the ball and returns true if there is a collision
        public bool Collision(Ball ball)
        {
            Rectangle paddleRect = new Rectangle(x, y, paddleWidth, paddleHeight);
            Rectangle ballRect = new Rectangle(ball.x, ball.y, ball.size, ball.size);

            if (paddleRect.IntersectsWith(ballRect))
            {
                ball.ySpeed = -ball.ySpeed;
                ball.y = y - ball.size;
                return true;
            }
            return false;
        }
    }
}
