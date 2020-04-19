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
            DataContext = (Application.Current as App).RudderViewModel;
        }
    }
}