using LibraryGamesApp.Utilities_Folder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static LibraryGamesApp.Utilities_Folder.RedBlackTree;

namespace LibraryGamesApp.UserControls_Folder
{
    /// <summary>
    /// Interaction logic for FindingCallsControl.xaml
    /// </summary>
    public partial class FindingCallsControl : UserControl
    {
        RedBlackTree redBlackTree = new RedBlackTree();
        private DispatcherTimer timer;
        TimeSpan elapesedTime = TimeSpan.Zero;
        Random random = new Random();
        public static List<string>[] levels;
        private static List<string> correctAnswersList;
        private static SortedDictionary<int, string> sortedDict = new SortedDictionary<int, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FindingCallsControl"/> class.
        /// </summary>
        public FindingCallsControl()
        {
            InitializeComponent();
            InitiliazeTimer();
        }

        private void InitiliazeTimer()
        {
            timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            Panel.SetZIndex(timerLabel, 999);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            levels = saveToTree(levels);
            populateButtons1();
            timer.Start();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            elapesedTime = elapesedTime.Add(TimeSpan.FromSeconds(1));
            UpdateTimerLabel();
        }

        private void UpdateTimerLabel()
        {
            timerLabel.Content = $"Time:{elapesedTime:mm\\:ss}";
        }

        #region populate_buttons
        /// <summary>
        /// Populate the buttons for the first level.
        /// </summary>
        private void populateButtons1()
        {
            randomQuestion();
            txtDescription.Text = correctAnswersList[3];
            sortedDict.Clear();
            if (levels != null)
            {
                List<string> firstLevel = levels[0];
                sortedDict.Add(int.Parse(correctAnswersList[2].Substring(0, 3)), correctAnswersList[2]);
                while (sortedDict.Count < 4)
                {
                    int randomIndex = random.Next(firstLevel.Count);
                    string valueToAdd = firstLevel[randomIndex];

                    int extractInt = int.Parse(valueToAdd.Substring(0, 3));

                    Node item = redBlackTree.Find(extractInt);

                    if (!sortedDict.ContainsKey(extractInt))
                    {
                        sortedDict.Add(extractInt, (item.data + " " + item.desc));
                    }
                }

                Button[] buttons = { btn1, btn2, btn3, btn4 };

                int i = 0;
                foreach (var pair in sortedDict)
                {
                    buttons[i].Content = pair.Value;
                    buttons[i].Click -= btn3_Click;
                    buttons[i].Click += btn1_Click;
                    i++;
                }
            }
        }

        /// <summary>
        /// Populate buttons for the second level.
        /// </summary>
        private void populateButtons2()
        {
            sortedDict.Clear();

            if (levels != null)
            {
                List<string> secondLevel = levels[1];

                sortedDict.Add(int.Parse(correctAnswersList[1].Substring(0, 3)), correctAnswersList[1]);
                while (sortedDict.Count < 4)
                {
                    int randomIndex = random.Next(secondLevel.Count);
                    string valueToAdd = secondLevel[randomIndex];

                    int extractInt = int.Parse(valueToAdd.Substring(0, 3));

                    Node item = redBlackTree.Find(extractInt);

                    if (!sortedDict.ContainsKey(extractInt))
                    {
                        sortedDict.Add(extractInt, (item.data + " " + item.desc));
                    }
                }

                Button[] buttons = { btn1, btn2, btn3, btn4 };

                int i = 0;
                foreach (var pair in sortedDict)
                {
                    buttons[i].Content = pair.Value;

                    buttons[i].Click -= btn1_Click;
                    buttons[i].Click += btn2_Click;
                    i++;
                }
            }
        }

