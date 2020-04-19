using FlightSimulatorApp.Model;
using System;
using System.ComponentModel;

namespace FlightSimulatorApp.ViewModel
{
    public class JoystickViewModel : INotifyPropertyChanged, IDisposable
    {
        public ServerModel Model { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * Constructor
         **/
        public JoystickViewModel(ServerModel m)
        {
            this.Model = m;
            InitEvents();
        }

        /**
         * Unsubscribe events
         **/
        public void Dispose()
        {
            Model.PropertyChanged -= NotifyPropertyChanged;
        }

        /**
         * subscribe events
         **/
        private void InitEvents()
        {
            Model.PropertyChanged += new PropertyChangedEventHandler(NotifyPropertyChanged);
        }

        /**
         * Method that called by the Set accessor of each property
         **/
        public void NotifyPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VM_" + e.PropertyName));
        }

        /**
         * The view model rudder property
         **/
        public double VM_Rudder
        {
            get { return (double)System.Math.Round(Model.Rudder, 2); }
            set
            {
                Model.Rudder = value;
                //send the new rudder value
                string path = "set /controls/flight/rudder " + value.ToString("N5");

                Model.PushMessage(path + " \n");

            }
        }

        /**
         * The view model elevator property
         **/
        public double VM_Elevator
        {
            get { return (double)System.Math.Round(Model.Elevator, 2); }
            set
            {
                Model.Elevator = value;
                //send the new elevator value
                string path = "set /controls/flight/elevator " + value.ToString("N5");

                Model.PushMessage(path + " \n");

            }
        }
    }
}