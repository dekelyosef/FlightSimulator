using System.Windows;
using System.Windows.Controls;

namespace FlightSimulatorApp.View.Controls
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class ControlPanel : UserControl
    {
        /**
         * Constructor
         **/
        public ControlPanel()
        {
            InitializeComponent();
            DataContext = (Application.Current as App).ControlPanelViewModel;
        }
    }
}