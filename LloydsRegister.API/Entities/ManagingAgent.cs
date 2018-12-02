using Newtonsoft.Json;

namespace LloydsRegister.API.Entities
{
    public class ManagingAgent
    {
        [JsonProperty(PropertyName = "id")]
        public string Id => "managingAgent_" + AgentCode;

        public string AgentCode { get; set; }

        public string AgentName { get; set; }
    }
}
