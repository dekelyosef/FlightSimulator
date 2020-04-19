using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FlightSimulatorApp.Model
{
    public class MyClientModel : IClientModel
    {
        private TcpClient client = null;
        private NetworkStream stream = null;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly object lockObj;


        /**
         * Constructor
         **/
        public MyClientModel()
        {
            this.lockObj = new object();
        }

        /**
         * Open a new Tcp Client connection to the server
         **/
        public void Connect(string ip, int port)
        {
            this.client = new TcpClient(ip, port);
            this.stream = client.GetStream();
            this.stream.ReadTimeout = 10000;
            this.stream.WriteTimeout = 10000;
            //clears buffers for this stream and causes any buffered data to be written to the file
            this.stream.Flush();
        }

        /**
         * Close the client and the network stream
         **/
        public void Disconnect()
        {
            this.client.Close();
            this.stream.Close();
        }

        /**
         * Read from server
         **/
        public string Read()
        {
            lock (lockObj)
            {
                byte[] bytes = new byte[1024];
                int bytesRead = this.stream.Read(bytes, 0, bytes.Length);
                return Encoding.ASCII.GetString(bytes, 0, bytesRead);
            }
        }

        /**
         * Send the string to the server
         **/
        public void Write(string command)
        {
            lock (lockObj)
            {
                //convert the command string to an array of bytes and sent to the server
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                this.stream.Write(buffer, 0, buffer.Length);
                this.stream.Flush();
            }
        }

        /**
         * Method that called by the Set accessor of each property
         **/
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}