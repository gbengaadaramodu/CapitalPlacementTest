using Core.Domain;
using Infrastructure.CommandHandlers;
using Infrastructure.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.QueryHandlers
{
    public class ProgramApplication : IProgramApplication
    {
        private readonly Container _programContainer;
        private readonly IConfiguration configuration;

        public ProgramApplication(CosmosClient cosmosClient, IConfiguration configuration)
        {
            this.configuration = configuration;
            var databaseName = configuration["ConnectionString:DatabaseName"];
            var programContainerName = "Programs";
            _programContainer = cosmosClient.GetContainer(databaseName, programContainerName);
        }

        public async Task<bool> CreateProgramApplication(FormSettingDto formSettingDto)
        {
            var resp = false;
            try
            {
                var id = Guid.NewGuid().ToString();
                var request = new ProgramData
                {
                    Id = id,
                    ProgramDataId = id,
                    CreatedDate = DateTime.Now,
                    ProgramTitle = formSettingDto.ProgramTitle,
                    ProgramDescription = formSettingDto.ProgramDescription,
                    PersonalInfo = new Core.Domain.PersonalInfo
                    {
                        FirstName =(int)formSettingDto.PersonalInfo.FirstName,
                        LastName = (int)formSettingDto.PersonalInfo.LastName,
                        Email = (int)formSettingDto.PersonalInfo.Email,
                        Phone = (int)formSettingDto.PersonalInfo.Phone,
                        Nationality = (int)formSettingDto.PersonalInfo.Nationality,
                        CurrentResience = (int)formSettingDto.PersonalInfo.CurrentResience,
                        DateofBirth = (int)formSettingDto.PersonalInfo.DateofBirth,
                        Gender = (int)formSettingDto.PersonalInfo.Gender,
                        IDNumber = (int)formSettingDto.PersonalInfo.IDNumber,
                    },
                };

                if(formSettingDto.CustomQuestion.Count > 0)
                {
                    var items = new List<Core.Domain.CustomQuestion>();
                    foreach(var eachItem in formSettingDto.CustomQuestion)
                    {
                        var temp = new Core.Domain.CustomQuestion
                        {
                            QuestionId = Guid.NewGuid().ToString(),
                            Question = eachItem.Question,
                            QuestionType = (int)eachItem.QuestionType,
                        };

                        items.Add(temp);
                    }

                    request.CustomQuestion = items;
                }
                var response = await _programContainer.CreateItemAsync(request);
                
                resp = response.Resource != null;
            }
            catch (Exception ex) 
            { 
                //logging error
            }

            return resp;
        }

        public async Task<bool> DeleteProgramApplication(FormSettingDto formSettingDto, string Id)
        {
            var resp = false;
            try
            {
                var newId = Guid.NewGuid().ToString();
                var request = new ProgramData
                {
                    Id = formSettingDto.Id,
                    ProgramDataId = Id,
                    ProgramTitle = formSettingDto.ProgramTitle,
                    ProgramDescription = formSettingDto.ProgramDescription,
                    PersonalInfo = new Core.Domain.PersonalInfo
                    {
                        FirstName = (int)formSettingDto.PersonalInfo.FirstName,
                        LastName = (int)formSettingDto.PersonalInfo.LastName,
                        Email = (int)formSettingDto.PersonalInfo.Email,
                        Phone = (int)formSettingDto.PersonalInfo.Phone,
                        Nationality = (int)formSettingDto.PersonalInfo.Nationality,
                        CurrentResience = (int)formSettingDto.PersonalInfo.CurrentResience,
                        DateofBirth = (int)formSettingDto.PersonalInfo.DateofBirth,
                        Gender = (int)formSettingDto.PersonalInfo.Gender,
                        IDNumber = (int)formSettingDto.PersonalInfo.IDNumber,
                    },
                };

                if (formSettingDto.CustomQuestion.Count > 0)
                {
                    var items = new List<Core.Domain.CustomQuestion>();
                    foreach (var eachItem in formSettingDto.CustomQuestion)
                    {
                        var temp = new Core.Domain.CustomQuestion
                        {
                            Question = eachItem.Question,
                            QuestionType = (int)eachItem.QuestionType,
                        };

                        items.Add(temp);
                    }

                    request.CustomQuestion = items;
                }



                await _programContainer.DeleteItemAsync<ProgramData>(Id, new PartitionKey(request.ProgramDataId));
                resp= true;
            }
            catch(Exception ex)
            {
                //logging
                resp = false;
            }
            return resp;
        }

        public async Task<(List<FormSettingDto> Responses, bool IsSuccessful)> GetAllProgramApplication()
        {
            var programDatas = new List<FormSettingDto>();
            var success = false;
            try
            {
                var query = _programContainer.GetItemLinqQueryable<ProgramData>().ToFeedIterator();

                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();

                    var responseItems = response.Resource;
                    foreach ( var responseItem in responseItems)
                    {
                        var temp = responseItem;

                        var resp = new FormSettingDto
                        {
                            Id = temp.ProgramDataId,
                            ProgramTitle = temp.ProgramTitle,
                            ProgramDescription = temp.ProgramDescription,
                            PersonalInfo = new CommandHandlers.PersonalInfo
                            {
                                FirstName = (Requirement)temp.PersonalInfo.FirstName,
                                LastName = (Requirement)temp.PersonalInfo.LastName,
                                Email = (Display)temp.PersonalInfo.Email,
                                Phone = (Display)temp.PersonalInfo.Phone,
                                Nationality = (Display)temp.PersonalInfo.Nationality,
                                CurrentResience = (Display)temp.PersonalInfo.CurrentResience,
                                DateofBirth = (Display)temp.PersonalInfo.DateofBirth,
                                Gender = (Display)temp.PersonalInfo.Gender,
                                IDNumber = (Display)temp.PersonalInfo.IDNumber,
                            },
                        };

                        if (temp.CustomQuestion.Count > 0)
                        {
                            var items = new List<CommandHandlers.CustomQuestion>();
                            foreach (var eachItem in temp.CustomQuestion)
                            {
                                var temp2 = new CommandHandlers.CustomQuestion
                                {
                                    QuestionId = eachItem.QuestionId,
                                    Question = eachItem.Question,
                                    QuestionType = (FormQuestionType)eachItem.QuestionType,
                                };

                                items.Add(temp2);
                            }

                            resp.CustomQuestion = items;
                        }

                        programDatas.Add(resp);
                    }
                }

                success = true;


            }
            catch (Exception ex)
            {
                //logging
            }

            return (programDatas, success);
        }

        public async Task<(FormSettingDto Response, bool IsSuccessful)> GetProgramApplicationById(string Id)
        {
            var resp = new FormSettingDto();
            var success = false;
            try
            {
                var query = _programContainer.GetItemLinqQueryable<ProgramData>().Where(t => t.Id == Id).Take(1).ToQueryDefinition();

                var sqlQuery = query.QueryText; // Retrieve the SQL query

                var response = await _programContainer.GetItemQueryIterator<ProgramData>(query).ReadNextAsync();
                if (response.FirstOrDefault() != null)
                {
                    var temp = response.FirstOrDefault();
                    resp = new FormSettingDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProgramTitle = temp.ProgramTitle,
                        ProgramDescription = temp.ProgramDescription,
                        PersonalInfo = new CommandHandlers.PersonalInfo
                        {
                            FirstName = (Requirement)temp.PersonalInfo.FirstName,
                            LastName = (Requirement)temp.PersonalInfo.LastName,
                            Email = (Display)temp.PersonalInfo.Email,
                            Phone = (Display)temp.PersonalInfo.Phone,
                            Nationality = (Display)temp.PersonalInfo.Nationality,
                            CurrentResience = (Display)temp.PersonalInfo.CurrentResience,
                            DateofBirth = (Display)temp.PersonalInfo.DateofBirth,
                            Gender = (Display)temp.PersonalInfo.Gender,
                            IDNumber = (Display)temp.PersonalInfo.IDNumber,
                        },
                    };

                    if (temp.CustomQuestion.Count > 0)
                    {
                        var items = new List<CommandHandlers.CustomQuestion>();
                        foreach (var eachItem in temp.CustomQuestion)
                        {
                            var temp2 = new CommandHandlers.CustomQuestion
                            {
                                QuestionId = eachItem.QuestionId,
                                Question = eachItem.Question,
                                QuestionType = (FormQuestionType)eachItem.QuestionType,
                            };

                            items.Add(temp2);
                        }

                        resp.CustomQuestion = items;
                    }

                    success = true;
                }
            }
            catch (Exception ex)
            {

                //logging error
            }


            return (resp, success);
          
        }

        public async Task<bool> UpdateProgramApplication(FormSettingDto formSettingDto)
        {
            var resp = false;
            try
            {
                var request = new ProgramData
                {
                    Id = formSettingDto.Id,
                    LastModified = DateTime.Now,
                    ProgramTitle = formSettingDto.ProgramTitle,
                    ProgramDescription = formSettingDto.ProgramDescription,
                    PersonalInfo = new Core.Domain.PersonalInfo
                    {
                        FirstName = (int)formSettingDto.PersonalInfo.FirstName,
                        LastName = (int)formSettingDto.PersonalInfo.LastName,
                        Email = (int)formSettingDto.PersonalInfo.Email,
                        Phone = (int)formSettingDto.PersonalInfo.Phone,
                        Nationality = (int)formSettingDto.PersonalInfo.Nationality,
                        CurrentResience = (int)formSettingDto.PersonalInfo.CurrentResience,
                        DateofBirth = (int)formSettingDto.PersonalInfo.DateofBirth,
                        Gender = (int)formSettingDto.PersonalInfo.Gender,
                        IDNumber = (int)formSettingDto.PersonalInfo.IDNumber,
                    },
                };

                if (formSettingDto.CustomQuestion.Count > 0)
                {
                    var items = new List<Core.Domain.CustomQuestion>();
                    foreach (var eachItem in formSettingDto.CustomQuestion)
                    {
                        var temp = new Core.Domain.CustomQuestion
                        {
                            Question = eachItem.Question,
                            QuestionType = (int)eachItem.QuestionType,
                        };

                        items.Add(temp);
                    }

                    request.CustomQuestion = items;
                }
                var response = await _programContainer.ReplaceItemAsync(request, request.Id);

                resp = true;
            }
            catch (Exception ex) {

                //logging error
            }


            return resp;
        }
    }
}
