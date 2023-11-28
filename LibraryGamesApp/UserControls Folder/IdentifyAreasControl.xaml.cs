using LibraryGamesApp.Utilities_Folder;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LibraryGamesApp.UserControls_Folder
{
    /// <summary>
    /// Interaction logic for IdentifyAreasControl.xaml
    /// </summary>
    public partial class IdentifyAreasControl : UserControl
    {
        private static int lifeCounter = 3;
        private static int correctAnswerCounter = 0;
        private static int roundCounter = 1;
        private static int leftButtonsCount = 4;
        private static int rightButtonsCount = 7;
        private static string btnContentR;
        private static string btnContentL;
        private Button leftButton;
        private Button rightButton;
        private CallNumberGenerator callNumber = new CallNumberGenerator();
        private Random random = new Random();
        private Dictionary<int, string> dictionaryTemp = new Dictionary<int, string>();
        private HashSet<int> usedButtons = new HashSet<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifyAreasControl"/> class.
        /// </summary>
        public IdentifyAreasControl()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void btnRight1_Click(object sender, RoutedEventArgs e)
        {
            rightButton = sender as Button;
            btnContentR = rightButton.Content.ToString();

            if (btnContentL != null)
            {
                if (callNumber.CompareDescriptions(btnContentL, btnContentR, roundCounter))
                {
                    correctAnswerCounter++;
                    rightButton.IsEnabled = false;
                    leftButton.IsEnabled = false;

                    if (correctAnswerCounter == 4)
                    {
                        btnSubmit.IsEnabled = true;
                    }

                    resetStrings();
                }
                else
                {
                    lifeCounter--;
                    lifeTracker();
                    resetStrings();
                }
            }
        }

        private void btnleft1_Click(object sender, RoutedEventArgs e)
        {
            leftButton = sender as Button;
            btnContentL = leftButton.Content.ToString();

            if (btnContentR != null)
            {
                if (callNumber.CompareDescriptions(btnContentL, btnContentR, roundCounter))
                {
                    correctAnswerCounter++;
                    rightButton.IsEnabled = false;
                    leftButton.IsEnabled = false;

                    if (correctAnswerCounter == 4)
                    {
                        btnSubmit.IsEnabled = true;
                    }

                    resetStrings();
                }
                else
                {
                    lifeCounter--;
                    lifeTracker();
                    resetStrings();
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            changeButtonsTextNormal();
            txtRounds.Text = "Rounds: " + roundCounter.ToString();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            dictionaryTemp.Clear();
            usedButtons.Clear();
            resetStrings();
            EnableButtonsInGrid(leftGrid, true);
            EnableButtonsInGrid(rightGrid, true);
            btnSubmit.IsEnabled = false;

            roundCounter++;
            txtRounds.Text = "Rounds: " + roundCounter.ToString();
            lifeCounter = 3;

            BitmapImage fullBar = new BitmapImage(new Uri("/Resources/fullBar.png", UriKind.RelativeOrAbsolute));
            myImage.Source = fullBar;

            if (roundCounter % 2 == 0)
            {
                changeButtonsTextReverse();
            }
            else
            {
                changeButtonsTextNormal();
            }
        }

        #endregion

        #region Functions

        private void changeButtonsTextNormal()
        {
            for (int i = 1; i <= leftButtonsCount; i++)
            {
                string btnLeft = ("btnleft" + i);
                Button buttonLeft = FindName(btnLeft) as Button;

                string callNumb = callNumber.GenCallNumbers();

                if (buttonLeft != null)
                {
                    dictionaryTemp.Add(i, callNumb);
                    buttonLeft.Content = callNumb;
                }
            }

            for (int i = 1; i <= rightButtonsCount; i++)
            {
                int btnNumber;

                do
                {
                    btnNumber = random.Next(1, 8);
                }
                while (usedButtons.Contains(btnNumber));

                usedButtons.Add(btnNumber);

                string btnRight = ("btnright" + btnNumber);
                Button buttonRight = FindName(btnRight) as Button;

                string callNumb;

                if (i < 5)
                {
                    if (buttonRight != null)
                    {
                        callNumb = dictionaryTemp[i];
                        buttonRight.Content = callNumber.getmatchingDescription(callNumb, i);
                    }
                }
                else
                {
                    buttonRight.Content = callNumber.getmatchingDescription("0", i);
                }
            }
        }

        private void changeButtonsTextReverse()
        {
            for (int i = 1; i <= rightButtonsCount; i++)
            {
                string btnRight = ("btnright" + i);
                Button buttonRight = FindName(btnRight) as Button;

                string callNumb = callNumber.GenCallNumbers();

                if (buttonRight != null)
                {
                    dictionaryTemp.Add(i, callNumb);
                    buttonRight.Content = callNumb;
                }
            }

            for (int i = 1; i <= leftButtonsCount; i++)
            {
                string callNumb;

                int btnNumber;

                do
                {
                    btnNumber = random.Next(1, 5);
                }
                while (usedButtons.Contains(btnNumber));

                usedButtons.Add(btnNumber);

                string btnLeft = ("btnleft" + btnNumber);
                Button buttonLeft = FindName(btnLeft) as Button;

                if (i <= rightButtonsCount)
                {
                    if (buttonLeft != null)
                    {
                        callNumb = dictionaryTemp[i];
                        buttonLeft.Content = callNumber.getmatchingDescription(callNumb, i);
                    }
                }
                else
                {
                    buttonLeft.Content = callNumber.getmatchingDescription("0", i);
                }
            }
        }

        private void EnableButtonsInGrid(Grid grid, bool enable)
        {
            foreach (UIElement element in grid.Children)
            {
                if (element is Button button)
                {
                    button.IsEnabled = enable;
                }
            }
        }

        private void lifeTracker()
        {
            switch (lifeCounter)
            {
                case 0:
                    BitmapImage emptyBar = new BitmapImage(new Uri("/Resources/emptyBar.png", UriKind.RelativeOrAbsolute));
                    myImage.Source = emptyBar;

                    MessageBox.Show("Reloading Columns...", "<---You Failed--->");
                    retryAfterDeath();
                    break;
                case 1:
                    BitmapImage oneBar = new BitmapImage(new Uri("/Resources/1Bar.png", UriKind.RelativeOrAbsolute));
                    myImage.Source = oneBar;
                    break;
                case 2:
                    BitmapImage twoBar = new BitmapImage(new Uri("/Resources/2Bar.png", UriKind.RelativeOrAbsolute));
                    myImage.Source = twoBar;
                    break;
                default:
                    BitmapImage fullBar = new BitmapImage(new Uri("/Resources/fullBar.png", UriKind.RelativeOrAbsolute));
                    myImage.Source = fullBar;
                    break;
            }
        }

        private void retryAfterDeath()
        {
            dictionaryTemp.Clear();
            usedButtons.Clear();
            EnableButtonsInGrid(leftGrid, true);
            EnableButtonsInGrid(rightGrid, true);
            btnSubmit.IsEnabled = false;

            roundCounter = 1;
            lifeCounter = 3;

            BitmapImage fullBar = new BitmapImage(new Uri("/Resources/fullBar.png", UriKind.RelativeOrAbsolute));
            myImage.Source = fullBar;

            changeButtonsTextNormal();
            txtRounds.Text = "Rounds: " + roundCounter.ToString();
        }

        private void resetStrings()
        {
            btnContentL = null;
            btnContentR = null;

            if (correctAnswerCounter == 4)
            {
                correctAnswerCounter = 0;
            }
        }

        #endregion
    }
}
