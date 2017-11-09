using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Calculator.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Chart : Page
    {
        public Chart()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().GetAnimation("chartLogo").TryStart(ChartButton);
            CalculateUserData(e.Parameter as ObservableCollection<string>);
        }

        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("chartLogo", ChartButton);
            Frame.GoBack();
        }

        private void CalculateUserData(ObservableCollection<string> historicalData)
        {
            var userData = new ObservableCollection<ChartItem>();
            var random = new Random();

            foreach (var item in historicalData)
            {
                userData.Add(new ChartItem(item, random.Next(0, 100)));
            }

            this.radChart.DataContext = userData;
        }
    }

    public class ChartItem
    {
        public ChartItem(string key, int value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public int Value { get; set; }
    }

}
