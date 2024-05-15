using Azure.Core;
using Core.Domain;
using Infrastructure.CommandHandlers;
using Infrastructure.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.QueryHandlers
{
    public class CandidateService : ICandidateService
    {
        private readonly Container _applyContainer;
        private readonly IConfiguration configuration;

        public CandidateService(CosmosClient cosmosClient, IConfiguration configuration)
        {
            this.configuration = configuration;
            var databaseName = configuration["ConnectionString:DatabaseName"];
            var applyContainerName = "Applications";
            _applyContainer = cosmosClient.GetContainer(databaseName, applyContainerName);
        }

        public async Task<bool> CandidateApplication(CandidateDto apply)
        {
            var resp = false;
            try
            {
                var candidate = new Candidate
                {
                   Id = Guid.NewGuid().ToString(),
                   CandidateId = Guid.NewGuid().ToString(),
                   FirstName = apply.FirstName,
                   LastName = apply.LastName,
                   ProgramId = apply.ProgramId,
                   Email = apply.Email,
                   Phone = apply.Phone,
                   IDNumber = apply.IDNumber,
                   IsActive = true,
                   Nationality = apply.Nationality, 
                   Gender = (int)apply.Gender,
                   CurrentResidence = apply.CurrentResience,
                   DateofBirth = apply.DateofBirth,
                   CreatedDate = DateTime.Now,
                };

                if(apply.AdditionalQuestion.Count > 0 )
                {
                    var items = new List<Core.Domain.AdditionalQuestion>();
                    foreach (var eachItem in apply.AdditionalQuestion)
                    {
                        var item = new Core.Domain.AdditionalQuestion
                        {
                            QuestionId = eachItem.QuestionId,
                            Answer = eachItem.Answer,
                        };

                        items.Add(item);
                    }

                    candidate.AdditionalQuestion = items;
                }

                var response = await _applyContainer.CreateItemAsync(candidate);

                resp = response.Resource != null;
            }
            catch (Exception ex)
            {
                //logging
            }
            return resp;
        }

        public async Task<(CandidateDto, bool IsSuccessful)> CandidateListByApplicaionCandidateId(string candidateId)
        {
            var candidate = new CandidateDto();
            var success = false;
            try
            {

                var query = _applyContainer.GetItemLinqQueryable<Candidate>().Where(t => t.CandidateId == candidateId).Take(1).ToQueryDefinition();

                var sqlQuery = query.QueryText; // Retrieve the SQL query

                var response = await _applyContainer.GetItemQueryIterator<Candidate>(query).ReadNextAsync();
                if (response.FirstOrDefault() != null)
                {
                    var apply = response.FirstOrDefault();
                    candidate = new CandidateDto
                    {
                        CandidateId = apply.CandidateId,
                        ProgramId = apply.ProgramId,
                        FirstName = apply.FirstName,
                        LastName = apply.LastName,
                        Email = apply.Email,
                        Phone = apply.Phone,
                        IDNumber = apply.IDNumber,
                        Nationality = apply.Nationality,
                        Gender = (Gender)apply.Gender,
                        CurrentResience = apply.CurrentResidence,
                        DateofBirth = apply.DateofBirth,
                    };

                    if (apply.AdditionalQuestion.Count > 0)
                    {
                        var items = new List<CommandHandlers.AdditionalQuestion>();
                        foreach (var eachItem in apply.AdditionalQuestion)
                        {
                            var temp2 = new CommandHandlers.AdditionalQuestion
                            {
                                QuestionId = eachItem.QuestionId,
                                Answer = eachItem.Answer,
                            };

                            items.Add(temp2);
                        }

                        candidate.AdditionalQuestion = items;
                    }

                    success = true;
                }

            }
            catch (Exception ex) { 
                //logging
            }

            return (candidate, success);
        }

        public async Task<(List<CandidateDto>, bool IsSuccessful)> CandidateListByApplicaionId(string programId)
        {

            var candidates = new List<CandidateDto>();
            var success = false;
            try
            {

                var query = _applyContainer.GetItemLinqQueryable<Candidate>().Where(x=>x.ProgramId == programId).ToFeedIterator();

                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();

                    var responseItems = response.Resource;
                    foreach (var responseItem in responseItems)
                    {
                        var apply = responseItem;

                        var candidate = new CandidateDto
                        {
                            CandidateId = apply.CandidateId,
                            ProgramId = apply.ProgramId,
                            FirstName = apply.FirstName,
                            LastName = apply.LastName,
                            Email = apply.Email,
                            Phone = apply.Phone,
                            IDNumber = apply.IDNumber,
                            Nationality = apply.Nationality,
                            Gender = (Gender)apply.Gender,
                            CurrentResience = apply.CurrentResidence,
                            DateofBirth = apply.DateofBirth,
                        };
                        if (apply.AdditionalQuestion.Count > 0)
                        {
                            var items = new List<CommandHandlers.AdditionalQuestion>();
                            foreach (var eachItem in apply.AdditionalQuestion)
                            {
                                var temp2 = new CommandHandlers.AdditionalQuestion
                                {
                                    QuestionId = eachItem.QuestionId,
                                    Answer = eachItem.Answer,
                                };

                                items.Add(temp2);
                            }

                            candidate.AdditionalQuestion = items;
                        }

                        candidates.Add(candidate);
                    }
                }

                success = true;

            }
            catch (Exception ex)
            {
                //logging
            }

            return (candidates, success);
        }
    }
}
