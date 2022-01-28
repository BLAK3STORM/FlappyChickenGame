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
using System.Threading;

namespace FlappyBirdGame
{
    public partial class Form1 : Form
    {

        // Instantiation
        int pipeSpeed = 8; // Initial game speed on x axis
        int gravity = 10; // Speed of the bird on y axis
        int score = 0; // Initial score
        Random spawn = new Random(); // For pipe spawning
        SoundPlayer start;
        SoundPlayer wind;
        SoundPlayer chicken;

        public Form1()
        {
            InitializeComponent();

            // Making the objects compitible for the background
            bird.Parent = background;
            bird.BackColor = Color.Transparent;
            pipeBottom.Parent = background;
            pipeBottom.BackColor = Color.Transparent;
            pipeTop.Parent = background;
            pipeTop.BackColor = Color.Transparent;
            scoreText.Parent = background;
            scoreText.BackColor = Color.Transparent;
            welcomeLabel.Parent = background;
            welcomeLabel.BackColor = Color.Transparent;
            controlLabel.Parent = background;
            controlLabel.BackColor = Color.Transparent;

            // Initial Location
            bird.SetBounds(21, 253, 59, 64);
            pipeTop.SetBounds(670, -82, 84, 288);
            pipeBottom.SetBounds(439, 419, 87, 300);

            // Initial Visiblity
            bird.Visible = false;
            pipeBottom.Visible = false;
            pipeTop.Visible = false;
            scoreText.Visible = false;

            // Sound
            start = new SoundPlayer("Resource/startbuttonclick.wav");
            wind = new SoundPlayer("Resource/wind.wav");
            chicken = new SoundPlayer("Resource/chicken.wav");
        }

        // Timer is always active from the start
        private void gameTimerEvent(object sender, EventArgs e)
        {

            speedTimer.Start(); // Speed increases in every 10 seconds

            bird.Top += gravity; // Increases the y axis value
            pipeTop.Left -= pipeSpeed; // Decreases the x axis value
            pipeBottom.Left -= pipeSpeed; // Decreases the x axis value

            scoreText.Text = "Score: "+score; // Updates score label

            // Bottom pipe spawning
            if(pipeBottom.Left < -50)
            {
                pipeBottom.Left = spawn.Next(685,960);
                pipeBottom.Top = spawn.Next(350, 500);
                score+=5; // Score increases upon passing a pipe
            }

            // Top pipe spawning
            if(pipeTop.Left < -50)
            {
                pipeTop.Left = spawn.Next(682,755);
                pipeTop.Top = spawn.Next(-250,-56);
                score+=5; // Score increases upon passing a pipe
            }


            // Collision Conditions
            if(bird.Bounds.IntersectsWith(pipeBottom.Bounds) ||
                bird.Bounds.IntersectsWith(pipeTop.Bounds) ||
                bird.Bounds.IntersectsWith(ground.Bounds) ||
                bird.Top == 0)
            {
                gameOver();
            }

        }

        // When 'space' key is Down(pressed) bird goes up as the y axis value is negative
        private void gameKeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                gravity = -10;

                // Stops the 'Ding' sound upon each keypress
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // When 'space' key is Up(released) bird goes down as the y axis value is positive
        private void gameKeyIsUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                gravity = 10;

                // Stops the 'Ding' sound upon each keypress
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // Game Over Method
        private void gameOver()
        {
            gameTimer.Stop();
            speedTimer.Stop();
            chicken.Play(); // Plays sound upon game over
            MessageBox.Show("Your score: " + score, "GAME OVER!");
            reset();
        }

        // Speed Update
        private void speedUp(object sender, EventArgs e)
        {
            pipeSpeed += 2;
        }

        // Start Button response
        private void startGame(object sender, EventArgs e)
        {
            gameTimer.Start();
            speedTimer.Start();
            bird.Visible = true;
            pipeTop.Visible = true;
            pipeBottom.Visible = true;
            scoreText.Visible = true;
            startButton.Enabled = false;
            startButton.Visible = false;
            welcomeLabel.Visible = false;
            controlLabel.Visible = false;

            start.Play(); // Plays sound upon start button click
            Thread.Sleep(500); // Takes an interval after pressing the button to start
            wind.PlayLooping(); // Plays sound during the game
        }

        // Resetting Everything
        private void reset()
        {
            score = 0;
            pipeSpeed = 8;
            gravity = 10;
            bird.SetBounds(21, 253, 59, 64);
            pipeTop.SetBounds(670, -82, 84, 288);
            pipeBottom.SetBounds(439, 419, 87, 300);
            bird.Visible = false;
            pipeBottom.Visible = false;
            pipeTop.Visible= false;
            scoreText.Visible = false;
            scoreText.Text = "Score: 0";
            startButton.Enabled= true;
            startButton.Visible= true;
            welcomeLabel.Visible= true;
            controlLabel.Visible= true;
        }
    }
}
