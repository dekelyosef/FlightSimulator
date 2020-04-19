using System;
using System.Windows;
using FlightSimulatorApp.ViewModel;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel vm;
        public event EventHandler Back;

        /**
         * Constructor
         **/
        public MainWindow()
        {
            InitializeComponent();
            DataContext = (Application.Current as App).MainWindViewModel;
            this.vm = (Application.Current as App).MainWindViewModel;
        }

        /**
         * Back button event
         **/
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.vm.DisConnect();
            Back?.Invoke(this, new EventArgs());
        }
    }
}
