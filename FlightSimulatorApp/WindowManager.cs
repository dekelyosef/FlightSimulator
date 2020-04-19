using System;
using FlightSimulatorApp.Model;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace FlightSimulatorApp
{

    /**
    * Manage all application windows
    **/
    class WindowManager
    {
        private static WindowManager windManager = null;
        private MainWindow main;
        private OpenWindow menu;
        private readonly System.Timers.Timer timer;

        private readonly TutorialWindow tutorial;
        private ServerModel model;

        public bool InvokeRequired { get; private set; }

        /**
         * Constructor
         * Singleton DP
         **/
        private WindowManager()
        {
            main = (Application.Current as App).MainWind;
            menu = (Application.Current as App).OpenWind;
            tutorial = (Application.Current as App).TutorialWind;
            model = (Application.Current as App).TheModel;
            InitEvents();
            timer = new System.Timers.Timer(4000);
        }


        /**
         * Subscribe events
         **/
        private void InitEvents()
        {
            menu.UserMenu.CloseApp += new EventHandler(CloseApp);
            menu.UserMenu.ConnectApp += new EventHandler(ConnectApp);
            menu.UserMenu.OpenTutorial += new EventHandler(ShowTutorial);
            main.Back += new EventHandler(BackToMenu);
            menu.UserMenu.OpenTutorial += new EventHandler(ShowTutorial);
            tutorial.Back += new EventHandler(BackFromTutorial);
            model.LostConnection += new EventHandler(StartLostConnection);
        }

        /**
         * Start application showing the menu
         * */
        public void StartApp()
        {
            main.Hide();
            menu.Show();
        }

        /**
         * Connect event- switch to main application window
         **/
        private void ConnectApp(object sender, EventArgs e)
        {
            menu.Hide();
            main.Show();
        }

        /**
         * When connection is lost, count 4 seconds to let the user 
         * see the notification.
         * Activate BackToMenu when time passed
         **/
        private void StartLostConnection(object sender, EventArgs e)
        {
            timer.Elapsed += BackToMenu;
            timer.Start();
        }

        /**
         * Close application- close all windows and shut down
         **/
        private void CloseApp(Object sender, EventArgs e)
        {
            tutorial.Close();
            menu.Close();
            main.Close();
            System.Windows.Application.Current.Shutdown();
        }

        /**
         * Switch back to menu window when the user is done
         * with the controls tutorial
         **/
        private void BackFromTutorial(object sender, EventArgs e)
        {
            tutorial.Hide();
            menu.Show();
        }

        /**
         * When app disconnects from simulator, close all windows and reboot app classes 
         * Go back to menu
         **/
        private void BackToMenu(object sender, EventArgs e)
        {
            timer.Stop();
            Application.Current.Dispatcher.Invoke(new Action(() => {
                main.Hide();
                (Application.Current as App).DisposeAll();
                main.Close();
                menu.Hide();

                (Application.Current as App).Bootstrap(false);
                GC.Collect();
                main = (Application.Current as App).MainWind;
                menu = (Application.Current as App).OpenWind;
                model = (Application.Current as App).TheModel;
                InitEvents();
                menu.Show();

            }));
        }

        /**
         * Show the controls tutorial window and start animation
         **/
        private void ShowTutorial(object sender, EventArgs e)
        {
            menu.Hide();
            tutorial.Show();
            tutorial.story.Seek(TimeSpan.Zero);
            tutorial.story.Begin();
        }

        /**
         * Get WindowManager
         **/
        public static WindowManager GetInstance()
        {
            if (windManager == null)
            {
                windManager = new WindowManager();
            }
            return windManager;
        }
    }
}