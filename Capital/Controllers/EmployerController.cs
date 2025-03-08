using Common.ResponseUtility;
using Infrastructure.CommandHandlers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Capital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmployerController> _logger;
        private readonly IProgramApplication _program;

        public EmployerController(IConfiguration configuration, ILogger<EmployerController> logger, IProgramApplication program)
        { 
            _configuration = configuration;
            _logger = logger;
            _program = program;

        }

        [HttpPost("CreateProgramApplication")]
        public async Task<IActionResult> CreateProgramApplication([FromBody]FormSettingDto model)
        {
            try
            {
                //Check the model state of the Request
                if (!ModelState.IsValid)
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.IncompleteRequirement,
                                           false, model)), "application/json");
                }

                if(model.CustomQuestion.Count > 0)
                {
                   var checkMultichoices =  model.CustomQuestion.Where(x => x.QuestionType == FormQuestionType.Dropdown || x.QuestionType == FormQuestionType.MultiChoice).ToList();
                    if(checkMultichoices.Count > 0)
                    {
                        foreach(var eachItem in checkMultichoices)
                        {
                            if(eachItem.QuestionType == FormQuestionType.MultiChoice && eachItem.MaxCount == null)
                                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.MaxCount, false, model)), "application/json");

                            if(eachItem.EnableOthers == null)
                                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.EnableOthers, false, model)), "application/json");

                        }
                    }
                }

                var response = await _program.CreateProgramApplication(model);
                if(response == true)
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.OK, ResponseMessages.succesful, true)), "application/json");
                else
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.Failed, false, model)), "application/json");
            }
            catch (Exception ex)
            {
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.ExceptionError, false, model)), "application/json");
            }
        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProgramApplicationById(string Id, FormSettingDto model)
        {
            try
            {
                //Check the model state of the Request
                if (!ModelState.IsValid)
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.IncompleteRequirement,
                                           false, model)), "application/json");
                }

                var existing = await _program.GetProgramApplicationById(Id);
                if (existing.IsSuccessful == false)
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.NotFound, ResponseMessages.NotFound,
                                           false, model)), "application/json");
                }

                if (model.CustomQuestion.Count > 0)
                {
                    var checkMultichoices = model.CustomQuestion.Where(x => x.QuestionType == FormQuestionType.Dropdown || x.QuestionType == FormQuestionType.MultiChoice).ToList();
                    if (checkMultichoices.Count > 0)
                    {
                        foreach (var eachItem in checkMultichoices)
                        {
                            if (eachItem.QuestionType == FormQuestionType.MultiChoice && eachItem.MaxCount == null)
                                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.MaxCount, false, model)), "application/json");

                            if (eachItem.EnableOthers == null)
                                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.EnableOthers, false, model)), "application/json");

                        }
                    }
                }

                var response = await _program.UpdateProgramApplication(model);

                if (response == true)
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.OK, ResponseMessages.Update, true)), "application/json");
                else
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.Failed, false, model)), "application/json");
            }
            catch (Exception ex)
            {
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.ExceptionError, false, model)), "application/json");
            }
        }

        [HttpGet("GetAllProgramApplications")]
        public async Task<IActionResult> GetAllProgramApplications()
        {
            try
            {
                var existing = await _program.GetAllProgramApplication();
                if (existing.IsSuccessful == false)
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.NotFound, ResponseMessages.NotFound,
                                           false)), "application/json");
                }
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.OK, ResponseMessages.succesful, true, existing)), "application/json");
            }
            catch (Exception ex)
            {
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.ExceptionError, false, null)), "application/json");
            }
        }

        [HttpGet("GetProgramApplicationById")]
        public async Task<IActionResult> GetProgramApplicationById([FromQuery] string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.IncompleteRequirement,
                                           false, null)), "application/json");
                }

                var existing = await _program.GetProgramApplicationById(Id);
                if (existing.IsSuccessful == false)
                {
                    return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.NotFound, ResponseMessages.NotFound,
                                           false, Id)), "application/json");
                }

                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.OK, ResponseMessages.succesful, true, existing)), "application/json");
            }
            catch (Exception ex)
            {
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.ExceptionError, false)), "application/json");
            }
        }


        [HttpDelete("DeleteProgramApplication")]
        public async Task<IActionResult> DeleteProgramApplication([FromQuery] string Id)
        {
            var existing = await _program.GetProgramApplicationById(Id);
            if (existing.IsSuccessful == false)
            {
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.NotFound, ResponseMessages.NotFound,
                                       false)), "application/json");
            }

            var resp = await _program.DeleteProgramApplication(existing.Response, Id);
            if(resp != false)
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.OK, ResponseMessages.succesful, true)), "application/json");
            else
                return Content(JsonConvert.SerializeObject(PrepareResponse(HttpStatusCode.BadRequest, ResponseMessages.ExceptionError, false)), "application/json");

        }
    }
}
