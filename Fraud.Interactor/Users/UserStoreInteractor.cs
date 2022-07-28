using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Concerns.FaultHandling;
using Fraud.Infrastructure.Repository;
using Fraud.UseCase.Users;

namespace Fraud.Interactor.Users
{
    public class UserStoreInteractor : IUserStoreUseCase
    {
        private readonly IUserRepository _userRepository;

        private Entities.Models.Users _currentLoggedInUser = null;

        public UserStoreInteractor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<ReturnResult<Entities.Models.Users>> GetUserByApiKey(string apiKey)
        {
            return await _userRepository.GetUserByApiKey(apiKey);
        }

        public ReturnResult<Entities.Models.Users> GetCurrentLoggedInUser()
        {
            var errorMessage = "Current logged user not set!";
            var returnResult = new ReturnResult<Entities.Models.Users>();
            
            if (_currentLoggedInUser == null)
            {
                FaultHandler.HandleError(ref returnResult, errorMessage);
                return returnResult;
            }

            returnResult.Result = _currentLoggedInUser;
            returnResult.IsSuccessfully = true;
            return returnResult;
        }

        public ReturnResult<bool> SetCurrentLoggedInUser(Entities.Models.Users users)
        {
            var errorMessage = "Changing current logged user detected! Operation rejected!";
            var returnResult = new ReturnResult<bool>();

            if (_currentLoggedInUser != null)
            {
                FaultHandler.HandleError(ref returnResult, errorMessage);
                returnResult.Result = false;
                return returnResult;
            }

            _currentLoggedInUser = users;

            returnResult.Result = true;
            returnResult.IsSuccessfully = true;
            return returnResult;
        }
    }
}