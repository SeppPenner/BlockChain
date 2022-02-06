// --------------------------------------------------------------------------------------------------------------------
// <copyright file="P2PClient.cs" company="Hämmer Electronics">
//   Copyright (c) 2020 All rights reserved.
// </copyright>
// <summary>
//   A class representing the P2P client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlockChain
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using WebSocketSharp;

    /// <summary>
    /// A class representing the P2P client.
    /// </summary>
    public class P2PClient
    {
        /// <summary>
        /// The WebSocket dictionary.
        /// </summary>
        private readonly IDictionary<string, WebSocket> webSocketDictionary = new Dictionary<string, WebSocket>();

        /// <summary>
        /// Connects the P2P client to the server.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void Connect(string url)
        {
            if (this.webSocketDictionary.ContainsKey(url))
            {
                return;
            }

            var ws = new WebSocket(url);
            ws.OnMessage += (sender, e) => 
            {
                if (e.Data == "Hi client.")
                {
                    Console.WriteLine(e.Data);
                }
                else
                {
                    var newChain = JsonConvert.DeserializeObject<BlockChain?>(e.Data);

                    if (newChain is null)
                    {
                        return;
                    }

                    if (!newChain.IsValid() || newChain.Blocks.Count <= Program.BlockChain.Blocks.Count)
                    {
                        return;
                    }

                    var newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(Program.BlockChain.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    Program.BlockChain = newChain;
                }
            };

            ws.Connect();
            ws.Send("Hi server.");
            ws.Send(JsonConvert.SerializeObject(Program.BlockChain));
            this.webSocketDictionary.Add(url, ws);
        }

        /// <summary>
        /// Broadcasts the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Broadcast(string data)
        {
            foreach (var item in this.webSocketDictionary)
            {
                item.Value.Send(data);
            }
        }

        /// <summary>
        /// Closes the P2P client.
        /// </summary>
        public void Close()
        {
            foreach (var item in this.webSocketDictionary)
            {
                item.Value.Close();
            }
        }
    }
}
