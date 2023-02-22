// ------------------------------------------------------------------------------------------------------
// <copyright file="WavesExplorerTransactionType.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

namespace Nomis.WavesExplorer.Interfaces.Enums
{
    /// <summary>
    /// Waves Explorer transaction type.
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.waves.tech/en/blockchain/transaction-type/#tokenization"/>
    /// </remarks>
    public enum WavesExplorerTransactionType :
        byte
    {
        /// <summary>
        /// Issues a token.
        /// </summary>
        IssueTransaction = 3,

        /// <summary>
        /// Transfers a token to another account.
        /// </summary>
        TransferTransaction = 4,

        /// <summary>
        /// Reissues a token.
        /// </summary>
        ReissueTransaction = 5,

        /// <summary>
        /// Decreases the amount of token.
        /// </summary>
        BurnTransaction = 6,

        /// <summary>
        /// Exchanges two different tokens between two accounts. Contains two counter orders: a buy order and a sell order.
        /// </summary>
        ExchangeTransaction = 7,

        /// <summary>
        /// Creates alias for the sender's address.
        /// </summary>
        CreateAliasTransaction = 10,

        /// <summary>
        /// Transfers a token, up to 100 recipients.
        /// </summary>
        MassTransferTransaction = 11,

        /// <summary>
        /// Adds, modifies and deletes data entries in the sender's account data storage.
        /// </summary>
        DataTransaction = 12,

        /// <summary>
        /// Assigns the dApp script or account script to the sender's account.
        /// </summary>
        SetScriptTransaction = 13,

        /// <summary>
        /// Modifies the asset script.
        /// </summary>
        SetAssetScriptTransaction = 15,

        /// <summary>
        /// Invokes a callable function of a dApp.
        /// </summary>
        InvokeScriptTransaction = 16,

        /// <summary>
        /// Changes the token name and description.
        /// </summary>
        UpdateAssetInfoTransaction = 17,

        /// <summary>
        /// Invokes a dApp script or transfers a token on behalf of a MetaMask user.
        /// </summary>
        EthereumLikeTransaction = 18
    }
}