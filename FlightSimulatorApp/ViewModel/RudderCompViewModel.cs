using System;
using System.ComponentModel;

namespace FlightSimulatorApp.ViewModel
{
    public class RudderCompViewModel : INotifyPropertyChanged, IDisposable
    {
        public JoystickViewModel joysVM;
        public SliderViewModel slideVM;
        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * Constructor
         **/
        public RudderCompViewModel(JoystickViewModel j, SliderViewModel s)
        {
            this.joysVM = j;
            this.slideVM = s;

            InitEvents();
        }

        /**
         * Unsubscribe events
         **/
        public void Dispose()
        {
            joysVM.PropertyChanged -= NotifyPropertyChanged;
            slideVM.PropertyChanged -= NotifyPropertyChanged;
        }

        /**
         * Subscribe events
         **/
        private void InitEvents()
        {
            joysVM.PropertyChanged += new PropertyChangedEventHandler(NotifyPropertyChanged);
            slideVM.PropertyChanged += new PropertyChangedEventHandler(NotifyPropertyChanged);
        }

        /**
         * Method that called by the Set accessor of each property
         **/
        public void NotifyPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        }

        /**
         * The view model rudder property
         **/
        public double VM_Rudder
        {
            get { return joysVM.VM_Rudder; }
            set { joysVM.VM_Rudder = value; }
        }

        /**
         * The view model elevator property
         **/
        public double VM_Elevator
        {
            get { return joysVM.VM_Elevator; }
            set { joysVM.VM_Elevator = value; }
        }

        /**
         * The view model aileron property
         **/
        public double VM_Aileron
        {
            get { return slideVM.VM_Aileron; }
            set { slideVM.VM_Aileron = value; }
        }

        /**
         * The view model throttle property
         **/
        public double VM_Throttle
        {
            get { return slideVM.VM_Throttle; }
            set { slideVM.VM_Throttle = value; }
        }
    }
}