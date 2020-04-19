using System;
using System.Windows;
using System.Windows.Controls;
using FlightSimulatorApp.ViewModel;

namespace FlightSimulatorApp.View.Controls
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        private readonly MainMenuViewModel vm;
        public event EventHandler CloseApp;
        public event EventHandler ConnectApp;
        public event EventHandler OpenTutorial;

        /**
         * Constructor
         **/
        public MainMenu()
        {
            InitializeComponent();
            DataContext = (Application.Current as App).MainMenuViewModel;
            this.vm = (Application.Current as App).MainMenuViewModel;
        }



        /**
         * Connect click event
         **/
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            bool flag = this.vm.Connect();

            if (ConnectApp != null && flag)
            {
                ConnectApp(this, new EventArgs());
            }
        }

        /**
         * Controls explanation click event
         **/
        private void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            OpenTutorial?.Invoke(this, new EventArgs());
        }

        /**
         * Quit click event
         **/
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            CloseApp?.Invoke(this, new EventArgs());
        }
    }
}