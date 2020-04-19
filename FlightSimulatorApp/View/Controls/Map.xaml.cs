using FlightSimulatorApp.ViewModel;
using Microsoft.Maps.MapControl.WPF;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FlightSimulatorApp.View.Controls
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        private readonly MapViewModel vm;
        private double lat;
        private double lon;
        private double latCenter;
        private double lonCenter;
        private double tempLat;
        private double tempLon;
        private bool flag;
        private bool firstCenter;
        private bool firstZoom;

        /**
         * Constructor
         **/
        public Map()
        {
            InitializeComponent();
            DataContext = (Application.Current as App).MapViewModel;
            this.vm = (Application.Current as App).MapViewModel;
            this.vm.PropertyChanged += new PropertyChangedEventHandler(Trace);
            this.flag = true;
            this.vm.VM_Center = new Location(0, 0);
            this.lat = this.vm.VM_Latitude;
            this.lon = this.vm.VM_Longitude;
            this.firstCenter = true;
            this.firstZoom = true;
        }

        /**
         * Trace after pushpin
         **/
        private void Trace(object sender, PropertyChangedEventArgs e)
        {
            this.tempLat = this.vm.VM_Latitude;
            this.tempLon = this.vm.VM_Longitude;

            //initialize the zoom of screen
            if (this.flag)
            {
                this.vm.VM_Zoom = 0;
                this.flag = false;
                return;
            }

            //initialize the center while getting the values from client
            if (firstCenter)
            {
                if (this.lat != tempLat && this.lon != tempLon)
                {
                    this.latCenter = tempLat;
                    this.lonCenter = tempLon;
                    this.vm.VM_Center = new Location(this.vm.VM_Latitude, this.vm.VM_Longitude);
                    this.firstCenter = false;
                }
            }

            if (this.lat != tempLat && this.lon != tempLon)
            {
                CheckSpeed();
            }
        }

        /**
         * Set zoom and center acording to the speed
         **/
        public void CheckSpeed()
        {
            //finds the difference
            double difLat = this.tempLat - this.lat;
            double difLon = this.tempLon - this.lon;

            if (this.lat + 0.001 >= tempLat && this.lon + 0.001 >= tempLon)
            {
                if (this.latCenter + (difLat * 18) < tempLat || this.lonCenter + (difLon * 18) < tempLon)
                {
                    SetCenter(tempLat, tempLon);
                    this.vm.VM_Zoom = 12;
                }
                if (firstZoom)
                {
                    this.vm.VM_Zoom = 12;
                    this.firstZoom = false;
                }
            }
            else if (this.lat + 0.005 >= tempLat && this.lon + 0.005 >= tempLon)
            {
                if (firstZoom)
                {
                    this.vm.VM_Zoom = 10;
                    this.firstZoom = false;
                }
                if (this.latCenter + (difLat * 16) < tempLat || this.lonCenter + (difLon * 16) < tempLon)
                {
                    SetCenter(tempLat, tempLon);
                    this.vm.VM_Zoom = 10;
                }
            }
            else if (this.lat + 0.1 >= tempLat && this.lon + 0.1 >= tempLon)
            {
                if (this.latCenter + (difLat * 4) < tempLat || this.lonCenter + (difLon * 4) < tempLon)
                {
                    SetCenter(tempLat, tempLon);
                    this.vm.VM_Zoom = 8;
                }
                if (firstZoom)
                {
                    this.vm.VM_Zoom = 8;
                    this.firstZoom = false;
                }
            }
            else if (this.lat + 0.5 >= tempLat && this.lon + 0.5 >= tempLon)
            {
                if (this.latCenter + (difLat * 3) < tempLat || this.lonCenter + (difLon * 3) < tempLon)
                {
                    SetCenter(tempLat, tempLon);
                    this.vm.VM_Zoom = 6;
                }
                if (firstZoom)
                {
                    this.vm.VM_Zoom = 6;
                    this.firstZoom = false;
                }
            }
            else if (this.lat + 1 >= tempLat && this.lon + 1 >= tempLon)
            {
                if (this.latCenter + (difLat * 2) < tempLat || this.lonCenter + (difLon * 2) < tempLon)
                {
                    SetCenter(tempLat, tempLon);
                    this.vm.VM_Zoom = 4;
                }
                if (firstZoom)
                {
                    this.vm.VM_Zoom = 4;
                    this.firstZoom = false;
                }
            }
            else if (this.lat + 2 >= tempLat && this.lon + 2 >= tempLon)
            {
                if (this.latCenter + (difLat * 4) < tempLat || this.lonCenter + (difLon * 4) < tempLon)
                {
                    SetCenter(tempLat, tempLon);
                    this.vm.VM_Zoom = 2;
                }
                if (firstZoom)
                {
                    this.vm.VM_Zoom = 2;
                    this.firstZoom = false;
                }
            }
            else
            {
                if (this.latCenter + (difLat * 6) < tempLat || this.lonCenter + (difLon * 6) < tempLon)
                {
                    SetCenter(tempLat, tempLon);
                    this.vm.VM_Zoom = 0;
                }
                if (firstZoom)
                {
                    this.vm.VM_Zoom = 0;
                }
            }

            this.lat = tempLat;
            this.lon = tempLon;
        }

        public void SetCenter(double latitude, double longitude)
        {
            //if the values are valid
            if (latitude != -180 && latitude != 180 && longitude != -90 && longitude != 90)
            {
                this.latCenter = this.vm.VM_Latitude;
                this.lonCenter = this.vm.VM_Longitude;
            }
            this.vm.VM_Center = new Location(this.latCenter, this.lonCenter);
        }
    }
}