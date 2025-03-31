using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrickBreaker
{
    public partial class MenuScreen : UserControl
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void easyButton_Click(object sender, EventArgs e)
        {
            Form1.ChangeScreen(this, new GameScreen());
            GameScreen.amountOfBalls = 5;
        }

        private void mediumButton_Click(object sender, EventArgs e)
        {
            Form1.ChangeScreen(this, new GameScreen());
            GameScreen.amountOfBalls = 3;
        }

        private void hardButton_Click(object sender, EventArgs e)
        {
            Form1.ChangeScreen(this, new GameScreen());
            GameScreen.amountOfBalls = 1;

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
