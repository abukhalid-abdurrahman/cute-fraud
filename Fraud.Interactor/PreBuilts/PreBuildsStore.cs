using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Fraud.Interactor.PreBuilts
{
    public static class PreBuildsStore
    {
        private static Dictionary<EventType, Events> _eventsMap;
        private static Dictionary<ActionType, Action> _actionsMap;
        private static bool _isConfigured = false;

        private static async Task<ReturnResult<bool>> ConfigureStore()
        {
            using var serviceScope = ServiceActivator.GetScope();
            if(serviceScope == null)
                return ReturnResult<bool>.FailResult(detailedMessage: "ServiceScope not initialized for ServiceActivator!");

            var eventRepository = serviceScope.ServiceProvider.GetRequiredService<IEventRepository>();
            if (eventRepository == null)
                return ReturnResult<bool>.FailResult(detailedMessage: "IEventRepository instance not exist in DI container!");

            var allEventsResult = await eventRepository.GetAllEvents();
            if (!allEventsResult.IsSuccessfully)
                return ReturnResult<bool>.FailResult(detailedMessage: allEventsResult.DetailedMessage);

            _eventsMap = new Dictionary<EventType, Events>();
            foreach (var events in allEventsResult.Result)
                _eventsMap.Add(events.EventCode, events);
            
            var actionRepository = serviceScope.ServiceProvider.GetRequiredService<IActionRepository>();
            if (actionRepository == null)
                return ReturnResult<bool>.FailResult(detailedMessage: "IActionRepository instance not exist in DI container!");
            
            var allActionsResult = await actionRepository.GetAllActions();
            if (!allActionsResult.IsSuccessfully)
                return ReturnResult<bool>.FailResult(detailedMessage: allActionsResult.DetailedMessage);
            
            _actionsMap = new Dictionary<ActionType, Action>();
            foreach (var action in allActionsResult.Result)
                _actionsMap.Add(action.ActionCode, action);

            return ReturnResult<bool>.SuccessResult(true);
        }
        
        public static async Task<ReturnResult<Events[]>> GetPreBuiltEvents()
        {
            if (!_isConfigured)
            {
                var configuringResult = await ConfigureStore();
                if(!configuringResult.IsSuccessfully)
                    return ReturnResult<Events[]>.FailResult(detailedMessage: configuringResult.DetailedMessage);
                
                _isConfigured = true;
            }
            return ReturnResult<Events[]>.SuccessResult(_eventsMap.Select(x => x.Value).ToArray());
        }

        public static async Task<ReturnResult<Events>> GetPreBuiltEvent(EventType eventType)
        {
            if(!_isConfigured)
            {
                var configuringResult = await ConfigureStore();
                if(!configuringResult.IsSuccessfully)
                    return ReturnResult<Events>.FailResult(detailedMessage: configuringResult.DetailedMessage);
                
                _isConfigured = true;
            }
            return ReturnResult<Events>.SuccessResult(_eventsMap.ContainsKey(eventType) ? _eventsMap[eventType] : null);
        }
        
        public static async Task<ReturnResult<Action[]>> GetPreBuiltActions()
        {
            if (!_isConfigured)
            {
                var configuringResult = await ConfigureStore();
                if(!configuringResult.IsSuccessfully)
                    return ReturnResult<Action[]>.FailResult(detailedMessage: configuringResult.DetailedMessage);
                
                _isConfigured = true;
            }
            return ReturnResult<Action[]>.SuccessResult(_actionsMap.Select(x => x.Value).ToArray());
        }

        public static async Task<ReturnResult<Action>> GetPreBuiltAction(ActionType actionType)
        {
            if(!_isConfigured)
            {
                var configuringResult = await ConfigureStore();
                if(!configuringResult.IsSuccessfully)
                    return ReturnResult<Action>.FailResult(detailedMessage: configuringResult.DetailedMessage);
                
                _isConfigured = true;
            }
            return ReturnResult<Action>.SuccessResult(_actionsMap.ContainsKey(actionType) ? _actionsMap[actionType] : null);
        }
    }
}