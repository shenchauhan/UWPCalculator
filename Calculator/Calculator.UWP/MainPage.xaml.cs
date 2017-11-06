using Calculation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Calculator.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {

        private double result;
        private string operation;

        private readonly CalculationHistory calculationHistory = new CalculationHistory();

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPage()
        {
            this.InitializeComponent();
            calculationHistory.ClearHistory();
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResultsTextBlock.Text == "0" | ResultsTextBlock.Text == "/" | ResultsTextBlock.Text == "+" | ResultsTextBlock.Text == "-" | ResultsTextBlock.Text == "x")
            {
                ResultsTextBlock.Text = string.Empty;
            }

            ResultsTextBlock.Text += (sender as Button)?.Content;
        }

        private void Calculate()
        {
            var x = double.Parse(ResultsTextBlock.Text);

            switch (operation)
            {
                case "/":
                    result /= x;
                    break;
                case "+":
                    result += x;
                    break;
                case "-":
                    result -= x;
                    break;
                case "x":
                    result *= x;
                    break;
                default:
                    result = x;
                    break;
            }
        }

        private void DivideButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
            operation = "/";
            calculationHistory.AddToCalculation(ResultsTextBlock.Text + operation);
            ResultsTextBlock.Text = operation;
        }

        private void MultiplyButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
            operation = "x";
            calculationHistory.AddToCalculation(ResultsTextBlock.Text + operation);
            ResultsTextBlock.Text = operation;
        }

        private void SubtractButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
            operation = "-";
            calculationHistory.AddToCalculation(ResultsTextBlock.Text + operation);
            ResultsTextBlock.Text = operation;
        }

        private void AdditionButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
            operation = "+";
            calculationHistory.AddToCalculation(ResultsTextBlock.Text + operation);
            ResultsTextBlock.Text = operation;
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
            calculationHistory.AddToCalculation(ResultsTextBlock.Text);
            ResultsTextBlock.Text = result.ToString();
            calculationHistory.AddToHistory(result);
           
            HistoryCollection = new ObservableCollection<string>(calculationHistory.FetchEntireHistory());

            result = 0;
            operation = string.Empty;
        }

        private ObservableCollection<string> historyCollection;

        public ObservableCollection<string> HistoryCollection
        {
            get { return historyCollection; }
            set
            {
                historyCollection = value;
                RaiseProperytChanged();
            }
        }

        private void RaiseProperytChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CButton_Click(object sender, RoutedEventArgs e)
        {
            ResultsTextBlock.Text = string.Empty;
        }

        private void CEButton_Click(object sender, RoutedEventArgs e)
        {
            result = 0;
            ResultsTextBlock.Text = "0";
            operation = string.Empty;
            calculationHistory.Clear();
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HistoryItem_Click(object sender, ItemClickEventArgs e)
        {
            ResultsTextBlock.Text = calculationHistory.FetchFromHistory(e.ClickedItem.ToString()).ToString();
        }
    }
}
