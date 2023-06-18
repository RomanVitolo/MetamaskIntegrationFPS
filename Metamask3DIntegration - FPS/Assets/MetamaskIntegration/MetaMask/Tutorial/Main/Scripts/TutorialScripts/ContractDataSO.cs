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
    public string Abi = @"[{'inputs': [{ 'internalType': 'address','name': 'collection','type': 'address' },{ 'internalType': 'uint256','name': 'tokenId','type': 'uint256' } ], 'name': 'buy','outputs': [], 'stateMutability': 'payable', 'type': 'function'}]";
    
    [Header("Data")]
    public string AddressManager = "0x42cafF4129259EE5dCD7DfD24E26B65De6afCe52";
    public string CollectionID = "0x7933cbc9e6202ebc24b95b9895f664d83657f9a1";
    public int TokenID;
    public string SetContractHash;
    public string ResultFromSign;
    public string ResultFromTransaction;

    [ContextMenu("ConvertPriceValue")]
    public string ConvertPriceValue()
    {
        double valueToConvert = priceValue;
        BigInteger weiValueConvert = UnitConversion.Convert.ToWei(valueToConvert);
        string hexadecimalValue = weiValueConvert.ToString("X");
        return "0x" + hexadecimalValue;
    }
}
