using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlightSimulatorApp.ViewModel;
using System.Windows.Media.Animation;

namespace FlightSimulatorApp.View.Controls
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        private readonly JoystickViewModel vm;
        private bool isPressed;
        private Point pressed;
        private double x;
        private double y;
        private Storyboard story;

        /**
         * The location x property
         **/
        public double X
        {
            get { return this.x; }
            set
            {
                if (this.x != value)
                {
                    //show accuracy of 2 digits after decimal point
                    this.x = (double)System.Math.Round(value, 2);
                }
            }
        }

        /**
         * The location y property
         **/
        public double Y
        {
            get { return this.y; }
            set
            {
                if (this.y != value)
                {
                    //show accuracy of 2 digits after decimal point
                    this.y = (double)System.Math.Round(value, 2);
                }
            }
        }

        /**
         * Constructor
         **/
        public Joystick()
        {
            InitializeComponent();
            DataContext = (Application.Current as App).JoystickViewModel;
            this.vm = (Application.Current as App).JoystickViewModel;
            Initialize();

        }

        /**
         * Add events
         **/
        private void Initialize()
        {
            isPressed = false;
            story = (Storyboard)Knob.FindResource("CenterKnob");
            story.Completed += new EventHandler(StoryComplete);
        }

        /**
         * When the storyboard is completed, stop the storyboard
         **/
        private void StoryComplete(object sender, EventArgs e)
        {
            story.Stop(Knob);
        }

        /**
         * Event for mouse pressed
         **/
        private void KnobBase_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isPressed = true;
            pressed = e.GetPosition(KnobBaseLimits);

            //normalize the knob position according to the mouse pressed location
            knobPosition.X = Utils.Utils.Normalize(pressed.X, -40, 40, 0, 170);
            knobPosition.Y = Utils.Utils.Normalize(pressed.Y, -40, 40, 0, 170);
        }

        /**
         * MouseUp event
         **/
        private void KnobBase_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.isPressed = false;
            pressed = new Point(85, 85);
            story.Begin(Knob, true);
            knobPosition.X = 0;
            knobPosition.Y = 0;
            x = 0;
            y = 0;
            //update the ruuder and elevator property
            vm.VM_Rudder = x;
            vm.VM_Elevator = y;
        }

        /**
         * MouseMove event
         **/
        private void KnobBaseLimits_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPressed)
            {
                //dx, dy- distance from the current mouse location to the last position the mouse was at before movement
                //Normalize valuse according to knob position range
                double dx = Utils.Utils.Normalize(e.GetPosition(KnobBaseLimits).X - pressed.X, -1, 1, -40, 40);
                double dy = Utils.Utils.Normalize(e.GetPosition(KnobBaseLimits).Y - pressed.Y, -1, 1, -40, 40);

                //check boundries
                if (knobPosition.X + dx < -40)
                {
                    dx = 0;
                }
                if (knobPosition.X + dx > 40)
                {
                    dx = 0;
                }
                if (knobPosition.Y + dy < -40)
                {
                    dy = 0;
                }
                if (knobPosition.Y + dy > 40)
                {
                    dy = 0;
                }

                //add the difference to the location
                knobPosition.X += dx;
                knobPosition.Y += dy;
                pressed = new Point(pressed.X + dx, pressed.Y + dy);

                x = Utils.Utils.Normalize(knobPosition.X, -1, 1, -40, 40);
                //update the ruuder property
                vm.VM_Rudder = x;

                y = Utils.Utils.Normalize(knobPosition.Y, -1, 1, -40, 40);
                //update the elevator property
                vm.VM_Elevator = y;
            }
        }
    }
}