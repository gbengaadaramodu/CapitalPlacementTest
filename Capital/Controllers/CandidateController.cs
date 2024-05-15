using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CandidateController> _logger;
        private readonly ICandidateService _candidate;

        public CandidateController(IConfiguration configuration, ILogger<CandidateController> logger, ICandidateService candidate)
        {
            _configuration = configuration;
            _logger = logger;
            _candidate = candidate;

        }
    }
}
