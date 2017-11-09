using Calculation;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation.Metadata;
using Windows.Storage.Streams;
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

        private async void CustomerSupport_Click(object sender, RoutedEventArgs e)
        {
            // Get the PinnedContactManager for the current user.
            PinnedContactManager pinnedContactManager = PinnedContactManager.GetDefault();

            // Check whether pinning to the taskbar is supported.
            if (!pinnedContactManager.IsPinSurfaceSupported(PinnedContactSurface.Taskbar))
            {
                return;
            }

            // Get the contact list for this app.
            ContactList list = await GetContactListAsync();

            // Check if the sample contact already exists.
            Contact contact = await list.GetContactFromRemoteIdAsync(Constants.ContactRemoteId);


            if (contact == null)
            {
                // Create the sample contact.
                contact = new Contact();
                contact.FirstName = "Clippy";
                contact.LastName = "";
                contact.RemoteId = Constants.ContactRemoteId;
                contact.Emails.Add(new ContactEmail { Address = Constants.ContactEmail });
                contact.Phones.Add(new ContactPhone { Number = Constants.ContactPhone });
                contact.SourceDisplayPicture = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/clippy.jpg"));

                await list.SaveContactAsync(contact);
            }

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5))
            {
                // Create a new contact annotation
                ContactAnnotation annotation = new ContactAnnotation();
                annotation.ContactId = contact.Id;

                // Add appId and contact panel support to the annotation
                String appId = "d9714431-a083-4f7c-a89f-fe7a38f759e4_75cr2b68sm664!App";
                annotation.ProviderProperties.Add("ContactPanelAppID", appId);
                annotation.SupportedOperations = ContactAnnotationOperations.ContactProfile;

                // Save annotation to contact annotation list
                // Windows.ApplicationModel.Contacts.ContactAnnotationList 
                var annotationList = await GetContactAnnotationList();
                await annotationList.TrySaveAnnotationAsync(annotation);
            }

            // Pin the contact to the taskbar.
            if (!await pinnedContactManager.RequestPinContactAsync(contact, PinnedContactSurface.Taskbar))
            {
                // Contact was not pinned.
                return;
            }
        }

        private async Task<ContactAnnotationList> GetContactAnnotationList()
        {
            ContactAnnotationStore annotationStore = await ContactManager.RequestAnnotationStoreAsync(ContactAnnotationStoreAccessType.AppAnnotationsReadWrite);
            if (null == annotationStore)
            {
                return null;
            }

            ContactAnnotationList annotationList;
            IReadOnlyList<ContactAnnotationList> annotationLists = await annotationStore.FindAnnotationListsAsync();
            if (0 == annotationLists.Count)
            {
                annotationList = await annotationStore.CreateAnnotationListAsync();
            }
            else
            {
                annotationList = annotationLists[0];
            }

            return annotationList;
        }


        private async Task<ContactList> GetContactListAsync()
        {
            ContactStore store = await ContactManager.RequestStoreAsync(ContactStoreAccessType.AppContactsReadWrite);

            IReadOnlyList<ContactList> contactLists = await store.FindContactListsAsync();
            ContactList sampleList = contactLists.FirstOrDefault(list => list.DisplayName.Equals(Constants.ContactListName));

            if (sampleList == null)
            {
                sampleList = await store.CreateContactListAsync(Constants.ContactListName);
            }

            return sampleList;
        }
    }

    internal static class Constants
    {
        internal const string ContactRemoteId = "{D44056FA-6C0E-47BE-B984-0974B21FF59D}";
        internal const string ContactListName = "Clippy";
        internal const string ContactEmail = "clippy@microsoft.com";
        internal const string ContactPhone = "4255550123";
    }
}