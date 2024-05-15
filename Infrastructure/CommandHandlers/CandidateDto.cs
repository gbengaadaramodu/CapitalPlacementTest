using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.CommandHandlers
{
    public class CandidateDto
    {
        public string? CandidateId { get; set; }
        public string ProgramId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public int Nationality { get; set; }
        public string CurrentResience { get; set; }
        public string IDNumber { get; set; }
        public DateTime DateofBirth { get; set; }
       // [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        public List<AdditionalQuestion> AdditionalQuestion { get; set; }
    }


    public  class AdditionalQuestion
    {
        public string QuestionId { get; set; }
        public List<string> Answer {  set; get; }
    }

    public enum Gender
    {
        Female = 1,
        Male,
    }
}
