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
using Calculation.NetStandard;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Calculator.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyPeople : Page, INotifyPropertyChanged
    {
        public MyPeople()
        {
            this.InitializeComponent();
        }

        private ObservableCollection<string> _conversations = new ObservableCollection<string>() { "Clippy: I see you are trying to perform a calculation!" };

        public ObservableCollection<string> Conversations
        {
            get { return _conversations; }
            set
            {
                _conversations = value;
                RaiseProperytChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaiseProperytChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            this.Conversations.Add($"Me: {this.CalculationTextBox.Text}");
            this.Conversations.Add($"Clippy: {Calculation.NetStandard.Calculator.Calculate(this.CalculationTextBox.Text).ToString()}");
        }
    }
}
