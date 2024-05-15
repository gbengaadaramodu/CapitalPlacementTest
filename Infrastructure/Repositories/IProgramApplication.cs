using Core.Domain;
using Infrastructure.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IProgramApplication
    {
        Task<bool> CreateProgramApplication(FormSettingDto formSettingDto);
        Task<bool> UpdateProgramApplication(FormSettingDto formSettingDto);
        Task<bool> DeleteProgramApplication(FormSettingDto existing, string Id);
        Task<(FormSettingDto Response,  bool IsSuccessful)> GetProgramApplicationById(string Id);
        Task<(List<FormSettingDto> Responses, bool IsSuccessful)> GetAllProgramApplication();
    }
}
