using FlightSimulatorApp.Model;
using System;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp.ViewModel
{
    public class MapViewModel : INotifyPropertyChanged, IDisposable
    {
        public ServerModel Model { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public double zoom;
        private Location center;

        /**
         * Constructor
         **/
        public MapViewModel(ServerModel m)
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VM_Zoom"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VM_Center"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VM_Location"));
        }

        /**
         * The view model latitude property
         **/
        public double VM_Latitude
        {
            get { return Model.Latitude; }
            set { Model.Latitude = value; }
        }

        /**
         * The view model longitude property
         **/
        public double VM_Longitude
        {
            get { return Model.Longitude; }
            set { Model.Longitude = value; }
        }

        /**
         * The view model location property
         **/
        public Location VM_Location
        {
            get { return Model.Location; }
            set { Model.Location = value; }
        }

        /**
         * The view model center of screen property
         **/
        public Location VM_Center
        {
            get { return this.center; }
            set { this.center = value; }
        }

        /**
         * The view model zoom of screen property
         **/
        public double VM_Zoom
        {
            get { return this.zoom; }
            set { this.zoom = value; }
        }
    }
}