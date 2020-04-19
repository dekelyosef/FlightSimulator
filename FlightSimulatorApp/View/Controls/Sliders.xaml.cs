using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlightSimulatorApp.ViewModel;

namespace FlightSimulatorApp.View.Controls
{
    /// <summary>
    /// Interaction logic for Sliders.xaml
    /// </summary>
    public partial class Sliders : UserControl
    {
        private bool aileronInProcess;
        private bool throttleInProcess;
        private readonly SliderViewModel vm;

        /**
         * Constructor
         **/
        public Sliders()
        {
            InitializeComponent();
            DataContext = (Application.Current as App).SliderViewModel;
            this.vm = (Application.Current as App).SliderViewModel;
            aileronInProcess = false;
            throttleInProcess = false;
            AileronSlider.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(AileronSlider_MouseDown), true);
            ThrottleSlider.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(ThrottleSlider_MouseDown), true);
        }

        /**
         * Aileron mouseDown event
         **/
        private void AileronSlider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.aileronInProcess = true;
        }

        /**
         * Aileron mouseuUp event
         **/
        private void AileronSlider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.aileronInProcess = false;
        }

        /**
         * Aileron mouseMove event
         **/
        private void AileronSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (aileronInProcess)
            {
                //update the aileron property
                this.vm.VM_Aileron = AileronSlider.Value;
            }
        }

        /**
         * Throttle mouseDown event
         **/
        private void ThrottleSlider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.throttleInProcess = true;
        }

        /**
         * Throttle mouseuUp event
         **/
        private void ThrottleSlider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.throttleInProcess = false;
        }

        /**
         * Throttle mouseMove event
         **/
        private void ThrottleSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (throttleInProcess)
            {
                //update the throttle property
                this.vm.VM_Throttle = ThrottleSlider.Value;
            }
        }
    }
}