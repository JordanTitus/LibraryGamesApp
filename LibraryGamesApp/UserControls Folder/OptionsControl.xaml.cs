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

namespace LibraryGamesApp.UserControls_Folder
{
    /// <summary>
    /// Interaction logic for OptionsControl.xaml
    /// </summary>
    public partial class OptionsControl : UserControl
    {
        private BookReplacingControl controlGame = new BookReplacingControl();
        private IdentifyAreasControl identifyAreas = new IdentifyAreasControl();
        private FindingCallsControl findCalls = new FindingCallsControl();

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsControl"/> class.
        /// </summary>
        public OptionsControl()
        {
            InitializeComponent();
        }

        private void btnGame1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.contentControl.Content = controlGame;
            }
        }

        private void btnExplain1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Arrange the Books in ascending order, according to their call numbers." +
                "\n10 books will be generated and placed in a random order." +
                "\nSee how fast you complete the sorting game.", "Rules: Replacing Books");
        }

        private void btnGame2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.contentControl.Content = identifyAreas;
            }
        }

        private void btnExplain2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Match the corresponding call numbers with their descriptions, " +
                "by clicking on a question from the right and then clicking on the matching answer on the left." +
                "\nYou have 3 lives. \nA life is deducted each time you answer incorrectly." +
                "\nWhen all four questions are answered (the buttons have been greyed out) " +
                "select continue at the bottom of the screen to continue playing." +
                "\nSee how many rounds you can beat before dying." +
                "\nRounds beaten and remaining lives are displayed in the bottom left.", "Rules: Identifying Areas");
        }

        private void btnGame3_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.contentControl.Content = findCalls;
            }
        }
    }
}
