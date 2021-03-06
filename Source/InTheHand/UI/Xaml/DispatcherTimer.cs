//-----------------------------------------------------------------------
// <copyright file="DispatcherTimer.cs" company="In The Hand Ltd">
//     Copyright � 2015-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
#if __ANDROID__ || __UNIFIED__  || WIN32
using System.Timers;
#endif

#if __IOS__ || __TVOS__  
using UIKit;
#elif __MAC__
using AppKit;
#endif

namespace InTheHand.UI.Xaml
{
    /// <summary>
    /// Provides a timer that is integrated into the Dispatcher queue, which is processed at a specified interval of time.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list></remarks>
    public sealed class DispatcherTimer
    {
#if __ANDROID__ || __UNIFIED__  || WIN32
        private Timer _timer;

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(Tick != null)
            {
#if __IOS__ || __TVOS__
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    Tick?.Invoke(this, null);
                });
#elif __MAC__
                NSApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    Tick?.Invoke(this, null);
                });
#elif __ANDROID__
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
                {
                    Tick?.Invoke(this, null);
                });
#endif
            }
        }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.UI.Xaml.DispatcherTimer _dispatcherTimer;

        private void _dispatcherTimer_Tick(object sender, object e)
        {
           Tick?.Invoke(this, null);
        }

        public static implicit operator Windows.UI.Xaml.DispatcherTimer(DispatcherTimer dispatcherTimer)
        {
            return dispatcherTimer._dispatcherTimer;
        }

#elif WINDOWS_PHONE
        private global::System.Windows.Threading.DispatcherTimer _dispatcherTimer;

        private void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
           Tick?.Invoke(this, null);
        }

        public static implicit operator global::System.Windows.Threading.DispatcherTimer(DispatcherTimer dispatcherTimer)
        {
            return dispatcherTimer._dispatcherTimer;
        }

#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherTimer"/> class. 
        /// </summary>
        public DispatcherTimer()
        {
#if __ANDROID__ || __UNIFIED__  || WIN32
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += _timer_Elapsed;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            _dispatcherTimer = new Windows.UI.Xaml.DispatcherTimer();
            _dispatcherTimer.Tick += _dispatcherTimer_Tick;
#elif WINDOWS_PHONE
            _dispatcherTimer = new global::System.Windows.Threading.DispatcherTimer();
            _dispatcherTimer.Tick += _dispatcherTimer_Tick;
#endif
        }

        /// <summary>
        /// Occurs when the timer interval has elapsed. 
        /// </summary>
        public event EventHandler<object> Tick;

        /// <summary>
        /// Gets or sets the amount of time between timer ticks. 
        /// </summary>
        public TimeSpan Interval
        {
            get
            {
#if __ANDROID__ || __UNIFIED__  || WIN32
                return TimeSpan.FromMilliseconds(_timer.Interval);

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _dispatcherTimer.Interval;

#else
                return TimeSpan.Zero;
#endif
            }

            set
            {
#if __ANDROID__ || __UNIFIED__  || WIN32
                _timer.Interval = value.TotalMilliseconds;

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                _dispatcherTimer.Interval = value;
#endif
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the timer is running. 
        /// </summary>
        /// <value>true if the timer is enabled and running; otherwise, false.</value>
        public bool IsEnabled
        {
            get
            {
#if __ANDROID__ || __UNIFIED__  || WIN32
                return _timer.Enabled;

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _dispatcherTimer.IsEnabled;

#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Starts the <see cref="DispatcherTimer"/>. 
        /// </summary>
        /// <remarks>If the timer has already started, then it is restarted.</remarks>
        public void Start()
        {
#if __ANDROID__ || __UNIFIED__  || WIN32
            if (_timer.Enabled)
            {
                _timer.Stop();
            }

            _timer.Start();

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            _dispatcherTimer.Start();
#endif
        }

        /// <summary>
        /// Stops the <see cref="DispatcherTimer"/>. 
        /// </summary>
        public void Stop()
        {
#if __ANDROID__ || __UNIFIED__ || WIN32
            _timer.Stop();

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            _dispatcherTimer.Stop();
#endif
        }
    }

#if WIN32
    internal interface IDispatcherTimer
    {
        TimeSpan Interval { get; set; }

        bool IsEnabled {get;}

        void Start();

        void Stop();
    }
#endif
}