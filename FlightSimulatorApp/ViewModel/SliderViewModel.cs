using System;
using System.ComponentModel;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.ViewModel
{
    public class SliderViewModel : IDisposable
    {
        public ServerModel Model { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * Constructor
         **/
        public SliderViewModel(ServerModel m)
        {
            this.Model = m;
            Model.Aileron = -1;
            Model.Throttle = 0;
            InitEvents();
        }

        /**
         * Unsubscribe values
         **/
        public void Dispose()
        {
            Model.PropertyChanged -= NotifyPropertyChanged;
        }

        /**
         * Subscribe events
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
         * The view model aileron property
         **/
        public double VM_Aileron
        {
            get { return Model.Aileron; }
            set
            {
                Model.Aileron = value;
                //send the new aileron value
                string path = "set /controls/flight/aileron " + value.ToString("N5");
                try
                {
                    string num = Model.ManualSend(path + " \n");
                    if (Double.TryParse(num, out double val))
                    {
                        Model.Aileron = val;
                    }
                }
                catch (Exception)
                {
                    Model.AddStatement("Failed to read Ailron value from simulator");
                }
            }
        }

        /**
         * The view model throttle property
         **/
        public double VM_Throttle
        {
            get { return Model.Throttle; }
            set
            {
                Model.Throttle = value;
                //send the new elevator value
                string path = "set /controls/engines/current-engine/throttle " + value.ToString("N5");
                try
                {
                    string num = Model.ManualSend(path + " \n");
                    if (Double.TryParse(num, out double val))
                    {
                        Model.Throttle = val;
                    }
                }
                catch (Exception)
                {
                    Model.AddStatement("Failed to read Throttle value from simulator");
                }
            }
        }
    }
}