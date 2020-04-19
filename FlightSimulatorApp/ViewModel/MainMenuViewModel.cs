using System;
using System.ComponentModel;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.ViewModel
{
    public class MainMenuViewModel : INotifyPropertyChanged, IDisposable
    {
        public ServerModel Model { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * Constructor
         **/
        public MainMenuViewModel(ServerModel m)
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

        public Boolean Connect()
        {
            this.Model.Connect();
            if (this.Model.IsConnect())
            {
                //start threads in charge of sending set and get requests
                this.Model.Start();
                this.Model.SendToSimulator();
                return true;
            }
            return false;
        }

        /**
         * The view model userName property
         **/
        public string VM_UserName
        {
            get { return Model.UserName; }
            set { Model.UserName = value; }
        }

        /**
         * The view model ip property
         **/
        public string VM_Ip
        {
            get { return Model.Ip; }
            set { Model.Ip = value; }
        }

        /**
         * The view model port property
         **/
        public string VM_Port
        {
            get { return Model.Port; }
            set { Model.Port = value; }
        }

        /**
         * The view model mainMenu notification property
         **/
        public string VM_MenuNote
        {
            get { return Model.MenuNote; }
            set { Model.MenuNote = value; }
        }
    }
}