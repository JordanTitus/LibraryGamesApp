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
    /// Interaction logic for MainMenuControl.xaml
    /// </summary>
    public partial class MainMenuControl : UserControl
    {
        private OptionsControl options = new OptionsControl();
        ///-----------------------------------------------------------------------------------------

        public MainMenuControl()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;

            if(mainWindow != null)
            {
                mainWindow.contentControl.Content = options;
                mainWindow.ChangeSelectedItem(1);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
