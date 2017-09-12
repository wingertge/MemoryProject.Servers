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
    public class UsersController
    {
        private static readonly IServiceLocator Services = new ServiceLocator().Instance;

        [FunctionName("UsersRegister")]
        public static async Task<HttpResponseMessage> Register([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/register")] HttpRequestMessage req, TraceWriter log)
        {
            if (!string.IsNullOrEmpty(req.GetHeader("X-AuthToken")))
                return req.CreateResponse(HttpStatusCode.BadRequest, "Received auth token on register.");

            var model = await req.Content.ReadAsAsync<RegisterModel>();
            if (!model?.IsValid() ?? true)
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid request body.");

            var userService = Services.GetInstance<IUserService>();
            var result = await userService.CreateUser(model.Email, model.Username, model.Password);
            if (!result.Succeeded) return req.CreateResponse(HttpStatusCode.BadRequest, result.Errors);
            var user = result.Data;
            var authService = Services.GetInstance<IAuthService>();
            var loginResult = await authService.Login(user.Email, model.Password);
            if (loginResult.Succeeded) return req.CreateResponse(HttpStatusCode.OK, loginResult.Data);

            log.Error("Failed to login newly created user! Errors: " + JsonConvert.SerializeObject(loginResult.Errors));
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}