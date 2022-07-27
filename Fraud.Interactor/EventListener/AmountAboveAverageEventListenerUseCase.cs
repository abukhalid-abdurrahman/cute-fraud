using System.Linq;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Concerns.FaultHandling;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Fraud.Interactor.PreBuilts;
using Fraud.UseCase.EventListener;
using Microsoft.Extensions.DependencyInjection;

namespace Fraud.Interactor.EventListener
{
    public sealed class AmountAboveAverageEventListenerUseCase : IEventListenerUseCase
    {
        private static readonly object Locker = new();
        private static AmountAboveAverageEventListenerUseCase _instance;

        public async Task<ReturnResult<bool>> HandleEvent(Order orderEntity)
        {
            var eventResult = new ReturnResult<bool>();
            var errorMessage = "Error was occurred in performing AmountAboveAverageEvent! Reason: {0}";

            using var serviceScope = ServiceActivator.GetScope();
            if (serviceScope == null)
            {
                FaultHandler.HandleError(ref eventResult,
                    string.Format(errorMessage, "ServiceScope is not initialized for ServiceActivator!"));
                return eventResult;
            }

            var scenarioRepository = serviceScope.ServiceProvider
                .GetRequiredService<ILocalScenarioRepository>();
            if (scenarioRepository == null)
            {
                FaultHandler.HandleError(ref eventResult,
                    string.Format(errorMessage, "ILocalScenarioRepository is not injected or initialization failed!"));
                return eventResult;
            }

            var eventsHistoryRepository = serviceScope.ServiceProvider
                .GetRequiredService<IEventsHistoryRepository>();
            if (eventsHistoryRepository == null)
            {
                FaultHandler.HandleError(ref eventResult,
                    string.Format(errorMessage, "IEventsHistoryRepository is not injected or initialization failed!"));
                return eventResult;
            }

            var scenarioRuleResult = await scenarioRepository.GetScenarioGraph(orderEntity.UserId);
            if (!scenarioRuleResult.IsSuccessfully)
            {
                FaultHandler.HandleError(ref eventResult,
                    string.Format(errorMessage, scenarioRuleResult.DetailedMessage ?? scenarioRuleResult.Message));
                return eventResult;
            }

            var orderEventHistory =
                await eventsHistoryRepository.GetEventHistoryByOrderId(orderEntity.ExternalRef);
            if (!orderEventHistory.IsSuccessfully || orderEventHistory.Result == null)
                FaultHandler.HandleWarning(ref eventResult, 
                    string.Format(errorMessage, "Event history not found!"), orderEventHistory.Message ?? orderEventHistory.DetailedMessage);
            
            var orderStateVertex = scenarioRuleResult.Result
                .StateEdges.FirstOrDefault(x => x.EventType == EventType.AmountAboveAverageEvent);

            if (orderStateVertex == null)
                return ReturnResult<bool>.SuccessResult(true);

            var raisedEventResult = await PreBuildsStore.GetPreBuiltEvent(orderStateVertex.EventType);
            if (!raisedEventResult.IsSuccessfully || raisedEventResult.Result == null)
                return ReturnResult<bool>.FailResult(detailedMessage: raisedEventResult.DetailedMessage);
            
            var updateEventHistoryStateAndEvent = 
                await eventsHistoryRepository.SetEventHistoryOrderStateAndEvent(orderEntity.ExternalRef, raisedEventResult.Result.Id, orderStateVertex.ToStateId);
            if (!updateEventHistoryStateAndEvent.IsSuccessfully)
                return ReturnResult<bool>.FailResult(detailedMessage: raisedEventResult.DetailedMessage);
            
            
            
            
            return eventResult;
        }

        public static IEventListenerUseCase GetInstance()
        {
            lock (Locker)
            {
                return _instance ??= new AmountAboveAverageEventListenerUseCase();
            }
        }
    }
}