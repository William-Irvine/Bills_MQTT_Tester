using MQTTnet;

namespace Bills_MQTT_Tester
{
    public partial class MainPage : ContentPage
    {
        private IMqttClient mqttClient;

        int count = 0;
        string broker = "";
        string user = "";
        string pass = "";

        public MainPage()
        {
            InitializeComponent();
        }

        public string GetBroker()
        {
            return broker;
        }

        public string GetUsername()
        {
            return user;
        }

        public string GetPassword()
        {
            return pass;
        }

        void SetBroker(string brkr)
        {
            broker = brkr;
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            broker = brokerAddressEntry.Text;
            user = usernameEntry.Text;
            pass = passwordEntry.Text;

            string brand = brandEntry.Text;
            string model = modelEntry.Text;
            string year = yearEntry.Text;

            if (string.IsNullOrEmpty(broker) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(year))
            {
                await DisplayAlert("Oops!", "Required fields have not been filled out. \nPlease review and try again.", "OK");
            }
            else
            {
                //Save or use these values
                await DisplayAlert("Connecting", $"Attempting to connect to Broker: {broker}", "OK");

                try
                {
                    var mqttFactory = new MqttClientFactory();

                    //Create new MQTT Client Istance
                    mqttClient = mqttFactory.CreateMqttClient();

                    //Configure MQTT client options
                    var options = new MqttClientOptionsBuilder()
                        .WithTcpServer(broker, 1883)
                        .WithClientId("Bills MQTT Tester")
                        .WithCredentials(user, pass)
                        .WithCleanSession()
                        .Build();
                    Console.WriteLine($"1 IS_CLIENT_CONNECTED: {mqttClient.IsConnected}");
                    if(!mqttClient.IsConnected)
                    {
                        // This will throw an exception if the server is not available.
                        // The result from this message returns additional data which was sent
                        // from the server. Please refer to the MQTT protocol specification for details.
                        var response = await mqttClient.ConnectAsync(options, CancellationToken.None);
                        Console.WriteLine("The MQTT client is connected.");

                        string json = $"{{\"brand\":\"{brand}\",\"model\":\"{model}\",\"year\":{year}}}";

                        Console.WriteLine(json);

                        var applicationMessage = new MqttApplicationMessageBuilder()
                           .WithTopic("data/car")
                           .WithPayload(json)
                           .Build();

                        await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);


                        // Send a clean disconnect to the server by calling _DisconnectAsync_. Without this the TCP connection
                        // gets dropped and the server will handle this as a non clean disconnect (see MQTT spec for details).
                        var mqttClientDisconnectOptions = mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
                        await mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);
                        Console.WriteLine("The MQTT client is diconnected.");
                    }
                } 
                catch (Exception ex)
                {
                    Console.WriteLine($"Error connecting: {ex.Message}");
                    Console.WriteLine("Connection failed.");
                }

            }
        }

        /*private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }*/

    }

}
