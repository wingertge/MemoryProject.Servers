using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MemoryApi.Services;
using MemoryCore.JsonModels;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;

namespace MemoryApi.Controllers
{
    public static class AuthController
    {
        private static readonly IServiceLocator Services = new ServiceLocator().Instance;

        [FunctionName("AuthLogin")]
        public static async Task<HttpResponseMessage> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/login")] HttpRequestMessage req, TraceWriter log)
        {
            if (!string.IsNullOrEmpty(req.GetHeader("X-AuthToken")))
                return req.CreateResponse(HttpStatusCode.BadRequest, "Shouldn't send auth token to login!");
            var model = await req.Content.ReadAsAsync<LoginModel>();
            if (!model.IsValid())
                return req.CreateResponse(HttpStatusCode.BadRequest, "Invalid request body.");

            var authService = Services.GetInstance<IAuthService>();
            var result = await authService.Login(model.Identifier, model.Password);
            return result.Succeeded
                ? req.CreateResponse(HttpStatusCode.OK, result.Data)
                : req.CreateResponse(HttpStatusCode.BadRequest, result.Errors);
        }

        [FunctionName("AuthValidate")]
        public static async Task<HttpResponseMessage> Validate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/validate")] HttpRequestMessage req, TraceWriter log)
        {
            var token = req.GetHeader("X-AuthToken");
            if (string.IsNullOrEmpty(token))
                return req.CreateResponse(HttpStatusCode.BadRequest, "Missing auth token.");

            var authService = Services.GetInstance<IAuthService>();
            var result = await authService.ValidateToken(token);
            return result.Succeeded 
                ? req.CreateResponse(HttpStatusCode.OK, result.Data) 
                : req.CreateResponse(HttpStatusCode.BadRequest, "Invalid token.");
        }
    }
}
