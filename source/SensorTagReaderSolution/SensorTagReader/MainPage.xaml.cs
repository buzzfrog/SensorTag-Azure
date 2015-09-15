using SensorTagReader.Service;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SensorTagReader
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer eventHubWriterTimer;
        TagReaderService tagreader;
        EventHubService eventHubService;
        int numberOfCallsDoneToEventHub;
        int numberOfFailedCallsToEventHub;

        Windows.Storage.ApplicationDataContainer localSettings;

        public MainPage()
        {
            this.InitializeComponent();
            StatusField.Text = "Please ensure the sensor is connected";

            tagreader = new TagReaderService();

            eventHubWriterTimer = new DispatcherTimer();
            eventHubWriterTimer.Interval = new TimeSpan(0, 0, 1);
            eventHubWriterTimer.Tick += OnEventHubWriterTimerTick;

            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if(ServiceBusNamespaceField.Text == string.Empty)    ServiceBusNamespaceField.Text = Convert.ToString(localSettings.Values["ServiceBusNamespaceField"]);
            if(EventHubNameField.Text == string.Empty)           EventHubNameField.Text = Convert.ToString(localSettings.Values["EventHubNameField"]);
            if(SharedAccessPolicyNameField.Text == string.Empty) SharedAccessPolicyNameField.Text = Convert.ToString(localSettings.Values["SharedAccessPolicyNameField"]);
            if(SharedAccessPolicyKeyField.Text == string.Empty)  SharedAccessPolicyKeyField.Text = Convert.ToString(localSettings.Values["SharedAccessPolicyKeyField"]);
            if(SensorNameField.Text == string.Empty)             SensorNameField.Text = Convert.ToString(localSettings.Values["SensorNameField"]);

            getVersionNumberOfApp();

        }

        private void getVersionNumberOfApp()
        {
            var thisPackage = Windows.ApplicationModel.Package.Current;
            var version = thisPackage.Id.Version;

            VersionField.Text = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void OnEventHubWriterTimerTick(object sender, object e)
        {
            if (tagreader == null || tagreader.CurrentValues == null)
                return;

            txtTemperature.Text = $"{tagreader.CurrentValues.Temperature:N2} C";
            txtHumidity.Text = $"{tagreader.CurrentValues.Humidity:N2} %";

            try
            {
                await eventHubService.SendMessage(new Messages.EventHubSensorMessage()
                {
                    SensorName = SensorNameField.Text,
                    TimeWhenRecorded = DateTime.Now,
                    Temperature = tagreader.CurrentValues.Temperature,
                    Humidity = tagreader.CurrentValues.Humidity
                });
                numberOfCallsDoneToEventHub++;
            }
            catch { numberOfFailedCallsToEventHub++; }

            EventHubInformation.Text = $"Calls: {numberOfCallsDoneToEventHub}, Failed Calls: {numberOfFailedCallsToEventHub}";

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private async void StartCommand_Click(object sender, RoutedEventArgs e)
        {
            if((String)StartCommand.Tag == "STOPPED")
            {
                try
                {
                    await startTracking();
                }
                catch (Exception ex)
                {
                    txtError.Text = ex.Message;
                }
            }
            else
            {
                stopTracking();
            }
        }

        private void stopTracking()
        {
            StartCommand.Content = "Start";
            StartCommand.Tag = "STOPPED";
            eventHubWriterTimer.Stop();
        }

        private async Task startTracking()
        {
            await tagreader.InitializeSensor();
            eventHubService = new EventHubService(ServiceBusNamespaceField.Text,
                EventHubNameField.Text, SharedAccessPolicyNameField.Text, SharedAccessPolicyKeyField.Text);

            StatusField.Text = "The sensor is connected";
            txtError.Text = "";
            eventHubWriterTimer.Start();
            StartCommand.Content = "Stop";
            StartCommand.Tag = "STARTED";
            SensorInformation.Text = await tagreader.GetSensorID();
            numberOfFailedCallsToEventHub = numberOfCallsDoneToEventHub = 0;
            EventHubInformation.Text = $"Calls: {numberOfCallsDoneToEventHub}, Failed Calls: {numberOfFailedCallsToEventHub}";
        }

        private void OnSettingsChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if(textBox != null)
            {
                localSettings.Values[textBox.Name] = textBox.Text;
                stopTracking();
            }
        }
    }
}
