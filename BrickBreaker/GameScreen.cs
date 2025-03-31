/* 
    Ayush Patel
    2025-03-31
    Brick Breaker
    Mr. Theodoropoulos
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace BrickBreaker
{
    public partial class GameScreen : UserControl
    {
        public static int gameScreenWidth, gameScreenHeight;
        bool leftArrowDown, rightArrowDown, spaceBarDown, aDown, dDown, xDown, escDown;

        public static int amountOfBalls;
        public static int score;
        public static int currentLevel = 1;
        public static int maxRows;

        Font hpFont = new Font("Consolas", 12);

        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Pen whitePen = new Pen(Color.White);

        //Game Objects
        Ball gameBall;
        Paddle gamePaddle;

        List<Bricks> bricksList = new List<Bricks>();

        SoundPlayer collisionSound;

        public GameScreen()
        {
            InitializeComponent();
            InitializeGame();

        }

        public void InitializeGame()
        {
            collisionSound = new SoundPlayer(Properties.Resources.collisionSound);

            gameScreenHeight = this.Height;
            gameScreenWidth = this.Width;

            gameBall = new Ball(this.Width / 2, this.Height - 40, 10, -8);
            gamePaddle = new Paddle();
            MakeBricks();
        }

        public void MakeBricks()
        {
            int brickWidth = 80;
            int brickHeight = 25;
            int gap = 5;
            int initialX = 5;
            int initialY = 50;
            int columns = 6 + currentLevel;

            if (columns == 7)
            {
                columns = 7;
            }
            int rows = 4;

            bricksList.Clear(); //Clear the list of bricks

            for (int row = 0; row < rows; row++) //For every row
            {
                for (int column = 0; column < columns; column++) //For every column
                {
                    Color brickColor = Color.Blue;

                    int hitPoints;

                    int x = initialX + column * (brickWidth + gap); //Set the x position of the brick
                    int y = initialY + row * (brickHeight + gap); //Set the y position of the brick

                    switch (row)
                    {
                        case 0:
                            brickColor = Color.Blue;
                            hitPoints = GameScreen.currentLevel + 3;
                            break;
                        case 1:
                            brickColor = Color.Red;
                            hitPoints = GameScreen.currentLevel + 2;
                            break;
                        case 2:
                            brickColor = Color.Yellow;
                            hitPoints = GameScreen.currentLevel + 1;
                            break;
                        case 3:
                            brickColor = Color.Green;
                            hitPoints = GameScreen.currentLevel;
                            break;
                        default:
                            brickColor = Color.White;
                            hitPoints = 1;
                            break;
                    }
                    bricksList.Add(new Bricks(x, y, brickWidth, brickHeight, brickColor, hitPoints)); //Add the brick to the list
                }
            }
        }

        //Part of Design, Draws 3 ellipses(balls) if shaded in, the player has that many balls remaining if just empty the player has no balls
        private void DrawBallLives(Graphics g)
        {
            int ballSize = 15;       //Size of ball
            int startX = 10;         //Starting x position of ball
            int startY = 10;         //Starting y position of ball
            int spacing = 20;        //Space between balls

            for (int i = 0; i < amountOfBalls; i++)
            {
                g.DrawEllipse(whitePen, startX + (i * spacing), startY, ballSize, ballSize);        //Draws the outline of the ball
                g.FillEllipse(whiteBrush, startX + (i * spacing), startY, ballSize, ballSize);      //Fills the ball with white color
            }
        }

        #region KeyPresses
        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Space:
                    spaceBarDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.X:
                    xDown = false;
                    break;
                case Keys.Escape:
                    escDown = false;
                    break;
            }
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Space:
                    spaceBarDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.X:
                    xDown = true;
                    break;
                case Keys.Escape:
                    escDown = true;
                    break;
            }
        }
        #endregion

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //Check if the ball is moving if not move it when space bar is clicked
            if (spaceBarDown == true || xDown == true && !gameBall.isMoving)
            {
                gameBall.isMoving = true;
            }
            if (gameBall.isMoving == false)
            {
                gameBall.x = gamePaddle.x + (gamePaddle.paddleWidth / 2) - (gameBall.size / 2); // Center the ball on the paddle as the paddle moves
                gameBall.y = gamePaddle.y - gameBall.size; //Setting the ball on top of the paddle
            }

            //Check if the ball is moving inside the screen
            if (gameBall.isMoving == true)
            {
                gameBall.Move();
                if (gameBall.y + gameBall.size >= GameScreen.gameScreenHeight)
                {
                    gameBall.isMoving = false;
                    amountOfBalls--;
                    gameBall.x = gamePaddle.x + gamePaddle.paddleWidth / 2 - gameBall.size / 2;
                    gameBall.y = gamePaddle.y - gameBall.size;
                }
                if (amountOfBalls == 0)
                {
                    gameTimer.Stop();
                    Form1.ChangeScreen(this, new GameOverScreen());
                }
            }

            //Moving the paddle
            if (leftArrowDown == true || aDown == true)
            {
                gamePaddle.Move("left");
            }
            if (rightArrowDown == true || dDown == true)
            {
                gamePaddle.Move("right");
            }

            //Moving the ball according to the hitPoint on the paddle
            if (gamePaddle.Collision(gameBall))
            {
                collisionSound.Play();
                float hitPoint = (gameBall.x + gameBall.size / 2f) - gamePaddle.x; //Checks where the ball hits the paddle
                float paddlePart = gamePaddle.paddleWidth / 2f;                    //Divides the paddle into 3 parts

                //If the ball hits the left part of the paddle
                if (hitPoint < paddlePart)
                {
                    gameBall.xSpeed = -gameBall.xSpeed;                             //Reverses the xSpeed of the ball
                }

                //If the ball hits the right part of the paddle
                else if (hitPoint > paddlePart * 2)
                {
                    gameBall.xSpeed = -gameBall.xSpeed;                             //Reverses the xSpeed of the ball
                }
            }

            //Check for collision with the bricks
            foreach (Bricks brick in bricksList)
            {
                if (brick.BrickCollision(gameBall))
                {
                    collisionSound.Play();
                    gameBall.ySpeed = -gameBall.ySpeed;
                    brick.hitPoints--;
                    score += 10;

                    if (brick.hitPoints <= 0)
                    {
                        bricksList.Remove(brick); //Remove the brick 
                        break;
                    }
                }
            }
            //If all the bricks have been destroyed then make new ones
            if (bricksList.Count == 0)
            {
                MakeBricks();
            }

            //If the escape key is pressed then stop the game
            if (escDown == true)
            {
                gameTimer.Stop();
                Form1.ChangeScreen(this, new MenuScreen());
            }
            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            scoreLabel.Text = $"{score}";

            DrawBallLives(e.Graphics);  //Paints the amount of balls the player has.
            e.Graphics.FillEllipse(whiteBrush, gameBall.x, gameBall.y, gameBall.size, gameBall.size); //Paints the ball
            e.Graphics.FillRectangle(whiteBrush, gamePaddle.x, gamePaddle.y, gamePaddle.paddleWidth, gamePaddle.paddleHeight); //Paints the paddle

            //Paints the bricks
            foreach (Bricks brick in bricksList) 
            {
                SolidBrush brickBrush = new SolidBrush(brick.color);
                e.Graphics.FillRectangle(brickBrush, brick.x, brick.y, brick.width, brick.height);
                e.Graphics.DrawString(brick.hitPoints.ToString(), hpFont, whiteBrush, brick.x + 35, brick.y + 2);
            }
        }
    }
}
