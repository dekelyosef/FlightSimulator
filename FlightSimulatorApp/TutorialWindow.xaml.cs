using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FlightSimulatorApp.View;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for TutorialWindow.xaml
    /// </summary>
    public partial class TutorialWindow : Window
    {
        public EventHandler Back;
        public Storyboard story;
        private static TutorialWindow wind = null;

        /**
         * Constructor
         * Singleton DP
         **/
        private TutorialWindow()
        {
            InitializeComponent();
            Base.Loaded += new RoutedEventHandler(StartAnimation);
            story = (Storyboard)this.FindResource("ToturialBoard");

        }

        public static TutorialWindow GetInstance()
        {
            if (wind == null)
            {
                wind = new TutorialWindow();
            }

            return wind;
        }

        /**
         * Start animation event
         **/
        private void StartAnimation(object sender, RoutedEventArgs e)
        {
            story.Begin(this, true);
            story.Pause(this);
        }

        /**
         * Back to menu event
         **/
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            story.Stop(this);
            story.Seek(TimeSpan.Zero);
            if (Back != null)
            {
                Back?.Invoke(this, new EventArgs());
            }
        }
    }
}
