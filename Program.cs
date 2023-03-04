using Azure.Messaging.ServiceBus;

public class Program
{
    private static ServiceBusClient _client;

    private static T GetArgument<T>(string[] args, int idx, T defaultValue, Func<string, T> parser)
    {
        if (args.Length <= idx)
            return defaultValue;

        string val = args[idx];
        return parser(val);
    }

    private static async Task RunSendCommand(string[] args)
    {
        var sender = _client.CreateSender("test");

        int count = GetArgument(args, 0, 10, x => int.Parse(x));
        TimeSpan period = GetArgument(args, 1, TimeSpan.FromSeconds(1), x => TimeSpan.FromSeconds(double.Parse(x)));

        Log(string.Format("sending {0} messages with period {1} seconds...", count, period.TotalSeconds));

        for (int i = 0; i < count; i++)
        {
            await sender.SendMessageAsync(new ServiceBusMessage(DateTime.Now.ToString("HH:mm:ss.fff")));
            Log("sent!");
            await Task.Delay(period);
        }
    }
    
    private static async Task RunReceiveCommand(string[] args)
    {
        var receiver = _client.CreateReceiver("test");
        int count = GetArgument(args, 0, 10, x => int.Parse(x));

        Log(string.Format("receiving {0} messages...", count));

        for (int i = 0; i < count; i++)
        {
            var message = await receiver.ReceiveMessageAsync();

            if (message == null)
            {
                Log("message not received...");
                continue;
            }

            await receiver.CompleteMessageAsync(message);

            Log($"received!: {message.Body.ToString()}");
        }
    }

    public static void Log(string message)
    {
        Console.WriteLine("[{0:HH:mm:ss.fff}] {1}", DateTime.Now, message);
    }

    public static async Task Main(string[] args)
    {
        // _client = new ServiceBusClient("Endpoint=sb://egubenkov.servicebus.windows.net/;SharedAccessKeyName=app;SharedAccessKey=vp7n15+VO7O0ICkzGq5KUYyYfuVlNX2kL+ASbM+cHtk=");
        _client = new ServiceBusClient("Endpoint=sb://egubenkov.servicebus.windows.net/;SharedAccessKeyName=app;SharedAccessKey=vp7n15+VO7O0ICkzGq5KUYyYfuVlNX2kL+ASbM+cHtk=");

        Log("Hello!");

        bool continueServing = true;

        string cmd = "";
        string[] cmdArgs = new string[]{};

        while (continueServing)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                string[] allArgs = input.Split(' ');

                cmd = allArgs[0];
                cmdArgs = allArgs.Skip(1).ToArray();
            }

            switch (cmd)
            {
                case "send":
                    await RunSendCommand(cmdArgs);
                    break;
                case "receive":
                    var receiver = _client.CreateReceiver("test");
                    await RunReceiveCommand(cmdArgs);
                    break;
                case "exit":
                    continueServing = false;
                    break;
                default:
                    Log("unknown command!");
                    break;
            }
        }
    }
}
