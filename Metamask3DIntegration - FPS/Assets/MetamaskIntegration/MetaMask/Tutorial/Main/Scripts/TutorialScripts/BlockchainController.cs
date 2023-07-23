using System;
using System.Numerics;
using MetaMask.Models;
using Nethereum.Contracts;
using Nethereum.Util;
using UnityEngine;


namespace MetaMask.Unity.Tutorial
{
    public class BlockchainController : MonoBehaviour
    {
        private Contract contract;

        [SerializeField] private EthRequestsStorageSO _ethRequestsStorageSo;
        [SerializeField] private ContractDataSO _contractDataSo;
        
        #region Events
        public event EventHandler OnWalletConnected;
        public event EventHandler OnWalletDisconnected;
        public event EventHandler OnWalletReady;
        public event EventHandler OnWalletPaused;
        public event EventHandler OnAddEthereumChain;
        public event EventHandler OnSwitchChainId;
        public event EventHandler OnSignSend;
        public event EventHandler OnTransactionSent;
        public event EventHandler OnSignedReady;
        public event EventHandler<string> OnBalanceReceived;
        public event EventHandler<string> OnTransactionCountResult;
        public event EventHandler OnSendTransactionResult;
        public event EventHandler<MetaMaskEthereumRequestResultEventArgs> OnTransactionResult;
        public event EventHandler<MetaMaskEthereumRequestFailedEventArgs> OnEthRequestFailed;

        #endregion

        #region Fields
        [SerializeField] protected MetaMaskConfig config;
        #endregion
        
        private void Awake()
        {
            MetaMaskUnity.Instance.Initialize();
            InitializeEvents();
        }
        
        private void Start() => GetContract();
        private void OnDisable() => UnsubscribeEvents();
        private void InitializeEvents()
        {
            MetaMaskUnity.Instance.Wallet.WalletAuthorized += WalletConnected;
            MetaMaskUnity.Instance.Wallet.WalletDisconnected += WalletDisconnected;
            MetaMaskUnity.Instance.Wallet.WalletReady += WalletReady;
            MetaMaskUnity.Instance.Wallet.WalletPaused += WalletPaused;
            MetaMaskUnity.Instance.Wallet.EthereumRequestResultReceived += TransactionResult;  
            MetaMaskUnity.Instance.Wallet.EthereumRequestFailed += OnErrorMessageEthRequest;
            MetaMaskUnity.Instance.Wallet.EthereumRequestFailed += OnErrorMessageEthRequest;
        }
        private void UnsubscribeEvents()
        {
            MetaMaskUnity.Instance.Wallet.WalletAuthorized -= WalletConnected;
            MetaMaskUnity.Instance.Wallet.WalletDisconnected -= WalletDisconnected;
            MetaMaskUnity.Instance.Wallet.WalletReady -= WalletReady;
            MetaMaskUnity.Instance.Wallet.WalletPaused -= WalletPaused;
            MetaMaskUnity.Instance.Wallet.EthereumRequestResultReceived -= TransactionResult;
            MetaMaskUnity.Instance.Wallet.EthereumRequestFailed -= OnErrorMessageEthRequest;
        }


        #region Event Handlers
        private void TransactionResult(object sender, MetaMaskEthereumRequestResultEventArgs e)
        {
            OnTransactionResult?.Invoke(sender, e);
        }

        private void WalletConnected(object sender, EventArgs e)
        {
            OnWalletConnected?.Invoke(this, EventArgs.Empty);
        }

        private void WalletDisconnected(object sender, EventArgs e)
        {
            OnWalletDisconnected?.Invoke(this, EventArgs.Empty);
        }

        private void WalletReady(object sender, EventArgs e)
        {
            OnWalletReady?.Invoke(this, EventArgs.Empty);
        }

        private void WalletPaused(object sender, EventArgs e)
        {
            OnWalletPaused?.Invoke(this, EventArgs.Empty);
        }
        
        private void OnErrorMessageEthRequest(object sender, MetaMaskEthereumRequestFailedEventArgs e)
        {
            OnEthRequestFailed?.Invoke(sender, e);
        }

        #endregion
        public void Connect() => MetaMaskUnity.Instance.Wallet.Connect();

        [ContextMenu("GetContract")]
        private void GetContract()
        {
            var web3 = new Nethereum.Web3.Web3("http://localhost:8545");
            contract = web3.Eth.GetContract(_contractDataSo.Abi, _contractDataSo.AddressManager);
        }

        private void GetBuyData(string collectionAddress, int tokenId)
        {
            var buy = contract.GetFunction("buy");
            var param1 = collectionAddress;
            var param2 = tokenId;
            var functionInput = buy.CreateCallInput(param1, param2);
            var data = functionInput.Data;
            _contractDataSo.SetContractHash = data;
        }
        
