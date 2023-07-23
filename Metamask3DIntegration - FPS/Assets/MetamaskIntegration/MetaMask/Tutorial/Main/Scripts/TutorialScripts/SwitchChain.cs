using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MetaMask.Unity
{
    public class NativeCurrency
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonProperty("symbol")]
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("decimals")]
        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }
    }
    
    public class AddEthereumChain
    {
        [JsonProperty("chainId")]
        [JsonPropertyName("chainId")]
        public string ChainId { get; set; }
        
        [JsonProperty("chainName")]
        [JsonPropertyName("chainName")]
        public string ChainName { get; set; }

        [JsonProperty("nativeCurrency")]
        [JsonPropertyName("nativeCurrency")]
        public NativeCurrency NativeCurrency { get; set; }
        
        [JsonProperty("rpcUrls")]
        [JsonPropertyName("rpcUrls")]
        public string[] RpcUrls { get; set; }

    }
    public class SwitchChain
    {
        [JsonProperty("chainId")]
        [JsonPropertyName("chainId")]
        public string ChainId { get; set; }
    }
}