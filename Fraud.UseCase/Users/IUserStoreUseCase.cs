using System.Threading.Tasks;
using Fraud.Concerns;

namespace Fraud.UseCase.Users
{
    public interface IUserStoreUseCase
    {
        public Task<ReturnResult<Entities.Models.Users>> GetUserByApiKey(string apiKey);
        public ReturnResult<Entities.Models.Users> GetCurrentLoggedInUser();
        public ReturnResult<bool> SetCurrentLoggedInUser(Entities.Models.Users users);
    }
}