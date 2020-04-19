using FlightSimulatorApp.Model;
using FlightSimulatorApp.ViewModel;
using System.Windows;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //window
        public MainWindow MainWind { get; internal set; }
        public OpenWindow OpenWind { get; internal set; }
        //models
        public ServerModel TheModel { get; internal set; }
        public MyClientModel MyClientModel { get; internal set; }
        //view models
        public MainWindowViewModel MainWindViewModel { get; internal set; }
        public MainMenuViewModel MainMenuViewModel { get; internal set; }
        public ControlPanelViewModel ControlPanelViewModel { get; internal set; }
        public JoystickViewModel JoystickViewModel { get; internal set; }
        public MapViewModel MapViewModel { get; internal set; }
        public SliderViewModel SliderViewModel { get; internal set; }
        public TutorialWindow TutorialWind { get; internal set; }

        /**
         * Set view models for open the main menu again
         **/
        public void DisposeAll()
        {
            ControlPanelViewModel.Dispose();
            ControlPanelViewModel = null;
            JoystickViewModel.Dispose();
            JoystickViewModel = null;
            MapViewModel.Dispose();
            MapViewModel = null;
            SliderViewModel.Dispose();
            SliderViewModel = null;
            MainMenuViewModel.Dispose();
            MainMenuViewModel = null;
            TheModel = null;
            MyClientModel = null;
            OpenWind = null;
        }

        /**
         * Initialize the view models
         **/
        public void Bootstrap(bool first)
        {
            MyClientModel = new MyClientModel();
            TheModel = new ServerModel(MyClientModel);
            ControlPanelViewModel = new ControlPanelViewModel(TheModel);
            JoystickViewModel = new JoystickViewModel(TheModel);
            MapViewModel = new MapViewModel(TheModel);
            MainMenuViewModel = new MainMenuViewModel(TheModel);
            MainWindViewModel = new MainWindowViewModel(TheModel);
            SliderViewModel = new SliderViewModel(TheModel);
            MainWind = new MainWindow();
            OpenWind = new OpenWindow(first);
            TutorialWind = TutorialWindow.GetInstance();
        }

        /**
         * Occurs when the program is starting, but after all add-in programs have been loaded
         **/
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Initialize the first window
            Bootstrap(true);
            WindowManager windowManager = WindowManager.GetInstance();
            windowManager.StartApp();
        }
    }
}