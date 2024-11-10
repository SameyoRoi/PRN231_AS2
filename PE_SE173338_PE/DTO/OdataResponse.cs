using Newtonsoft.Json;

namespace PE_SE173338_PE.DTO
{
    public class OdataResponse<T>
    {
        [JsonProperty("value")]
        public List<T> Value { get; set; }

        [JsonProperty("@odata.count")]
        public int Count { get; set; }
    }
}
