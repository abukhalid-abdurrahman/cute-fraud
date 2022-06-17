using System.Linq;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Concerns.FaultHandling;
using Fraud.Entities.DTOs.Action;
using Fraud.Entities.DTOs.Event;
using Fraud.Entities.DTOs.PreBuilt;
using Fraud.Infrastructure.Repository;
using Fraud.UseCase.PreBuilts;

namespace Fraud.Interactor.PreBuilts
{
    public class GetPreBuiltEntitiesListInteractor : IGetPreBuiltEntitiesListUseCase
    {
        private readonly IActionRepository _actionRepository;
        private readonly IEventRepository _eventRepository;

        public GetPreBuiltEntitiesListInteractor(
            IActionRepository actionRepository,
            IEventRepository eventRepository)
        {
            _actionRepository = actionRepository;
            _eventRepository = eventRepository;
        }
        
        public async Task<ReturnResult<PreBuiltListDto>> GetPreBuiltList()
        {
            var result = new ReturnResult<PreBuiltListDto>();
            var errorMessageTemplate = "Error was occured while fetching pre-built list! Reason: {0}";

            var preBuiltActionsResult = await _actionRepository.GetAllActions();
            if (!preBuiltActionsResult.IsSuccessfully)
            {
                FaultHandler.HandleError(ref result, string.Format(errorMessageTemplate, preBuiltActionsResult.Message));
                return result;
            }

            var preBuiltEventsResult = await _eventRepository.GetAllEvents();
            if (!preBuiltEventsResult.IsSuccessfully)
            {
                FaultHandler.HandleError(ref result, string.Format(errorMessageTemplate, preBuiltEventsResult.Message));
                return result;
            }

            var actionsListDto = preBuiltActionsResult.Result
                .Select(x => new ActionTypeDto()
                {
                    ActionCode = x.ActionCode,
                    ActionId = x.Id
                })
                .ToArray();

            var eventListDto = preBuiltEventsResult.Result
                .Select(x => new EventTypeDto()
                {
                    EventCode = x.EventCode,
                    EventId = x.Id
                })
                .ToArray();

            result.ErrorCode = 1000;
            result.IsSuccessfully = true;
            result.Result = new PreBuiltListDto()
            {
                Actions = actionsListDto,
                Events = eventListDto
            };

            return result;
        }
    }
}