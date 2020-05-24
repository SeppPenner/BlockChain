// --------------------------------------------------------------------------------------------------------------------
// <copyright file="P2PServer.cs" company="Haemmer Electronics">
//   Copyright (c) 2020 All rights reserved.
// </copyright>
// <summary>
//   A class representing the P2P server.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlockChain
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using WebSocketSharp;
    using WebSocketSharp.Server;

    /// <summary>
    /// A class representing the P2P server.
    /// </summary>
    public class P2PServer : WebSocketBehavior
    {
        /// <summary>
        /// The WebSocket URL.
        /// </summary>
        private const string WebSocketUrl = "BlockChain";

        /// <summary>
        /// The WebSocket address.
        /// </summary>
        private const string WebSocketAddress = "ws://127.0.0.1";

        /// <summary>
        /// A value indicating whether the chain is synced or not.
        /// </summary>
        private bool chainSynched;

        /// <summary>
        /// The WebSocket server.
        /// </summary>
        private WebSocketServer webSocketServer;

        /// <summary>
        /// Starts the P2P server.
        /// </summary>
        public void Start()
        {
            this.webSocketServer = new WebSocketServer($"{WebSocketAddress}:{Program.Port}.");
            this.webSocketServer.AddWebSocketService<P2PServer>($"/{WebSocketUrl}");
            this.webSocketServer.Start();
            Console.WriteLine($"Started server at {WebSocketAddress}:{Program.Port}.");
        }

        /// <summary>
        /// Handles the message event.
        /// </summary>
        /// <param name="e">The message event args.</param>
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi server.")
            {
                Console.WriteLine(e.Data);
                this.Send("Hi client.");
            }
            else
            {
                var newChain = JsonConvert.DeserializeObject<BlockChain>(e.Data);

                if (newChain.IsValid() && newChain.Blocks.Count > Program.BlockChain.Blocks.Count)
                {
                    var newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(Program.BlockChain.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    Program.BlockChain = newChain;
                }

                if (this.chainSynched)
                {
                    return;
                }

                this.Send(JsonConvert.SerializeObject(Program.BlockChain));
                this.chainSynched = true;
            }
        }
    }
}
