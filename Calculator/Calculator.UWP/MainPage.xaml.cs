using Calculation;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Input.Inking;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Calculator.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<string> historyCollection;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPage()
        {
            this.InitializeComponent();
            HistoryCollection = new ObservableCollection<string>(CalculationHistory.FetchEntireHistory());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().GetAnimation("chartLogo")?.TryStart(ChartButton);
        }

        /// <summary>
        /// Update the UI to show what will be calculated.
        /// </summary>
        private void CalculatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsTextBlock.Text == "0" | ResultsTextBlock.Text == "/" | ResultsTextBlock.Text == "+" | ResultsTextBlock.Text == "-" | ResultsTextBlock.Text == "x")
            {
                ResultsTextBlock.Text = string.Empty;
            }

            ResultsTextBlock.Text += (sender as Button)?.Content;
        }

        /// <summary>
        /// Do some amazing calculations. Store calculation and result in SQL.
        /// </summary>
        private async void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyInkCanvas.InkPresenter.StrokeContainer.GetStrokes().Any())
            {
                var inkRecognizerContainer = new InkRecognizerContainer();
                var results = await inkRecognizerContainer.RecognizeAsync(MyInkCanvas.InkPresenter.StrokeContainer, InkRecognitionTarget.All);
                var recognizedText = string.Concat(results.Select(i => i.GetTextCandidates()[0]));

                ResultsTextBlock.Text = Calculation.NetStandard.Calculator.Calculate(recognizedText).ToString();

                MyInkCanvas.InkPresenter.StrokeContainer.Clear();
            }
            else
            {
                ResultsTextBlock.Text = Calculation.NetStandard.Calculator.Calculate(ResultsTextBlock.Text).ToString();
            }

            HistoryCollection = new ObservableCollection<string>(CalculationHistory.FetchEntireHistory());
        }

        /// <summary>
        /// Fetch history item from SQL database.
        /// </summary>
        private void HistoryItem_Click(object sender, ItemClickEventArgs e)
        {
            ResultsTextBlock.Text = CalculationHistory.FetchFromHistory(e.ClickedItem.ToString()).ToString();
        }

        /// <summary>
        /// Collection of all the history in the database.
        /// </summary>
        public ObservableCollection<string> HistoryCollection
        {
            get { return historyCollection; }
            set
            {
                historyCollection = value;
                if (historyCollection.Count == 10)
                {
                    PopToast();
                }

                RaiseProperytChanged();
            }
        }

        /// <summary>
        /// Clears the screen.
        /// </summary>
        private void CButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryCollection = new ObservableCollection<string>(CalculationHistory.FetchEntireHistory());
        }

        /// <summary>
        /// Clears the screen.
        /// </summary>
        private void CEButton_Click(object sender, RoutedEventArgs e)
        {
            ResultsTextBlock.Text = string.Empty;
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaiseProperytChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            CalculationHistory.ClearHistory();
            HistoryCollection = new ObservableCollection<string>();
        }

        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("chartLogo", ChartButton);
            Frame.Navigate(typeof(Chart), HistoryCollection);
        }

        private void PopToast()
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "10 item badge!"
                            },
                            new AdaptiveText()
                            {
                                Text = "Congratulations!! You've got 10 items in your history."
                            }
                        },
                        Attribution = new ToastGenericAttributionText()
                        {
                            Text = "The UWP Calculator"
                        }
                    }
                },
                Launch = "action=viewStory&storyId=92187"
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }
    }
}