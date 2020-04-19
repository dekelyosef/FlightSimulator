using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for OpenWindow.xaml
    /// </summary>
    public partial class OpenWindow : Window
    {
        /**
         * Constructor
         **/
        public OpenWindow(bool first)
        {
            InitializeComponent();
            //For first creation, initialize events
            if (first)
            {
                InitEvents();
            }
        }

        /**
         * Add events
         **/
        private void InitEvents()
        {
            UserMenu.MenuBase.Loaded += new RoutedEventHandler(MenuLoaded);
            WelcomeBase.Loaded += new RoutedEventHandler(WelcomeLabelLoaded);
            Storyboard story = (Storyboard)this.FindResource("welcomeStoryBoard");
            story.Completed += AnimationCompleted;

        }

        /**
         * Start and control the menu animation
         **/
        public void MenuLoaded(Object sender, RoutedEventArgs e)
        {
            Storyboard story = (Storyboard)UserMenu.FindResource("MenuStoryBoard");
            UserMenu.MenuBase.Visibility = Visibility.Hidden;
            story.Seek(TimeSpan.Zero);
            story.Begin(UserMenu, true);
            story.Pause();
        }

        /**
         * Resume menu fade in animation when welcome animation ends
         **/
        private void AnimationCompleted(Object sender, EventArgs e)
        {
            Storyboard story = (Storyboard)UserMenu.FindResource("MenuStoryBoard");
            UserMenu.MenuBase.Visibility = Visibility.Visible;

            story.Seek(TimeSpan.Zero);
            story.Resume();
        }

        /**
         * Start welcome fade out animation when loaded
         **/
        private void WelcomeLabelLoaded(Object sender, RoutedEventArgs e)
        {
            Storyboard story = (Storyboard)this.FindResource("welcomeStoryBoard");
            story.Begin();
        }
    }
}