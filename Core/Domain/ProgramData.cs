using Newtonsoft.Json;

namespace Core.Domain
{
    public class ProgramData : BaseObject
    {

        [JsonProperty("ProgramDataId")]
        public string ProgramDataId { get; set; }
        [JsonProperty("ProgramTitle")]
        public string ProgramTitle { get; set; }
        [JsonProperty("ProgramDescription")]
        public string ProgramDescription { get; set; }
        [JsonProperty("PersonalInfo")]
        public PersonalInfo PersonalInfo { get; set; }
        [JsonProperty("CustomQuestion")]
        public List<CustomQuestion> CustomQuestion { get; set; }

    }

    public class CustomQuestion
    {
        
        [JsonProperty("QuestionId")]
        public string QuestionId { get; set; }
        [JsonProperty("QuestionType")]
        public int QuestionType { get; set; }
        [JsonProperty("Question")]
        public string Question { get; set; }
    }

    public class PersonalInfo
    {
        [JsonProperty("FirstName")]
        public int FirstName { get; set; }
        [JsonProperty("LastName")]
        public int LastName { get; set; }
        [JsonProperty("Email")]
        public int Email { get; set; }
        [JsonProperty("Phone")]
        public int Phone { get; set; }
        [JsonProperty("Nationality")]
        public int Nationality { get; set; }
        [JsonProperty("CurrentResience")]
        public int CurrentResience { get; set; }
        [JsonProperty("IDNumber")]
        public int IDNumber { get; set; }
        [JsonProperty("DateofBirth")]
        public int DateofBirth { get; set; }
        [JsonProperty("Gender")]
        public int Gender { get; set; }
    }
}
