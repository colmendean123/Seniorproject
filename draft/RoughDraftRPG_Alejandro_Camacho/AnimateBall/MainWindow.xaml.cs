using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

/* Name: Alejandro Camacho
 * Description:
 * Q : Quit Game 
 * RPG Draft, Music, Theme, and User Control through C#
 * Animation Control Keys:
 * S : Stops Animation
 * SpaceBar: Starts Animation
 *
 * Theme Control Keys: D, G, B can change the color of theme: Default, Green, Blue.
 * The application also uses windows media player, to play The_Weekend_BlindingLights.mp3 which was my intro song.
 * Stack panel with buttons, for music control. Currently plays extra suffix sound of a gun clip being shot completely empty. 
 * 
 * Music Keys:
 * P : Play Music
 * M : Pause Music
 * Stop Button: Stops music, 
 * SFX Button: Shoots empty gun shot clip. 
 * 
 * 
 * 
          
 */



namespace RoughDraftRPG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameTimer = new DispatcherTimer();

        private double gamePlayerTopY = 0;
        private double gamePlayerLeftX = 0;
        private double gamePlayerDy = 5;
        private double gamePlayerDx = 5;

        public MainWindow()
        {
            InitializeComponent();
            GreetUser();
            ResetMenuBtn.IsEnabled = false;
            NewMenuBtn.IsEnabled = true;
        }

        private void GreetUser()
        {
            MessageBox.Show("WELCOME TO RPG GAME: \n Select Game, NEW, Player One, to start the game.\nTo restart select game, restart.\n\nRPG Controls:\nWe need a hero, could you be the one to guide us free from the dark ewu caves?\nCollect Pillars of OO\nFight any monsters you encounter in the dungeon\nUseful items may be found, but use at your own risk as they can be enchanted.\nMost of all have fun and enjoy the experience.\nTo Move Player: User Arrows, To Change themes press the character for the color");
        }

        //Game timer tick, controlls collisions

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            
            Canvas.SetTop(gamePlayer, gamePlayerTopY);
            Canvas.SetLeft(gamePlayer, gamePlayerLeftX);

        }


        //Checking for key down events, color scheme change, and pause and play functionality
        //Press S to Stop animation
        //Press SpaceBar to play animation
        //Change colors by pressing, D default, B for blue, G for green
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (gamePlayerTopY - gamePlayerDy >= 0)
                    gamePlayerTopY -= gamePlayerDy;
            }
            else if (e.Key == Key.Down)
            {
                if (gamePlayerTopY + gamePlayerDy + gamePlayer.Height <= myGameCanvas.Height)
                    gamePlayerTopY += gamePlayerDy;
            }


            else if (e.Key == Key.Left)
            {
                if (gamePlayerLeftX >= 0)
                {
                    gamePlayerLeftX -= gamePlayerDx;
                }
            }

            else if (e.Key == Key.Right)
            {
                if (gamePlayerLeftX <= myGameCanvas.Width)
                {
                    gamePlayerLeftX += gamePlayerDx;
                }
            }

            //Control keys: Q 
            //Quit game: calling GameOver method

            else if (e.Key == Key.Q)
            {
                GameOver();
            }


            //Control key: P 
            //Music control: Play Music
            else if (e.Key == Key.P)
            {
                myMediaElement.Play();
            }

            //Music Contol Key: Pause
            else if (e.Key == Key.M) {
                myMediaElement.Pause();
            }

            //Control Key: Stop Animation
            else if (e.Key == Key.S)
            {
                gameTimer.Stop();
            }

            //Control Key: Start Animation
            else if (e.Key == Key.Space)
            {
                gameTimer.Start();
            }

            //Control Key: Color Blue
            else if (e.Key == Key.B)
            {
                var bc = new BrushConverter();
                myGrid.Background = (Brush)bc.ConvertFrom("#FF93B4D4");
                myGameCanvas.Background = (Brush)bc.ConvertFrom("#FF3F85BF");
                gameMonster.Fill = (Brush)bc.ConvertFrom("#FF93B4D4");
                gameMonster.Stroke = (Brush)bc.ConvertFrom("#FF93B4D4");
                gamePlayer.Fill = (Brush)bc.ConvertFrom("#FF93B4D4");
                gamePlayer.Stroke = (Brush)bc.ConvertFrom("#FF93B4D4");
            }
            //Control Key: Color Green
            else if (e.Key == Key.G)
            {
                var bc = new BrushConverter();
                myGrid.Background = (Brush)bc.ConvertFrom("#FF79A883");
                myGameCanvas.Background = (Brush)bc.ConvertFrom("#FFBBE0B3");
                gamePlayer.Fill = (Brush)bc.ConvertFrom("#FF79A883");
                gamePlayer.Stroke = (Brush)bc.ConvertFrom("#FF79A883");
                gameMonster.Fill = (Brush)bc.ConvertFrom("#FF79A883");
                gameMonster.Stroke = (Brush)bc.ConvertFrom("#FF79A883");
            }

            //Control Key: Color Default
            else if (e.Key == Key.D)
            {
                var bc = new BrushConverter();
                myGrid.Background = Brushes.White;
                myGameCanvas.Background = (Brush)bc.ConvertFrom("#FFAEAEAE");
                gamePlayer.Fill = Brushes.White;
                gamePlayer.Stroke = Brushes.White;
                gameMonster.Fill = Brushes.White;
                gameMonster.Stroke = Brushes.White;
            }
        }


        //File Menu Quit option, shuts down application. References music song moneyTalks.mp3

        private void FileMenuQuitBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("$$$$ THANK YOU FOR PLAYING  $$$");
            Application.Current.Shutdown();
        }


        //About menu option
        private void AboutMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Developed by: Team 6: ....");
        }


        //Game Rules for animation ping pong game
        private void RulesMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("WELCOME TO RPG GAME\nStart GAME => NEW GAME\nPress 'S' to STOP ANIMATION\nPress 'Space Bar' to START ANIMATION\nTheme Change:\nPress 'G' to change the GREEN COLOR\nPress 'B' for Blue Color\nPress 'D' for Default Color");
        }


        //Reset menu option hides hides paddle and gameball, resets gameball in the middle
        private void ResetMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            myMediaElement.Stop();
           

            NewMenuBtn.IsEnabled = true;
            ResetMenuBtn.IsEnabled = false;

            gamePlayer.Visibility = Visibility.Hidden;
            gameTimer.Stop();
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            GameTimer_Tick(sender, e);
        }


        //GameOver event, displays message box to user, with the high score
        private void GameOver()
        {
            myMediaElement.Stop();
            gamePlayer.Visibility = Visibility.Hidden;
            gameTimer.Stop();
            MessageBox.Show("GAME OVER");
        }

        //Start Game method

        private void StartGame()
        {
            myMediaElement.Play();
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            gameTimer.Tick += GameTimer_Tick;
            
            gamePlayerTopY = Canvas.GetTop(gamePlayer);
            gamePlayerLeftX = Canvas.GetLeft(gamePlayer);
            gamePlayer.Visibility = Visibility.Visible;
          
            gameTimer.IsEnabled = true;

           
        }

        //Player one game option, initiaties start game method
        private void PlayerOneMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            myMediaElement.Source = new System.Uri("The Weeknd.mp3", UriKind.Relative);
            myMediaElement.Play();
            StartGame();
            ResetMenuBtn.IsEnabled = true;
            NewMenuBtn.IsEnabled = false;
        }

        //Music Control Play button
        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            myMediaElement.Play();
        }

        //Music control: Pause Music
        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            myMediaElement.Pause();
        }

        //Music Control: Stop Music
        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            myMediaElement.Stop();
        }


        //Sound Control: SFX shooting a gun empty clip
        private void sfxBtn_Click(object sender, RoutedEventArgs e)
        {
            myMediaElement.Stop();
            myMediaElement.Source = new System.Uri("shoot.wav", UriKind.Relative);
            myMediaElement.Play();
        }
    
    }
}




