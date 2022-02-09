// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hämmer Electronics">
//   Copyright (c) 2020 All rights reserved.
// </copyright>
// <summary>
//   The main program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlockChain;

/// <summary>
/// The main program.
/// </summary>
public class Program
{
    /// <summary>
    /// The WebSocket URL.
    /// </summary>
    private const string WebSocketUrl = "BlockChain";

    /// <summary>
    /// The client.
    /// </summary>
    private static readonly P2PClient Client = new();

    /// <summary>
    /// The server.
    /// </summary>
    private static readonly P2PServer Server = new();

    /// <summary>
    /// The name.
    /// </summary>
    private static string name = "Unknown";

    /// <summary>
    /// Gets or sets the block chain.
    /// </summary>
    public static BlockChain BlockChain { get; set; } = new();

    /// <summary>
    /// Gets or sets the port.
    /// </summary>
    public static int Port { get; set; }

    /// <summary>
    /// The main method.
    /// </summary>
    /// <param name="args">Some arguments.</param>
    public static void Main(string[] args)
    {
        BlockChain.InitializeChain();

        if (args.Length >= 1)
        {
            Port = int.Parse(args[0]);
        }

        if (args.Length >= 2)
        {
            name = args[1];
        }

        if (Port > 0)
        {
            Server.Start();
        }

        if (name != "Unknown")
        {
            Console.WriteLine($"Current user is {name}.");
        }

        var selection = 0;
        while (selection != 4)
        {
            switch (selection)
            {
                case 1:
                    Console.WriteLine("Please enter the server URL.");
                    var serverUrl = Console.ReadLine();
                    Client.Connect($"{serverUrl}/{WebSocketUrl}");
                    break;
                case 2:
                    Console.WriteLine("Please enter the receiver name.");
                    var receiverName = Console.ReadLine();
                    Console.WriteLine("Please enter the amount.");
                    var amount = Console.ReadLine();

                    var transaction = new Transaction
                    {
                        Amount = int.Parse(amount ?? "0"),
                        FromAddress = name,
                        ToAddress = receiverName
                    };

                    BlockChain.CreateTransaction(transaction);
                    BlockChain.ProcessPendingTransactions(name);
                    Client.Broadcast(JsonConvert.SerializeObject(BlockChain));
                    break;
                case 3:
                    Console.WriteLine("Block chain:");
                    Console.WriteLine(JsonConvert.SerializeObject(BlockChain, Formatting.Indented));
                    break;
            }

            WriteOptions();

            var action = Console.ReadLine();
            selection = int.Parse(action ?? "-1");
        }

        Client.Close();
    }

    /// <summary>
    /// Writes the options to the console.
    /// </summary>
    private static void WriteOptions()
    {
        Console.WriteLine("Please select an action:");
        Console.WriteLine("=========================");
        Console.WriteLine("1. Connect to a server.");
        Console.WriteLine("2. Add a transaction.");
        Console.WriteLine("3. Display block chain.");
        Console.WriteLine("4. Exit.");
        Console.WriteLine("=========================");
    }
}
