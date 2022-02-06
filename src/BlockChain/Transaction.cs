// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Transaction.cs" company="Hämmer Electronics">
//   Copyright (c) 2020 All rights reserved.
// </copyright>
// <summary>
//   A class representing a transaction in the block chain.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BlockChain
{
    /// <summary>
    /// A class representing a transaction in the block chain.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Gets or sets the from address.
        /// </summary>
        public string? FromAddress { get; set; }

        /// <summary>
        /// Gets or sets the to address.
        /// </summary>
        public string? ToAddress { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public long Amount { get; set; }
    }
}
