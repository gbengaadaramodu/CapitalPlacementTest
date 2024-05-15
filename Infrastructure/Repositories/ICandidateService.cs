using Infrastructure.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ICandidateService
    {
        Task<bool> CandidateApplication(CandidateDto apply);
        Task<(List<CandidateDto>, bool IsSuccessful)> CandidateListByApplicaionId(string Id);
        Task<(CandidateDto, bool IsSuccessful)> CandidateListByApplicaionCandidateId(string CandidateId);
    }
}
