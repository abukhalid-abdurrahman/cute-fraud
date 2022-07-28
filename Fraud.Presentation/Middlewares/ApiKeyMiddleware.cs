using System.Threading.Tasks;
using Fraud.Entities.DTOs;
using Fraud.UseCase.Users;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Fraud.Presentation.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserStoreUseCase _userStoreUseCase;

        private const string ApiKeyHeaderName = "X-Api-Key";

        public ApiKeyMiddleware(RequestDelegate next, IUserStoreUseCase userStoreUseCase)
        {
            _next = next;
            _userStoreUseCase = userStoreUseCase;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var errorMessage = "X-Api-Key header not provided, or api key is invalid!";
            var responseContent = JsonConvert.SerializeObject(Response<object>.FailResponse(errorMessage));

            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                await context.Response.WriteAsync(responseContent);
                return;
            }

            var userByApiKeyResult = await _userStoreUseCase.GetUserByApiKey(extractedApiKey);
            if (!userByApiKeyResult.IsSuccessfully || userByApiKeyResult.Result == null)
            {
                await context.Response.WriteAsync(responseContent);
                return;
            }

            if (!userByApiKeyResult.Result.ApiKey.Equals(extractedApiKey)) 
            {
                await context.Response.WriteAsync(responseContent);
                return;
            }

            _userStoreUseCase.SetCurrentLoggedInUser(userByApiKeyResult.Result);

            await _next(context);
        }
    }
}