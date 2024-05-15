using Newtonsoft.Json;

namespace Core.Domain
{
    public class BaseObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [JsonProperty("CreatedBy")]
        public string? CreatedBy { get; set; }
        [JsonProperty("ModifiedBy")]
        public string? ModifiedBy { get; set; }
        [JsonProperty("LastModified")]
        public DateTime? LastModified { get; set; }
        [JsonProperty("IsActive")]
        public bool IsActive { get; set; } = true;
        [JsonProperty("IsDeleted")]
        public bool? IsDeleted { get; set; }
    }
}
