using Newtonsoft.Json;

namespace Core.Domain
{
    public class Candidate : BaseObject
    {
        [JsonProperty("CandidateId")]
        public string CandidateId { get; set; }
        [JsonProperty("ProgramId")]
        public string ProgramId { get; set; }
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }
        [JsonProperty("LastName")]
        public string LastName { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("Phone")]
        public string Phone { get; set; }
        [JsonProperty("Nationality")]
        public int Nationality { get; set; }
        [JsonProperty("CurrentResidence")]
        public string CurrentResidence { get; set; }
        [JsonProperty("IDNumber")]
        public string IDNumber { get; set; }
        [JsonProperty("DateofBirth")]
        public DateTime DateofBirth { get; set; }
        // [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonProperty("Gender")]
        public int Gender { get; set; }
        [JsonProperty("AdditionalQuestion")]
        public List<AdditionalQuestion> AdditionalQuestion { get; set; }
    }


    public class AdditionalQuestion
    {
        [JsonProperty("QuestionId")]
        public string QuestionId { get; set; }
        [JsonProperty("Answer")]
        public List<string> Answer { set; get; }
    }
}
