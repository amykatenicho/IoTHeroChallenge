namespace IoTWorkshop
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using GHIElectronics.UWP.Shields;
    using Microsoft.Azure.Devices.Client;

    public sealed partial class MainPage : Page
    {
        private const bool UseMockedSensors = true;
        private Random rnd;
        private FEZHAT hat;
        private DispatcherTimer timer;
        private DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("{device connection string}");

        public MainPage()
        {
            this.InitializeComponent();

            // Initialize FEZ HAT shield
            this.SetupHat();
        }

        private async void SetupHat()
        {
            if (UseMockedSensors)
            {
                this.rnd = new Random();
            }
            else
            {
                this.hat = await FEZHAT.CreateAsync();
            }

            this.timer = new DispatcherTimer();

            this.timer.Interval = TimeSpan.FromMilliseconds(5000);
            this.timer.Tick += this.Timer_Tick;

            this.timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            // Light Sensor
            var light = UseMockedSensors ? rnd.NextDouble() : this.hat.GetLightLevel();

            // Temperature Sensor
            var temp = UseMockedSensors ? rnd.NextDouble() * 50 - 10 : this.hat.GetTemperature();

            // Display values
            this.LightTextBox.Text = light.ToString("P2");
            this.LightProgress.Value = light;
            this.TempTextBox.Text = temp.ToString("N2");
            this.TempProgress.Value = temp;

            // send data to IoT Hub
            var jsonMessage = string.Format("{{ displayname:null, location:\"USA\", organization:\"Fabrikam\", guid: \"41c2e437-6c3d-48d0-8e12-81eab2aa5013\", timecreated: \"{0}\", measurename: \"Temperature\", unitofmeasure: \"C\", value:{1}}}",
                 DateTime.UtcNow.ToString("o"),
                 temp);

            this.SendMessage(jsonMessage);

            jsonMessage = string.Format("{{ displayname:null, location:\"USA\", organization:\"Fabrikam\", guid: \"41c2e437-6c3d-48d0-8e12-81eab2aa5013\", timecreated: \"{0}\", measurename: \"Light\", unitofmeasure: \"L\", value:{1}}}",
                 DateTime.UtcNow.ToString("o"),
                 light);

            this.SendMessage(jsonMessage);
        }

        public async void SendMessage(string message)
        {
            // Send message to an IoT Hub using IoT Hub SDK
            try
            {
                var content = new Message(Encoding.UTF8.GetBytes(message));
                await deviceClient.SendEventAsync(content);

                Debug.WriteLine("Message Sent: {0}", message, null);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception when sending message:" + e.Message);
            }
        }
    }
}
