// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Block.cs" company="Haemmer Electronics">
//   Copyright (c) 2020 All rights reserved.
// </copyright>
// <summary>
//   A class representing a block in the block chain.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlockChain
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    using Newtonsoft.Json;

    /// <summary>
    /// A class representing a block in the block chain.
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the previous hash.
        /// </summary>
        public string PreviousHash { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        public IList<Transaction> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the nonce.
        /// </summary>
        public int Nonce { get; set; }

        /// <summary>
        /// Calculates the hash.
        /// </summary>
        /// <returns>The hash as BASE64 <see cref="string"/>.</returns>
        public string CalculateHash()
        {
            var sha = SHA512.Create();
            var inputBytes = Encoding.ASCII.GetBytes($"{this.TimeStamp}-{this.PreviousHash ?? ""}-{JsonConvert.SerializeObject(this.Transactions)}-{this.Nonce}");
            var outputBytes = sha.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }

        /// <summary>
        /// Mines the block.
        /// </summary>
        /// <param name="difficulty">The difficulty.</param>
        public void Mine(int difficulty)
        {
            var leadingZeros = new string('0', difficulty);

            while (this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros)
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
        }
    }
}
