using GalaSoft.MvvmLight.Threading;
using MobileCloudHackDay.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileCloudHackDay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public DispatcherTimer Timer { get; set; }

        public MainViewModel Vm { get { return (MainViewModel)DataContext; } }


        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => { Timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(30) }; Timer.Tick += Timer_Tick; Timer.Start(); Timer_Tick(this, null); };

            Action upt = new Action
            (
                () =>
                {
                    
                    Vm.UpdateList(false);
                }

            );

            ((App)App.Current).InitNotificationsAsync(upt);
        }

        private void Timer_Tick(object sender, object e)
        {
            Vm.UpdateList((bool)btAskServiceToUpdate.IsChecked);
            //btAskServiceToUpdate.IsChecked = false;
        }





    }
}
