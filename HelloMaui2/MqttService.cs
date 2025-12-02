using MQTTnet;
using System.Text;

namespace Bills_MQTT_Tester
{
    public class MqttService
    {
        private IMqttClient? _mqttClient;

        public async Task ConnectAsync(string brokerAddress, int port, string username, string password)
        {
            var factory = new MqttClientFactory();
            _mqttClient = factory.CreateMqttClient();

            // Build options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(brokerAddress, port)
                .WithCredentials(username, password)
                .WithCleanSession()
                .Build();

        }
    }
}
