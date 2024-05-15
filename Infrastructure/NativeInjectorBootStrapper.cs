using Infrastructure.QueryHandlers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ASP.NET Authorization Polices
            //Register the Permission policy handlers
            services.AddScoped<IProgramApplication, ProgramApplication>();
            services.AddScoped<ICandidateService, CandidateService>();


        }
    }
}
