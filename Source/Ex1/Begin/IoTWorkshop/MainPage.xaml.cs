namespace IoTWorkshop
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using GHIElectronics.UWP.Shields;

    public sealed partial class MainPage : Page
    {
        private const bool UseMockedSensors = true;
        private Random rnd;
        private FEZHAT hat;
        private DispatcherTimer timer;

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
            
            // TODO: add code to send data to IoT Hub
        }
    }
}
