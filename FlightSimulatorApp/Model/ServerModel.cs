using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace FlightSimulatorApp.Model
{
    public class ServerModel : IServerModel
    {
        public MyClientModel client;
        public bool stop;
        public bool wait;
        public bool isConnect;
        public event PropertyChangedEventHandler PropertyChanged;
        private Queue<string> messages;

        //mutex
        public event EventHandler LostConnection;
        private readonly Mutex mutex;
        private readonly Mutex m;

        private string headline;
        private string note;
        private string menuNote;
        private string noteColor;
        //control panel properties
        //values property
        public string heading;
        public string airSpeed;
        public string altitude;
        public string roll;
        public string pitch;
        public string altimeter;
        public string groundSpeed;
        public string verticalSpeed;
        //colors property
        public string headingColor;
        public string airSpeedColor;
        public string altitudeColor;
        public string rollColor;
        public string pitchColor;
        public string altimeterColor;
        public string groundSpeedColor;
        public string verticalSpeedColor;
        //map properties
        private double latitude;
        private double longitude;
        private double zoom;
        private Location location;
        private Location center;
        //Joystick properties
        private double rudder;
        private double elevator;
        private double aileron;
        private double throttle;
        //MainManu properties
        private string userName;
        private string ip;
        private string port;

        /**
         * Constructor
         **/
        public ServerModel(MyClientModel c)
        {
            this.stop = false;
            this.client = c;
            this.isConnect = false;
            this.mutex = new Mutex();
            this.m = new Mutex();
            Ip = ConfigurationManager.AppSettings["IP"].ToString();
            Port = ConfigurationManager.AppSettings["Port"].ToString();
            Latitude = -200;
            Longitude = -200;
            messages = new Queue<string>();
        }

        /**
         * Opens a server to recieve data from the client
         **/
        public void Connect()
        {
            bool invalid = false;

            //checks if the given port number contains only numbers
            if (!int.TryParse(port, out int portNum))
            {
                invalid = true;
                SetNoteColor("RED");
                this.MenuNote = "Port number is not valid, Try again...";
            }

            try
            {
                //connect to client
                this.client.Connect(ip, portNum);
                SetNoteColor("GREEN");
                AddStatement("Connected to Server on ip: " + ip + ", port: " + port);
                this.isConnect = true;
            }
            catch (Exception)
            {
                if (!invalid)
                {
                    SetNoteColor("RED");
                    this.MenuNote = "Failed to connect server! Try again..";
                }
            }
        }

        /**
         * Close the connection
         **/
        public void Disconnect()
        {
            this.stop = true;
            if (isConnect)
            {
                try
                {
                    client.Disconnect();
                    isConnect = false;
                }
                catch (Exception)
                {
                    SetNoteColor("RED");
                    AddStatement("Client was disconnected!");
                }
            }
        }

        /**
         * Opens new thread for sending get requests and reading the values back
         * */
        public void Start()
        {
            //if server isn't connect to client
            if (!isConnect)
            {
                return;
            }
            new Thread(delegate ()
            {
                while (!stop)
                {
                    string msg = "";
                    try
                    {
                        msg = WriteAndRead(GetCommands(), true);
                    }
                    catch (IOException)
                    {
                        stop = true;
                        SetNoteColor("RED");
                        AddStatement("Lost connection to simulator! please re-connect...");

                        this.client.Disconnect();
                        LostConnection?.Invoke(this, new EventArgs());
                    }
                    catch (Exception)
                    {
                        SetNoteColor("RED");
                        AddStatement("Error writing or reading from simulator");
                    }
                    try
                    {
                        this.SetCommands(msg);
                    }
                    catch (Exception)
                    {
                        SetNoteColor("RED");
                        AddStatement("Error setting values");
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }

        /**
         * Write to client and read from client
         **/
        public string WriteAndRead(string msg, bool wait)
        {
            //mutex
            this.mutex.WaitOne();
            this.client.Write(msg);
            //if writing to simulator the contol panel's values wait 10 sec before reading
            if (wait)
            {
                Thread.Sleep(1000);
            }
            string str = this.client.Read();
            this.mutex.ReleaseMutex();
            return str;
        }

        /**
        * Push new message to messages queue
        **/
        public void PushMessage(string message)
        {
            this.messages.Enqueue(message);
        }

        /**
        * Get the property name according to the path
        **/
        private string GetPropName(string message)
        {
            if (message == null)
            {
                return null;
            }
            if (message.Contains("throttle"))
            {
                return "throttle";
            }
            if (message.Contains("rudder"))
            {
                return "rudder";
            }
            if (message.Contains("aileron"))
            {
                return "aileron";
            }
            if (message.Contains("elevator"))
            {
                return "elevator";
            }

            return null;
        }

        /**
        * Set a property to a given value by the property name
        **/
        private void SetProp(double value, string propName)
        {
            if (propName == "throttle")
            {
                this.Throttle = value;
            }
            if (propName == "rudder")
            {
                this.Rudder = value;
            }
            if (propName == "aileron")
            {
                this.Aileron = value;
            }
            if (propName == "elevator")
            {
                this.Elevator = value;
            }
        }

        /**
        * Sending set messages to simulator via Thread
        * The thread takes the messages from the messages queue
        **/
        public void SendToSimulator()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    //If queue is empty
                    if (this.messages.Count == 0)
                    {
                        continue;
                    }
                    try
                    {
                        //Send first message in queue and send to simulator
                        double val;
                        string toSend = messages.Dequeue();
                        string propName = GetPropName(toSend);
                        string value = WriteAndRead(toSend, false);
                        if (!Double.TryParse(value, out val))
                        {
                            SetNoteColor("RED");
                            AddStatement("Error setting the " + propName + " value");
                        }
                        else
                        {
                            SetProp(val, propName);
                        }
                    }
                    catch (IOException)
                    {
                        stop = true;
                        SetNoteColor("RED");
                        AddStatement("Lost connection to simulator! please re-connect...");

                        this.client.Disconnect();
                        LostConnection?.Invoke(this, new EventArgs());
                    }
                    catch (Exception)
                    {
                        SetNoteColor("RED");
                        AddStatement("Error writing or reading from simulator");
                    }
                }
            }).Start();
        }
        

        /**
         * Get the commands for sending to client
         **/
        public string GetCommands()
        {
            return "get /instrumentation/heading-indicator/indicated-heading-deg \n" +
                "get /instrumentation/gps/indicated-vertical-speed \n" +
                "get /instrumentation/gps/indicated-ground-speed-kt \n" +
                "get /instrumentation/airspeed-indicator/indicated-speed-kt \n" +
                "get /instrumentation/gps/indicated-altitude-ft \n" +
                "get /instrumentation/attitude-indicator/internal-roll-deg \n" +
                "get /instrumentation/attitude-indicator/internal-pitch-deg \n" +
                "get /instrumentation/altimeter/indicated-altitude-ft \n" +
                "get /position/latitude-deg \n" +
                "get /position/longitude-deg \n";
        }

        /**
         * Parse the given stream of tokens to strings values and update the comnnad's values
         **/
        public void SetCommands(string str)
        {
            string[] values = str.Split('\n');
            //checks if the given stream isn't valid
            if (values.Length < 10)
            {
                return;
            }
            //Control panel properties
            HeadingColor = "BLACK";
            this.Heading = values[0];
            if (values[0] == "ERR")
            {
                HeadingColor = "RED";
                SetNoteColor("RED");
                AddStatement("Error getting the Heading value");
            }
            VerticalSpeedColor = "BLACK";
            this.VerticalSpeed = values[1];
            if (values[1] == "ERR")
            {
                VerticalSpeedColor = "RED";
                SetNoteColor("RED");
                AddStatement("Error getting the VerticalSpeed value");
            }
            GroundSpeedColor = "BLACK";
            this.GroundSpeed = values[2];
            if (values[2] == "ERR")
            {
                GroundSpeedColor = "RED";
                SetNoteColor("RED");
                AddStatement("Error getting the GroundSpeed value");
            }
            AirSpeedColor = "BLACK";
            this.AirSpeed = values[3];
            if (values[3] == "ERR")
            {
                AirSpeedColor = "RED";
                SetNoteColor("RED");
                AddStatement("Error getting the AirSpeed value");
            }
            AltitudeColor = "BLACK";
            this.Altitude = values[4];
            if (values[4] == "ERR")
            {
                AltitudeColor = "RED";
                SetNoteColor("RED");
                AddStatement("Error getting the Altitude value");
            }
            RollColor = "BLACK";
            this.Roll = values[5];
            if (values[5] == "ERR")
            {
                RollColor = "RED";
                SetNoteColor("RED");
                AddStatement("Error getting the Roll value");
            }
            PitchColor = "BLACK";
            this.Pitch = values[6];
            if (values[6] == "ERR")
            {
                PitchColor = "RED";
                SetNoteColor("RED");
                AddStatement("Error getting the Pitch value");
            }
            AltimeterColor = "BLACK";
            this.Altimeter = values[7];
            if (values[7] == "ERR")
            {
                AltimeterColor = "RED";
                SetNoteColor("RED");
                AddStatement("Error getting the Altimeter value");
            }

            //Map properties
            if (Double.TryParse(values[8], out double valLat))
            {
                //checks bounds
                if (valLat > 180)
                {
                    this.Latitude = 180;
                    SetNoteColor("RED");
                    AddStatement("Latitude value exceeds the bounds");
                }
                else if (valLat < -180)
                {
                    this.Latitude = -180;
                    SetNoteColor("RED");
                    AddStatement("Latitude value exceeds the bounds");
                }
                else
                {
                    this.Latitude = valLat;
                }
            }
            else
            {
                SetNoteColor("RED");
                AddStatement("Error getting the Latitude value");
            }
            if (Double.TryParse(values[9], out double valLon))
            {
                //checks bounds
                if (valLon > 90)
                {
                    this.Longitude = 90;
                    SetNoteColor("RED");
                    AddStatement("Longitude value exceeds the bounds");
                }
                else if (valLon < -90)
                {
                    this.Longitude = -90;
                    SetNoteColor("RED");
                    AddStatement("Longitude value exceeds the bounds");
                }
                else
                {
                    this.Longitude = valLon;
                }
            }
            else
            {
                SetNoteColor("RED");
                AddStatement("Error getting the Longitude value");
            }
        }

        /**
         * Method that called by the Set accessor of each property
         **/
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /**
         * Is the connection open
         **/
        public Boolean IsConnect()
        {
            return this.isConnect;
        }

        /**
         * Set notification property
         **/
        public void AddStatement(string str)
        {
            //mute
            m.WaitOne();
            this.Note = str;
            m.ReleaseMutex();
        }

        /**
         * Main window notification property
         **/
        public string Note
        {
            get { return this.note; }
            set
            {
                if (this.note != value)
                {
                    this.note = value;
                    //notify the changes
                    NotifyPropertyChanged("Note");
                }
            }
        }

        /**
         * Main menu notification property
         **/
        public string MenuNote
        {
            get { return this.menuNote; }
            set
            {
                if (this.menuNote != value)
                {
                    this.menuNote = value;
                    //notify the changes
                    NotifyPropertyChanged("MenuNote");
                }
            }
        }

        /**
         * A color property for displaying the notifications 
         **/
        public string NoteColor
        {
            get { return this.noteColor; }
            set
            {
                if (this.noteColor != value)
                {
                    this.noteColor = value;
                    //notify the changes
                    NotifyPropertyChanged("NoteColor");
                }
            }
        }

        /**
         * Set color property
         **/
        public void SetNoteColor(string str)
        {
            this.NoteColor = str;
        }

        /**
         * The heading property
         **/
        public string Heading
        {
            get { return this.heading; }
            set
            {
                if (this.heading != value)
                {
                    this.heading = value;
                    //notify the changes
                    NotifyPropertyChanged("Heading");
                }
            }
        }
        /**
         * A color property for displaying the heading 
         **/
        public string HeadingColor
        {
            get { return this.headingColor; }
            set
            {
                if (this.headingColor != value)
                {
                    this.headingColor = value;
                    //notify the changes
                    NotifyPropertyChanged("HeadingColor");
                }
            }
        }

        /**
         * The airSpeed property
         **/
        public string AirSpeed
        {
            get { return this.airSpeed; }
            set
            {
                if (this.airSpeed != value)
                {
                    this.airSpeed = value;
                    //notify the changes
                    NotifyPropertyChanged("AirSpeed");
                }
            }
        }
        /**
         * A color property for displaying the airSpeed 
         **/
        public string AirSpeedColor
        {
            get { return this.airSpeedColor; }
            set
            {
                if (this.airSpeedColor != value)
                {
                    this.airSpeedColor = value;
                    //notify the changes
                    NotifyPropertyChanged("AirSpeedColor");
                }
            }
        }

        /**
         * The altitude property
         **/
        public string Altitude
        {
            get { return this.altitude; }
            set
            {
                if (this.altitude != value)
                {
                    this.altitude = value;
                    //notify the changes
                    NotifyPropertyChanged("Altitude");
                }
            }
        }
        /**
         * A color property for displaying the altitudeColor 
         **/
        public string AltitudeColor
        {
            get { return this.altitudeColor; }
            set
            {
                if (this.altitudeColor != value)
                {
                    this.altitudeColor = value;
                    //notify the changes
                    NotifyPropertyChanged("AltitudeColor");
                }
            }
        }

        /**
         * The roll property
         **/
        public string Roll
        {
            get
            {
                return this.roll;
            }
            set
            {
                if (this.roll != value)
                {
                    this.roll = value;
                    //notify the changes
                    NotifyPropertyChanged("Roll");
                }
            }
        }
        /**
         * A color property for displaying the roll 
         **/
        public string RollColor
        {
            get
            {
                return this.rollColor;
            }
            set
            {
                if (this.rollColor != value)
                {
                    this.rollColor = value;
                    //notify the changes
                    NotifyPropertyChanged("RollColor");
                }
            }
        }

        /**
         * The pitch property
         **/
        public string Pitch
        {
            get { return this.pitch; }
            set
            {
                if (this.pitch != value)
                {
                    this.pitch = value;
                    //notify the changes
                    NotifyPropertyChanged("Pitch");
                }
            }
        }
        /**
         * A color property for displaying the pitch 
         **/
        public string PitchColor
        {
            get { return this.pitchColor; }
            set
            {
                if (this.pitchColor != value)
                {
                    this.pitchColor = value;
                    //notify the changes
                    NotifyPropertyChanged("PitchColor");
                }
            }
        }

        /**
         * The altimeter property
         **/
        public string Altimeter
        {
            get { return this.altimeter; }
            set
            {
                if (this.altimeter != value)
                {
                    this.altimeter = value;
                    //notify the changes
                    NotifyPropertyChanged("Altimeter");
                }
            }
        }
        /**
         * A color property for displaying the altimeter 
         **/
        public string AltimeterColor
        {
            get { return this.altimeterColor; }
            set
            {
                if (this.altimeterColor != value)
                {
                    this.altimeterColor = value;
                    //notify the changes
                    NotifyPropertyChanged("AltimeterColor");
                }
            }
        }

        /**
         * The groundSpeed property
         **/
        public string GroundSpeed
        {
            get { return this.groundSpeed; }
            set
            {
                if (this.groundSpeed != value)
                {
                    this.groundSpeed = value;
                    //notify the changes
                    NotifyPropertyChanged("GroundSpeed");
                }
            }
        }
        /**
         * A color property for displaying the groundSpeed 
         **/
        public string GroundSpeedColor
        {
            get { return this.groundSpeedColor; }
            set
            {
                if (this.groundSpeedColor != value)
                {
                    this.groundSpeedColor = value;
                    //notify the changes
                    NotifyPropertyChanged("GroundSpeedColor");
                }
            }
        }

        /**
         * The verticalSpeed property
         **/
        public string VerticalSpeed
        {
            get { return this.verticalSpeed; }
            set
            {
                if (this.verticalSpeed != value)
                {
                    this.verticalSpeed = value;
                    //notify the changes
                    NotifyPropertyChanged("VerticalSpeed");
                }
            }
        }
        /**
         * A color property for displaying the verticalSpeed 
         **/
        public string VerticalSpeedColor
        {
            get { return this.verticalSpeedColor; }
            set
            {
                if (this.verticalSpeedColor != value)
                {
                    this.verticalSpeedColor = value;
                    //notify the changes
                    NotifyPropertyChanged("VerticalSpeedColor");
                }
            }
        }

        /**
         * The latitude property
         **/
        public double Latitude
        {
            get { return this.latitude; }
            set
            {
                if (this.latitude != value)
                {
                    this.latitude = value;
                    //notify the changes
                    NotifyPropertyChanged("Latitude");
                }
            }
        }

        /**
         * The longitude property
         **/
        public double Longitude
        {
            get { return this.longitude; }
            set
            {
                if (this.longitude != value)
                {
                    this.longitude = value;
                    //notify the changes
                    NotifyPropertyChanged("Longitude");
                }
            }
        }

        /**
         * The location property
         **/
        public Location Location
        {
            get { return new Location(this.latitude, this.longitude); }
            set
            {
                if (this.location != value)
                {
                    this.location = value;
                    //notify the changes
                    NotifyPropertyChanged("Location");
                }
            }
        }

        public Location Center
        {
            get
            {
                if (this.Longitude == -200 && this.Latitude == -200)
                {
                    //initialize the center screen
                    return new Location(0, 0);
                }
                return new Location(this.latitude, this.longitude);
            }
            set
            {
                if (this.center != value)
                {
                    this.center = value;
                    //notify the changes
                    NotifyPropertyChanged("Center");
                }
            }
        }

        /**
         * The zoom screen property
         **/
        public double Zoom
        {
            get { return this.zoom; }
            set
            {
                if (this.zoom != value)
                {
                    this.zoom = value;
                    //notify the changes
                    NotifyPropertyChanged("Zoom");
                }
            }
        }

        /**
         * The rudder property
         **/
        public Double Rudder
        {
            get { return this.rudder; }
            set
            {
                if (this.rudder != value)
                {
                    this.rudder = value;
                    //notify the changes
                    NotifyPropertyChanged("Rudder");
                }
            }
        }

        /**
         * The elevator property
         **/
        public Double Elevator
        {
            get { return this.elevator; }
            set
            {
                if (this.elevator != value)
                {
                    this.elevator = value;
                    //notify the changes
                    NotifyPropertyChanged("Elevator");
                }
            }
        }

        /**
         * The aileron property
         **/
        public Double Aileron
        {
            get { return this.aileron; }
            set
            {
                if (this.aileron != value)
                {
                    this.aileron = value;
                    //notify the changes
                    NotifyPropertyChanged("Aileron");
                }
            }
        }

        /**
         * The throttle property
         **/
        public Double Throttle
        {
            get { return this.throttle; }
            set
            {
                if (this.throttle != value)
                {
                    this.throttle = value;
                    //notify the changes
                    NotifyPropertyChanged("Throttle");
                }
            }
        }

        /**
         * UserName property
         **/
        public string UserName
        {
            get { return this.userName; }
            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    NotifyPropertyChanged("UserName");
                    //Set the headline to show the name
                    this.Headline = value + "'s Controller";

                }
            }
        }

        /**
         * Ip property
         **/
        public string Ip
        {
            get { return this.ip; }
            set
            {
                if (this.ip != value)
                {
                    this.ip = value;
                }
            }
        }

        /**
         * Port property
         **/
        public string Port
        {
            get { return this.port; }
            set
            {
                if (this.port != value)
                {
                    this.port = value;
                }
            }
        } 
        
        /**
         * Headline property
         **/
        public string Headline
        {
            get { return this.headline; }
            set
            {
                if (this.headline != value)
                {

                    //Format the name so that every word starts with upper case
                    this.headline = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
                    NotifyPropertyChanged("Headline");

                }
            }
        }
    }
}