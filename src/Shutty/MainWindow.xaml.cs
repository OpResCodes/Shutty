using System;
using System.Globalization;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Shutty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static Random _rnd = new Random();
        private DateTime _started;
        
        public System.Timers.Timer _aTimer = null;
               
        public double RemainingTime
        {
            get { return (double)GetValue(RemainingTimeProperty); }
            set { SetValue(RemainingTimeProperty, value); }
        }

        public static readonly DependencyProperty RemainingTimeProperty =
            DependencyProperty.Register("RemainingTime", typeof(double), typeof(MainWindow), new PropertyMetadata(0d));
               
        public string ShutdownAction
        {
            get { return (string)GetValue(ShutdownActionProperty); }
            set { SetValue(ShutdownActionProperty, value); }
        }

        public static readonly DependencyProperty ShutdownActionProperty =
            DependencyProperty.Register("ShutdownAction", typeof(string), typeof(MainWindow), new PropertyMetadata("None Selected"));

        public double SelectedTime
        {
            get { return (double)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(double), typeof(MainWindow), new PropertyMetadata(0d));
               
        public TimeSpan RemainingTimespan
        {
            get { return (TimeSpan)GetValue(RemainingTimespanProperty); }
            set { SetValue(RemainingTimespanProperty, value); }
        }

        public static readonly DependencyProperty RemainingTimespanProperty =
            DependencyProperty.Register("RemainingTimespan", typeof(TimeSpan), typeof(MainWindow), new PropertyMetadata(TimeSpan.Zero));




        public bool ShowCountDown
        {
            get { return (bool)GetValue(ShowCountDownProperty); }
            set { SetValue(ShowCountDownProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowCountDown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCountDownProperty =
            DependencyProperty.Register("ShowCountDown", typeof(bool), typeof(Window), new PropertyMetadata(false));


        private void TimerButton_Checked(object sender, RoutedEventArgs e)
        {
            ActionButtonPanel.IsEnabled = true;
            var senderbutton = (ButtonBase)sender;
            double minutes = double.Parse(senderbutton.Content.ToString(),_ci);
            SelectedTime = (minutes * 60);
        }

        static CultureInfo _ci = new CultureInfo("en-US");

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            if (SelectedTime > 0d)
            {
                ShutdownAction = btn.Content.ToString();
                StartTimer();
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            var diff = e.SignalTime.Subtract(_started);
            Dispatcher.Invoke(() =>
            {
                if (diff.TotalSeconds >= SelectedTime)
                {
                    StopTimer();
                    if (ShutdownAction.Equals("Shutdown"))
                        ShuttyLogic.ShutDown(true);
                    if (ShutdownAction.Equals("Hibernate"))
                        ShuttyLogic.Hibernate(true);
                    if (ShutdownAction.Equals("Reboot"))
                        ShuttyLogic.Reboot(true);
                }
                else
                {
                    RemainingTime -= (((Timer)sender).Interval / 1000d);
                    RemainingTimespan = RemainingTimespan.Subtract(TimeSpan.FromSeconds(1));
                }
            });
        }

        private void StopTimer()
        {
            _aTimer.Stop();
            _aTimer.Dispose();
            RemainingTime = SelectedTime;
            RemainingTimespan = TimeSpan.FromSeconds(SelectedTime);
            ActionButtonPanel.IsEnabled = true;
            TimeSelection.IsEnabled = true;

            Binding b = new Binding("ActualWidth");
            b.ElementName = "LayoutRoot";
            b.Mode = BindingMode.OneWay;
            b.Converter = new SizeConverter() { RelativeSize = "0.65" };
            TimerSelection.SetBinding(Grid.WidthProperty, b);
            ShowCountDown = false;
        }

        private void StartTimer()
        {
            if (_aTimer != null)
            {
                _aTimer.Stop();
                _aTimer.Dispose();
            }
            RemainingTime = SelectedTime;
            RemainingTimespan = TimeSpan.FromSeconds(SelectedTime);
            ActionButtonPanel.IsEnabled = false;
            TimeSelection.IsEnabled = false;

            BindingOperations.ClearBinding(TimerSelection, Grid.WidthProperty);
            TimerSelection.Width = TimerSelection.ActualWidth;
            ShowCountDown = true;

            _started = DateTime.Now;
            _aTimer = new Timer();
            _aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _aTimer.Interval = 1000;
            _aTimer.Enabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
        }
    }
}
