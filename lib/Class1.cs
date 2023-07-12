using Microsoft.VisualBasic;
using uPLibrary.Networking.M2Mqtt;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;

namespace lib;

public class MQTT
{
    private static string brokerHost;
    private static string mqttUsername;
    private static string publishTopic = $"MqttChat/{mqttUsername}";
    private static string subscribeTopic = "MqttChat/#";
    private static int brokerPort;
    private static MqttClient mqttClient;

    public static void opprettMqtt(string username)
      {
        // MQTT-brokerinnstillinger
        brokerHost = "broker.hivemq.com";
        brokerPort = 1883;
        mqttUsername = username;

        // Definer MQTT-emne for sending og mottak
        publishTopic = $"MqttChat/{mqttUsername}";
        subscribeTopic = "MqttChat/#";
      }

    public static void connectAndSubscribe()
     {
      // Opprett en MQTT-klient
      mqttClient = new MqttClient(brokerHost);

      // Koble til MQTT-brokeren
      mqttClient.Connect(Guid.NewGuid().ToString());
    
      // Funksjon som kalles når en melding mottas
      mqttClient.MqttMsgPublishReceived += (s, e) =>
      {
        // Mottatt melding
        var receivedMessage = Encoding.UTF8.GetString(e.Message);
        Console.WriteLine("\nMottatt melding: " + receivedMessage);
      };

      // Abonner på MQTT-emnet for å motta meldinger
      mqttClient.Subscribe(new[] { subscribeTopic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
     }

    public static void disconnect()
     {
      // Avslutt MQTT-klienten
      mqttClient.Disconnect();
     }

      public static void sendMsg(string melding)
     {
        // Definer melding og konverter til JSON
        var message = new
        {
          from = mqttUsername,
          content = melding
        };
        var jsonMessage = JsonConvert.SerializeObject(message);

        // Publiser meldingen på MQTT-emnet
        mqttClient.Publish(publishTopic, Encoding.UTF8.GetBytes(jsonMessage), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
     }
}
