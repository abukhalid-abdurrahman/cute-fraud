using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.DTOs;
using Fraud.Entities.DTOs.Action;
using Fraud.Entities.DTOs.Event;
using Fraud.Entities.DTOs.PreBuilt;
using Fraud.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Fraud.App.Controllers
{
    [ApiController]
    [Route("api/[controller]/PreBuiltList")]
    public class PreBuiltController : ControllerBase
    {
        [HttpGet]
        public async Task<Response<PreBuiltListDto>> GetPreBuiltList()
        {
            return new()
            {
                Code = 1000,
                Message = "Approved",
                Payload = new PreBuiltListDto()
                {
                    Actions = new List<ActionTypeDto>()
                    {
                        new()
                        {
                            ActionCode = ActionType.BlockAction
                        },
                        new()
                        {
                            ActionCode = ActionType.RequestVerification
                        },
                        new()
                        {
                            ActionCode = ActionType.TemporaryBlockAction
                        },
                    },
                    Events = new List<EventTypeDto>()
                    {
                        new()
                        {
                            EventCode = EventType.MinimumIntervalEvent
                        },
                        new()
                        {
                            EventCode = EventType.AmountAboveAverageEvent
                        },
                        new()
                        {
                            EventCode = EventType.OperationsAboveAverageEvent
                        },
                    }
                }
            };
        }
    }
}