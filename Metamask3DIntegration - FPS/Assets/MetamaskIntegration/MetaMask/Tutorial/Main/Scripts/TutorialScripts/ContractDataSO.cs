using System.Numerics;
using Nethereum.Util;
using UnityEngine;

[CreateAssetMenu(menuName = "Contract Parameters")]
public class ContractDataSO : ScriptableObject
{
    private readonly double priceValue = 0.01f;
    [HideInInspector] 
    public string NetworkID = "0x61";
    [HideInInspector]
    public string ChainName = "BSC Testnet";
    [HideInInspector]
    public string[] RpcUrls = {"https://data-seed-prebsc-1-s1.binance.org:8545/"};
    [HideInInspector]
    public string Abi = @"[{'inputs': [{ 'internalType': 'address','name': 'collection','type': 'address' },{ 'internalType': 'uint256','name': 'tokenId','type': 'uint256' } ], 'name': 'buy','outputs': [], 'stateMutability': 'payable', 'type': 'function'}]";
    
    [Header("Data")]
    public string AddressManager = "";
    public string CollectionID = "";
    public int TokenID;
    public string SetContractHash;
    public string ResultFromSign;
    public string ResultFromTransaction;

    public string ConvertPriceValue()
    {
        double valueToConvert = priceValue;
        BigInteger weiValueConvert = UnitConversion.Convert.ToWei(valueToConvert);
        string hexadecimalValue = weiValueConvert.ToString("X");
        return "0x" + hexadecimalValue;
    }
}
