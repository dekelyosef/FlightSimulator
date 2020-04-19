using System;
using System.ComponentModel;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        public ServerModel Model { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * Constructor
         **/
        public MainWindowViewModel(ServerModel m)
        {
            Model = m;
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
         * Disconnect from client
         **/
        public void DisConnect()
        {
            this.Model.Disconnect();
        }

        /**
         * The view model mainWindow notification property
         **/
        public string VM_Note
        {
            get
            {
                return Model.Note;
            }
            set
            {
                Model.Note = value;
            }
        }

        /**
         * Note - color property
         **/
        public string VM_NoteColor
        {
            get { return Model.NoteColor; }
            set
            {
                Model.NoteColor = value;
            }
        }
        
        public string VM_Headline
        {
            get { return Model.Headline; }
            set
            {
                Model.Headline = value;
            }
        }
    }
}
