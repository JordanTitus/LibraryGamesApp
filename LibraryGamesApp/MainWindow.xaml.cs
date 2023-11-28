using LibraryGamesApp.UserControls_Folder;
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

namespace LibraryGamesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            //user control with the main menu options (start, options and exit)
            //start will change the contentControl to another user control
            MainMenuControl controllerDefault = new MainMenuControl();
            contentControl.Content = controllerDefault;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void tabControlMian_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem selectedItem = (TabItem)tabControlMian.SelectedItem;

            if (selectedItem != null)
            {
                string tabHeader = selectedItem.Header.ToString();

                switch (tabHeader)
                {
                    case "Home":
                        contentControl.Content = new MainMenuControl();
                        break;
                    case "Exit":
                        Application.Current.Shutdown();
                        break;
                }


            }
        }

        public void ChangeSelectedItem(int tabIndex)
        {
            tabControlMian.SelectedIndex = tabIndex;
        }
        
    }
}
