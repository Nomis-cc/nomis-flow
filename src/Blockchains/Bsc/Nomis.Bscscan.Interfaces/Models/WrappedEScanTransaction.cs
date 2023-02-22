// ------------------------------------------------------------------------------------------------------
// <copyright file="WrappedEScanTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using EthScanNet.Lib.Models.ApiResponses.Accounts.Models;

namespace Nomis.Bscscan.Interfaces.Models
{
    /// <inheritdoc cref="EScanTransaction"/>
    public class WrappedEScanTransaction :
        EScanTransaction
    {
        /// <summary>
        /// Initialize <see cref="WrappedEScanTransaction"/>.
        /// </summary>
        /// <param name="transaction"><see cref="EScanTransaction"/>.</param>
        public WrappedEScanTransaction(EScanTransaction transaction)
        {
            Value = transaction.Value;
            BlockHash = transaction.BlockHash;
            BlockNumber = transaction.BlockNumber;
            Confirmations = transaction.Confirmations;
            ContractAddress = transaction.ContractAddress;
            CumulativeGasUsed = transaction.CumulativeGasUsed;
            From = transaction.From;
            Gas = transaction.Gas;
            GasPrice = transaction.GasPrice;
            GasUsed = transaction.GasUsed;
            Hash = transaction.Hash;
            Input = transaction.Input;
            IsError = transaction.IsError;
            Nonce = transaction.Nonce;
            TimeStamp = transaction.TimeStamp;
            TransactionIndex = transaction.TransactionIndex;
            TxreceiptStatus = transaction.TxreceiptStatus;
            To = transaction.To;
        }
    }
}