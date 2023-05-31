using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MetaMask.Unity.Tutorial
{
    public class VisualController : MonoBehaviour
    {
        [Header("BlockchainController")]
        [SerializeField] private BlockchainController _blockchainController;
       
        [Header("ContractData")]
        [SerializeField] private ContractDataSO _contractDataSo;
        
        [Header("QR View")]
        [SerializeField] private RawImage QRCodeImage;
        [SerializeField] private Sprite loadingSprite;
        [SerializeField] private GameObject _principalScreenUI;
        
        [Header("After Sign View")]
        [SerializeField] private GameObject _userProfileUI;
        [SerializeField] private TextMeshProUGUI _currentWalletAddress;
        [SerializeField] private TextMeshProUGUI _currentWalletBalance;
        [SerializeField] private TextMeshProUGUI _currentStates;

        private void OnEnable()
        {
            if (_blockchainController == null) 
                _blockchainController = FindObjectOfType<BlockchainController>();
            InitializedEvents();
        }

        private void Start()
        {
            _blockchainController.Connect();
            _contractDataSo.ResultFromSign = string.Empty;
            _contractDataSo.ResultFromTransaction = string.Empty;
        }

        private void InitializedEvents()
        {
            _blockchainController.OnWalletConnected += OnWalletConnected;
            _blockchainController.OnWalletDisconnected += OnWalletDisconnected;
            _blockchainController.OnWalletPaused += OnWalletPaused;
            _blockchainController.OnWalletReady += OnWalletReady;
            _blockchainController.OnSignSend += OnSignSend;
            _blockchainController.OnTransactionSent += OnTransactionSent;
            _blockchainController.OnTransactionResult += OnTransactionResult;
            _blockchainController.OnSignedReady += OnSignIsReady;
            _blockchainController.OnBalanceReceived += OnBalanceReceived;
            _blockchainController.OnEthRequestFailed += OnErrorResult;
        }

        public void ConnectToServer() => StartCoroutine(ConnectToServerCoroutine());
        
        private  IEnumerator ConnectToServerCoroutine()
        {
            yield return new WaitForSeconds(1f);
            _userProfileUI.SetActive(false);
            _principalScreenUI.SetActive(false);
            _blockchainController.Connect();
        }

        private void OnDisable()
        {
            RemoveEventsSubscriptions();
        }

        private void RemoveEventsSubscriptions()
        {
            _blockchainController.OnWalletConnected -= OnWalletConnected;
            _blockchainController.OnWalletDisconnected += OnWalletDisconnected;
            _blockchainController.OnWalletPaused += OnWalletPaused;
            _blockchainController.OnWalletReady -= OnWalletReady;
            _blockchainController.OnSignSend -= OnSignSend;
            _blockchainController.OnTransactionSent -= OnTransactionSent;
            _blockchainController.OnSignedReady -= OnSignIsReady;
            _blockchainController.OnBalanceReceived -= OnBalanceReceived;
            _blockchainController.OnEthRequestFailed -= OnErrorResult;
        }

        private void OnWalletReady(object sender, EventArgs e)
        {
            
        }
        private void OnWalletPaused(object sender, EventArgs e)
        {
            
        }

        private void OnWalletConnected(object sender, EventArgs e)
        {
            _blockchainController.Sign();
        }

        private void OnSignSend(object sender, EventArgs e)
        {
            _currentWalletAddress.text = "Current Wallet: " + MetaMaskUnity.Instance.Wallet.SelectedAddress;
            _blockchainController.GetBalance();
        }
        
        private void OnSignIsReady(object sender, EventArgs e)
        {
            CheckUIConnection();
        }

        public void CheckUIConnection()
        {
            if (_contractDataSo.ResultFromSign != "")
            {
                _principalScreenUI.SetActive(false);
                _userProfileUI.SetActive(true);
                StartCoroutine(WaitForWallet());
                _currentStates.text = "Welcome to you Web3 Profile";
            }
            else if(_contractDataSo.ResultFromSign == "")
            {
                _principalScreenUI.SetActive(true);
                _userProfileUI.SetActive(false);
            }
        }

        private IEnumerator WaitForWallet()
        {
            yield return new WaitForSeconds(1.5f);
            _currentWalletAddress.text = "Current Wallet: " + MetaMaskUnity.Instance.Wallet.SelectedAddress;
        }

        private void OnBalanceReceived(object sender, string balance) 
        {
            ConvertWeiToBnb(balance);
        }                
        
        private void ConvertWeiToBnb(string amount)
        {
            var getBalance = Convert.ToInt64(amount, 16) / (decimal)Math.Pow(10, 18);
            _currentWalletBalance.text = "Current Balance: " + getBalance.ToString() + " " + "BNB";
        }

        private void OnTransactionSent(object sender, EventArgs e)
        {
            _currentStates.text = "Send Transaction, Please Check you Metamask App";
        }

        private void OnTransactionResult(object sender, MetaMaskEthereumRequestResultEventArgs e)
        {
            
        }

        private void OnWalletDisconnected(object sender, EventArgs e)
        {
            _contractDataSo.ResultFromSign = string.Empty;
            _currentStates.text = "Your wallet is Disconnected, Please repeat the connection steps again.";
            StartCoroutine(ConnectToServerCoroutine());
        }
        
        private void OnErrorResult(object sender, MetaMaskEthereumRequestFailedEventArgs e)
        {
            //SpinnerUI.SetActive(false);
            //_buyingNFtLoading.SetActive(false); 
            _currentStates.text = "Error in the request. Please try again...";
        }
    }
    
}