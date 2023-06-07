using UnityEngine;

namespace MetaMask.Unity.Tutorial
{
    [CreateAssetMenu(menuName = "EthRequestDataHolder")]
    public class EthRequestsStorageSO : ScriptableObject
    {
        [field: SerializeField] public string SendTransaction { get; private set; } = "eth_sendTransaction";
        [field: SerializeField] public string GetBalance { get; private set; } = "eth_getBalance";
        [field: SerializeField] public string SignTypeData { get; private set; } = "eth_signTypedData_v4";
        [field: SerializeField] public string SwitchChain { get; private set; } = "wallet_switchEthereumChain";
        [field: SerializeField] public string GetTransactionCount { get; private set; } = "eth_getTransactionCount";
        [field: SerializeField] public string GetTransactionByHash { get; private set; } = "eth_getTransactionByHash";
    }
}