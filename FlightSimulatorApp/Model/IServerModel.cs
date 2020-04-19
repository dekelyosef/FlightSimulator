using Microsoft.Maps.MapControl.WPF;
using System.ComponentModel;

namespace FlightSimulatorApp.Model
{
    public interface IServerModel : INotifyPropertyChanged
    {
        //connetion to the simulator
        void Connect();
        void Disconnect();
        void Start();
        string ManualSend(string path);
        void AddStatement(string str);

        //flightSimulator properties
        //notification
        string Note { set; get; }
        string NoteColor { set; get; }
        string MenuNote { set; get; }
        //control panel
        //values
        string Heading { set; get; }
        string GroundSpeed { set; get; }
        string VerticalSpeed { set; get; }
        string AirSpeed { set; get; }
        string Altitude { set; get; }
        string Roll { set; get; }
        string Pitch { set; get; }
        string Altimeter { set; get; }
        //colors
        string HeadingColor { set; get; }
        string GroundSpeedColor { set; get; }
        string VerticalSpeedColor { set; get; }
        string AirSpeedColor { set; get; }
        string AltitudeColor { set; get; }
        string RollColor { set; get; }
        string PitchColor { set; get; }
        string AltimeterColor { set; get; }
        //map
        double Latitude { set; get; }
        double Longitude { set; get; }
        Location Location { set; get; }
        Location Center { set; get; }
        double Zoom { set; get; }
        //joystick
        double Rudder { set; get; }
        double Elevator { set; get; }
        double Aileron { set; get; }
        double Throttle { set; get; }
    }
}