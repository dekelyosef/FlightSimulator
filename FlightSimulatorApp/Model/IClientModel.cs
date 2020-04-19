using System.ComponentModel;

namespace FlightSimulatorApp.Model
{
    public interface IClientModel : INotifyPropertyChanged
    {
        //open the server socket
        void Connect(string ip, int port);

        //close the connection
        void Disconnect();

        //Write to server
        void Write(string command);

        //Read from server
        string Read();
    }
}