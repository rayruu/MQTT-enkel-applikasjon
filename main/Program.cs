// See https://aka.ms/new-console-template for more information
using lib;

Console.WriteLine("Program startet");
if (args.Length>0)
{
    string username = args[0];
    MQTT.opprettMqtt(username);
}
else
{
     Console.WriteLine("Vennligst skriv inn brukernavnet ditt: ");
     string username = Console.ReadLine();
     MQTT.opprettMqtt(username);
}

//Koble til og abonner på MQTT-meldinger i en egen tråd
Thread mqttThread = new Thread(MQTT.connectAndSubscribe);
mqttThread.Start();

bool shouldContinue = true;

while (shouldContinue)
{
    Console.Write("Skriv inn melding (skriv 'exit' for å avslutte): ");
    string melding = Console.ReadLine();

    if (melding == "exit")
    {
        shouldContinue =false;
        MQTT.disconnect();
    }
    else
    {
        MQTT.sendMsg(melding);
    }
}

// Hold applikasjonen kjørende.
Console.ReadLine();
