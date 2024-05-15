using Common.ResponseUtility;
using Infrastructure.CommandHandlers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Capital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CandidateController> _logger;
        private readonly ICandidateService _candidate;
        private readonly IProgramApplication _program;
        public CandidateController(IConfiguration configuration, ILogger<CandidateController> logger, ICandidateService candidate, IProgramApplication program)
        {
            _configuration = configuration;
            _logger = logger;
            _candidate = candidate;
            _program = program;
            _program = program;
        }


        [HttpPost("CreateCandidateApplication")]
        public async Task<IActionResult> CreateCandidateApplication([FromBody] CandidateDto model)
        {
            try
            {
                //Check the model state of the Request
                if (!ModelState.IsValid)
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.IncompleteRequirement,
                                           false, model)), "application/json");
                }

                //check if the program is available
                var programExist =  await _program.GetProgramApplicationById(model.ProgramId);
                if (programExist.IsSuccessful == false)
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.NotFound, ResponseMessages.NotFound,
                                           false, model)), "application/json");
                }


                var response = await _candidate.CandidateApplication(model);
                if (response == true)
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.OK, ResponseMessages.succesful, true)), "application/json");
                else
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.Failed, false, model)), "application/json");
            }
            catch (Exception ex)
            {
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.ExceptionError, false, model)), "application/json");
            }
        }

        [HttpGet("GetCandidateApplicationById")]
        public async Task<IActionResult> GetCandidateApplicationById([FromQuery] string candidateId)
        {
            try
            {
                if (string.IsNullOrEmpty(candidateId))
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.IncompleteRequirement,
                                           false, null)), "application/json");
                }

                var existing = await _candidate.CandidateListByApplicaionCandidateId(candidateId);
                if (existing.IsSuccessful == false)
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.NotFound, ResponseMessages.NotFound,
                                           false, candidateId)), "application/json");
                }

                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.OK, ResponseMessages.succesful, true, existing)), "application/json");
            }
            catch (Exception ex)
            {
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.ExceptionError, false)), "application/json");
            }
        }
        
        [HttpGet("GetCandidateApplicationByProgramId")]
        public async Task<IActionResult> GetCandidateApplicationByProgramId([FromQuery] string programId)
        {
            try
            {
                if (string.IsNullOrEmpty(programId))
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.IncompleteRequirement,
                                           false, null)), "application/json");
                }

                var existing = await _candidate.CandidateListByApplicaionId(programId);
                if (existing.IsSuccessful == false)
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.NotFound, ResponseMessages.NotFound,
                                           false, programId)), "application/json");
                }

                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.OK, ResponseMessages.succesful, true, existing)), "application/json");
            }
            catch (Exception ex)
            {
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.ExceptionError, false)), "application/json");
            }
        }


    }
}
