using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.CommandHandlers
{
    public class FormSettingDto
    {
        public string? Id {  get; set; }
        public string ProgramTitle { get; set; }
        public string ProgramDescription { get; set; } 
        public PersonalInfo PersonalInfo { get; set; }
        public List<CustomQuestion> CustomQuestion { get; set; }

    }

    public class CustomQuestion
    {
      //  [JsonConverter(typeof(JsonStringEnumConverter))]
      public string? QuestionId {  get; set; } = string.Empty;
        public FormQuestionType QuestionType { get; set; }
        public string Question {  get; set; }
        public bool? EnableOthers { get; set; }
        public int? MaxCount { get; set; }
    }


    public  class PersonalInfo
    {
        public Requirement FirstName { get; set; }
        public Requirement LastName { get; set; }
        public Display Email {  get; set; }
        public Display Phone { get; set; }   
        public Display Nationality { get; set; }
        public Display CurrentResience { get; set; }
        public Display IDNumber { get; set; }
        public Display DateofBirth { get; set; }
        public Display Gender { get; set; }
    }

    public enum FormQuestionType
    {
        MultiChoice,
        Paragraph,
        YesNo,
        Dropdown,
        Date,
        Number
    }

    public enum Display
    {
        Hide,
        Show
    }

    public enum Requirement
    {
        Mandatory,
        Optional
    }
}
