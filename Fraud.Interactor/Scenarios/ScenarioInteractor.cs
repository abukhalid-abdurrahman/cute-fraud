using System;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.DTOs.Scenario;
using Fraud.Infrastructure.Repository;
using Fraud.UseCase.Scenario;
using Fraud.UseCase.Users;

namespace Fraud.Interactor.Scenarios
{
    public class ScenarioInteractor : IScenarioUseCase
    {
        private readonly IStateRepository _stateRepository;
        private readonly ILocalScenarioRepository _localScenarioRepository;
        private readonly IUserStoreUseCase _userStoreUseCase;

        public ScenarioInteractor(IStateRepository stateRepository, 
            ILocalScenarioRepository localScenarioRepository,
            IUserStoreUseCase userStoreUseCase)
        {
            _stateRepository = stateRepository;
            _localScenarioRepository = localScenarioRepository;
            _userStoreUseCase = userStoreUseCase;
        }

        public async Task<ReturnResult<bool>> CreateUserScenario(GraphScenarioDto graphScenarioDto)
        {
            throw new ArgumentNullException();
        }

        public async Task<ReturnResult<bool>> DeleteUserScenario()
        {
            var currentUserResult = _userStoreUseCase.GetCurrentLoggedInUser();
            if (!currentUserResult.IsSuccessfully)
                return ReturnResult<bool>.FailResult(result: false, detailedMessage: currentUserResult.Message);
            return await _localScenarioRepository.DeleteScenarioByUserId(currentUserResult.Result.Id);
        }

        public async Task<ReturnResult<GraphScenarioDto>> GetUserScenario()
        {
            var currentUserResult = _userStoreUseCase.GetCurrentLoggedInUser();
            if (!currentUserResult.IsSuccessfully)
                return ReturnResult<GraphScenarioDto>.FailResult(detailedMessage: currentUserResult.Message);
            return await _localScenarioRepository.GetScenarioGraphByUserId(currentUserResult.Result.Id);
        }
    }
}