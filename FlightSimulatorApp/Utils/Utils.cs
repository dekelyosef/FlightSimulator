namespace FlightSimulatorApp.Utils
{
    public class Utils
    {
        /**
         * Adjusting values measured on different scales to a notionally common scale
         **/
        public static double Normalize(double n, double a, double b, double xMin, double xMax)
        {
            return ((b - a) * ((n - xMin) / (xMax - xMin))) + a;
        }
    }
}