        /// <summary>
        /// Populate the buttons for the third level.
        /// </summary>
        private void populateButtons3()
        {
            sortedDict.Clear();

            if (levels != null)
            {
                List<string> thirdlevel = levels[2];

                sortedDict.Add(int.Parse(correctAnswersList[0].Substring(0, 3)), correctAnswersList[0]);
                while (sortedDict.Count < 4)
                {
                    int randomIndex = random.Next(thirdlevel.Count);
                    string valueToAdd = thirdlevel[randomIndex];

                    int extractInt = int.Parse(valueToAdd.Substring(0, 3));

                    Node item = redBlackTree.Find(extractInt);

                    if (!sortedDict.ContainsKey(extractInt))
                    {
                        sortedDict.Add(extractInt, (item.data + " " + item.desc));
                    }
                }

                Button[] buttons = { btn1, btn2, btn3, btn4 };

                int i = 0;
                foreach (var pair in sortedDict)
                {
                    buttons[i].Content = pair.Key;

                    buttons[i].Click -= btn2_Click;
                    buttons[i].Click += (sender, e) => btn3_Click(sender, e);
                    i++;
                }
            }
        }
        #endregion

        #region game_setup
        /// <summary>
        /// Randomly selects a question from the third level.
        /// </summary>
        private void randomQuestion()
        {
            if (levels != null)
            {
                List<string> thirdLevel = levels[2];

                int randomNumber;
                int parsedNumber;
                do
                {
                    randomNumber = random.Next(1, 990);
                    parsedNumber = ExtractNumberFromList(randomNumber, thirdLevel);

                } while (parsedNumber == -1);

                Node item = redBlackTree.Find(parsedNumber);

                correctAnswersList = new List<string>
                {
                    (item.data + " " + item.desc),

                    FindParents(item.data.ToString(), 2),

                    FindParents(item.data.ToString(), 1),

                    item.desc,

                    item.data.ToString()
                };
            }
        }

        /// <summary>
        /// Finds the parents of the third level entry.
        /// </summary>
        private string FindParents(string thirdLevel, int level)
        {
            string extractedString;
            if (level == 2)
            {
                extractedString = thirdLevel.Substring(0, 2);
                Node item = redBlackTree.Find((int.Parse(extractedString) * 10));
                return (item.data + " " + item.desc);
            }
            else
            {
                extractedString = thirdLevel.Substring(0, 1);
                Node item = redBlackTree.Find((int.Parse(extractedString) * 100));
                return (item.data + " " + item.desc);
            }
        }

        /// <summary>
        /// Extracts a number from the list of strings.
        /// </summary>
        static int ExtractNumberFromList(int number, List<string> list)
        {
            foreach (var item in list)
            {
                string[] parts = item.Split(new[] { "." }, StringSplitOptions.None);

                if (int.TryParse(parts[0], out int parsed))
                {
                    if (parsed == number)
                    {
                        return parsed;
                    }
                }
            }
            return -1;
        }
        #endregion 

        #region Button_click
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                string content = clickedButton.Content.ToString();

                if (correctAnswersList.Contains(content))
                {
                    populateButtons2();
                }
                else
                {
 
                }
            }
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                string content = clickedButton.Content.ToString();

                if (correctAnswersList.Contains(content))
                {;
                    populateButtons3();
                }
                else
                {

                }
            }
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                string content = clickedButton.Content.ToString();

                if (correctAnswersList.Contains(content))
                {
                    MessageBox.Show("Congrats, you got them all ☺!");
                    RestartGame();
                }
                else
                {
                    // Handle incorrect click if needed
                }
            }
        }
        #endregion

        #region File_Loading
        /// <summary>
        /// Reads the content of a file and populates the Red-Black Tree with its values.
        /// </summary>
        public List<string>[] saveToTree(List<string>[] levels)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string FileName = "Tree.txt";

            string filePath = System.IO.Path.Combine(currentDirectory, FileName);

            redBlackTree.PopulateTreeFromFile(filePath);
            redBlackTree.GetTreeValues();
            levels = redBlackTree.DisplayTree();

            return levels;
        }

        private void RestartGame()
        {
            // Detach event handlers
            btn1.Click -= btn1_Click;
            btn2.Click -= btn2_Click;
            btn3.Click -= btn3_Click;

            // Reset game state to the initial level
            elapesedTime = TimeSpan.Zero;
            UpdateTimerLabel();
            populateButtons1();
        }
        #endregion
    }
}
