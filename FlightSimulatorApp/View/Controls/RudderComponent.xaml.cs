using System.Windows;
using System.Windows.Controls;

namespace FlightSimulatorApp.View.Controls
{
    /// <summary>
    /// Interaction logic for RudderComponent.xaml
    /// </summary>
    public partial class RudderComponent : UserControl
    {
        /**
         * Constructor
         **/
        public RudderComponent()
        {
            InitializeComponent();
            MySliders.DataContext = (Application.Current as App).SliderViewModel;
            Joystick.DataContext = (Application.Current as App).JoystickViewModel;
            RudderVal.DataContext = (Application.Current as App).JoystickViewModel;
            ElevatorVal.DataContext = (Application.Current as App).JoystickViewModel;

        }
    }
}