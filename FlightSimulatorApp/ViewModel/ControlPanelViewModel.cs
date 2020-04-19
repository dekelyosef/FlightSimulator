using FlightSimulatorApp.Model;
using System;
using System.ComponentModel;

namespace FlightSimulatorApp.ViewModel
{
    public class ControlPanelViewModel : INotifyPropertyChanged, IDisposable
    {
        public ServerModel Model { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * Constructor
         **/
        public ControlPanelViewModel(ServerModel m)
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
         * The view model heading property
         **/
        public string VM_Heading
        {
            get { return Model.Heading; }
            set { Model.Heading = value; }
        }
        /**
         * Heading - color property
         **/
        public string VM_HeadingColor
        {
            get { return Model.HeadingColor; }
            set { Model.HeadingColor = value; }
        }

        /**
         * The view model airSpeed property
         **/
        public string VM_AirSpeed
        {
            get { return Model.AirSpeed; }
            set { Model.AirSpeed = value; }
        }
        /**
         * AirSpeed - color property
         **/
        public string VM_AirSpeedColor
        {
            get { return Model.AirSpeedColor; }
            set { Model.AirSpeedColor = value; }
        }

        /**
         * The view model altitude property
         **/
        public string VM_Altitude
        {
            get { return Model.Altitude; }
            set { Model.Altitude = value; }
        }
        /**
         * Altitude - color property
         **/
        public string VM_AltitudeColor
        {
            get { return Model.AltitudeColor; }
            set { Model.AltitudeColor = value; }
        }

        /**
         * The view model roll property
         **/
        public string VM_Roll
        {
            get { return Model.Roll; }
            set { Model.Roll = value; }
        }
        /**
         * Roll - color property
         **/
        public string VM_RollColor
        {
            get { return Model.RollColor; }
            set { Model.RollColor = value; }
        }

        /**
         * The view model pitch property
         **/
        public string VM_Pitch
        {
            get { return Model.Pitch; }
            set { Model.Pitch = value; }
        }
        /**
         * Pitch - color property
         **/
        public string VM_PitchColor
        {
            get { return Model.PitchColor; }
            set { Model.PitchColor = value; }
        }

        /**
         * The view model altimeter property
         **/
        public string VM_Altimeter
        {
            get { return Model.Altimeter; }
            set { Model.Altimeter = value; }
        }
        /**
         * Altimeter - color property
         **/
        public string VM_AltimeterColor
        {
            get { return Model.AltimeterColor; }
            set { Model.AltimeterColor = value; }
        }

        /**
         * The view model groundSpeed property
         **/
        public string VM_GroundSpeed
        {
            get { return Model.GroundSpeed; }
            set { Model.GroundSpeed = value; }
        }
        /**
         * GroundSpeed - color property
         **/
        public string VM_GroundSpeedColor
        {
            get { return Model.GroundSpeedColor; }
            set { Model.GroundSpeedColor = value; }
        }

        /**
         * The view model verticalSpeed property
         **/
        public string VM_VerticalSpeed
        {
            get { return Model.VerticalSpeed; }
            set { Model.VerticalSpeed = value; }
        }
        /**
         * VerticalSpeed - color property
         **/
        public string VM_VerticalSpeedColor
        {
            get { return Model.VerticalSpeedColor; }
            set { Model.VerticalSpeedColor = value; }
        }
    }
}