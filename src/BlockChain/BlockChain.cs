// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockChain.cs" company="Hämmer Electronics">
//   Copyright (c) 2020 All rights reserved.
// </copyright>
// <summary>
//   A class representing the block chain.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlockChain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A class representing the block chain.
    /// </summary>
    public class BlockChain
    {
        /// <summary>
        /// The reward.
        /// </summary>
        private const int Reward = 1;

        /// <summary>
        /// Gets or sets the pending transactions.
        /// </summary>
        public IList<Transaction> PendingTransactions { get; set; } = new List<Transaction>();

        /// <summary>
        /// Gets or sets the blocks.
        /// </summary>
        public IList<Block> Blocks { get; set; } = new List<Block>();

        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        public int Difficulty { get; set; } = 2;

        /// <summary>
        /// Initializes the block chain.
        /// </summary>
        public void InitializeChain()
        {
            this.Blocks = new List<Block>();
            this.AddGenesisBlock();
        }

        /// <summary>
        /// Creates the genesis block.
        /// </summary>
        /// <returns>The genesis <see cref="Block"/>.</returns>
        public Block CreateGenesisBlock()
        {
            var block = new Block
            {
                TimeStamp = DateTime.Now,
                PreviousHash = null,
                Transactions = this.PendingTransactions
            };

            block.Mine(this.Difficulty);
            this.PendingTransactions = new List<Transaction>();
            return block;
        }

        /// <summary>
        /// Adds the genesis <see cref="Block"/>.
        /// </summary>
        public void AddGenesisBlock()
        {
            this.Blocks.Add(this.CreateGenesisBlock());
        }

        /// <summary>
        /// Gets the latest <see cref="Block"/>.
        /// </summary>
        /// <returns>The latest <see cref="Block"/>.</returns>
        public Block GetLatestBlock()
        {
            return this.Blocks[^1];
        }

        /// <summary>
        /// Creates a <see cref="Transaction"/>.
        /// </summary>
        /// <param name="transaction">The <see cref="Transaction"/>.</param>
        public void CreateTransaction(Transaction transaction)
        {
            this.PendingTransactions.Add(transaction);
        }

        /// <summary>
        /// Processes the pending <see cref="Transaction"/>s.
        /// </summary>
        /// <param name="minerAddress">The miner address.</param>
        public void ProcessPendingTransactions(string minerAddress)
        {
            var block = new Block
            {
                TimeStamp = DateTime.Now,
                PreviousHash = this.GetLatestBlock().Hash,
                Transactions = this.PendingTransactions
            };

            this.AddBlock(block);
            this.PendingTransactions = new List<Transaction>();

            var transaction = new Transaction
            {
                FromAddress = null,
                ToAddress = minerAddress,
                Amount = Reward
            };

            this.CreateTransaction(transaction);
        }

        /// <summary>
        /// Adds the <see cref="Block"/>.
        /// </summary>
        /// <param name="block">The <see cref="Block"/>.</param>
        public void AddBlock(Block block)
        {
            var latestBlock = this.GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Mine(this.Difficulty);
            this.Blocks.Add(block);
        }

        /// <summary>
        /// Checks whether the <see cref="BlockChain"/> is valid.
        /// </summary>
        /// <returns>A <see cref="bool"/> value indicating whether the <see cref="BlockChain"/> is valid or not.</returns>
        public bool IsValid()
        {
            for (var i = 1; i < this.Blocks.Count; i++)
            {
                var currentBlock = this.Blocks[i];
                var previousBlock = this.Blocks[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
