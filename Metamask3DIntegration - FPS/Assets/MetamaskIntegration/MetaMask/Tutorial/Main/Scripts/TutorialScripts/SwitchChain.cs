using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MetaMask.Unity
{
    public class SwitchChain
    {
        [JsonProperty("chainId")]
        [JsonPropertyName("chainId")]
        public string ChainId { get; set; }
    }
}