        [ContextMenu(nameof(AddEthereumChain))]
        public async void AddEthereumChain()
        {
            var addEthereumDetails = new AddEthereumChain()
            {
                ChainId = _contractDataSo.NetworkID,
                ChainName = _contractDataSo.ChainName,
                NativeCurrency = new NativeCurrency()
                {
                    Decimals = 18,
                    Name = "Base BNB",
                    Symbol = "BNB",
                },
                RpcUrls = _contractDataSo.RpcUrls
            };

            var request = new MetaMaskEthereumRequest
            {
                Method = _ethRequestsStorageSo.AddEthereumChain,
                Parameters = new AddEthereumChain[] {addEthereumDetails}
            };
            var result = await MetaMaskUnity.Instance.Wallet.Request(request);
            OnAddEthereumChain?.Invoke(this, EventArgs.Empty);
        }

        public async void SwitchChainID()
        {
            var chainSwitchDetails = new SwitchChain()
            {
                ChainId = _contractDataSo.NetworkID
            };
            
            var request = new MetaMaskEthereumRequest
            {
                Method = _ethRequestsStorageSo.SwitchChain,
                Parameters = new SwitchChain[] {chainSwitchDetails}
            };
            OnSwitchChainId?.Invoke(this, EventArgs.Empty);
            await MetaMaskUnity.Instance.Wallet.Request(request);
        }
        
        [ContextMenu(nameof(Sign))]
        public async void Sign()
        {
            //SwitchChainID();
            string msgParams = @"{
                                          ""types"": {
                                            ""EIP712Domain"": [
                                              { ""name"": ""name"", ""type"": ""string"" },
                                              { ""name"": ""version"", ""type"": ""string"" },
                                              { ""name"": ""chainId"", ""type"": ""uint256"" },
                                              { ""name"": ""verifyingContract"", ""type"": ""address"" }
                                            ],
                                            ""WalletConnection"": [
                                              { ""name"": ""User"", ""type"": ""address"" },
                                              { ""name"": ""AppName"", ""type"": ""string"" },
                                              { ""name"": ""Message"", ""type"": ""uint256"" },
                                              { ""name"": ""Timestamp"", ""type"": ""uint256"" }
                                            ]
                                          },
                                          ""primaryType"": ""WalletConnection"",
                                          ""domain"": {
                                            ""name"": ""MetamaskTutorial"",
                                            ""version"": ""1"",
                                            ""chainId"": 97,
                                            ""verifyingContract"": ""0xCcCCccccCCCCcCCCCCCcCcCccCcCCCcCcccccccC""
                                          },
                                          ""message"": {
                                            ""User"": ""ADDRESS_HERE"",
                                            ""AppName"": ""Roman_Tutorial_MetamaskSDK"",
                                            ""Message"": ""Welcome to this Tutorial"",
                                            ""Timestamp"": ""2023""
                                          }
                                       }";
            
            msgParams = msgParams.Replace("ADDRESS_HERE", MetaMaskWallet.CurrentWalletAddress);
            string from = MetaMaskUnity.Instance.Wallet.SelectedAddress;

            var paramsArray = new string[] { from, msgParams };

            var request = new MetaMaskEthereumRequest
            {
                Method = _ethRequestsStorageSo.SignTypeData,
                Parameters = paramsArray
            };
            OnSignSend?.Invoke(this, EventArgs.Empty);
            var result = await MetaMaskUnity.Instance.Wallet.Request(request);
            _contractDataSo.ResultFromSign = result.ToString();
            OnSignedReady?.Invoke(_contractDataSo.ResultFromSign, EventArgs.Empty);
        }

        public async void GetBalance()
        {
            string from = MetaMaskUnity.Instance.Wallet.SelectedAddress;
            var paramsArray = new string[] {from, "latest"};

            var request = new MetaMaskEthereumRequest()
            {
                Method = _ethRequestsStorageSo.GetBalance,
                Parameters = paramsArray
            };
            
            var result = await MetaMaskUnity.Instance.Wallet.Request(request);
            OnBalanceReceived?.Invoke(this, result.ToString());
            MetaMaskUnity.Instance.SaveSession();
        }

        public async void GetTransactionCount()
        {
            string from = MetaMaskUnity.Instance.Wallet.SelectedAddress;
            var paramsArray = new string[] {from, "latest"};

            var request = new MetaMaskEthereumRequest()
            {
                Method = _ethRequestsStorageSo.GetTransactionCount,
                Parameters = paramsArray
            };
            
            var result = await MetaMaskUnity.Instance.Wallet.Request(request);
            OnTransactionCountResult?.Invoke(this, result.ToString());
        }

        public async void SendTransaction()
        {
            GetBuyData(_contractDataSo.CollectionID, _contractDataSo.TokenID);
             
            var transactionParams = new MetaMaskTransaction
            {
                To = _contractDataSo.AddressManager,
                From = MetaMaskUnity.Instance.Wallet.SelectedAddress,
                Value = _contractDataSo.ConvertPriceValue(),
                Data = _contractDataSo.SetContractHash
            };

            var request = new MetaMaskEthereumRequest
            {
                Method = _ethRequestsStorageSo.SendTransaction,
                Parameters = new MetaMaskTransaction[] { transactionParams }
            };
            OnTransactionSent?.Invoke(this, EventArgs.Empty);
            var result = await MetaMaskUnity.Instance.Wallet.Request(request);
            _contractDataSo.ResultFromTransaction = result.ToString();
            OnSendTransactionResult?.Invoke(_contractDataSo.ResultFromTransaction, EventArgs.Empty);
        }
    }
}