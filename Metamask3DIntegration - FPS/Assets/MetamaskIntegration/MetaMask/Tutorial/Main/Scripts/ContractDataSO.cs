using UnityEngine;

[CreateAssetMenu(menuName = "Contract Parameters")]
public class ContractDataSO : ScriptableObject
{
    [HideInInspector]
    public string PriceValue = "0x2386f26fc10000";
    [HideInInspector]
    public string NetworkID = "0x61";
    [HideInInspector]
    public string Abi = @"[{'inputs': [{ 'internalType': 'address','name': 'collection','type': 'address' },{ 'internalType': 'uint256','name': 'tokenId','type': 'uint256' } ], 'name': 'buy','outputs': [], 'stateMutability': 'payable', 'type': 'function'   }]";
    
    [Header("Data")]
    public string AddressManager = "0x42cafF4129259EE5dCD7DfD24E26B65De6afCe52";
    public string CollectionID = "0x7933cbc9e6202ebc24b95b9895f664d83657f9a1";
    public int TokenID;
    public string SetContractHash;
    public string ResultFromSign;
    public string ResultFromTransaction;
}
