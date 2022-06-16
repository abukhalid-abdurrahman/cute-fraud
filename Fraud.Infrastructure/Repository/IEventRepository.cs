using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;
namespace Fraud.Infrastructure.Repository
{
    public interface IEventRepository : IDisposable
    {
        Task<ReturnResult<IEnumerable<Events>>> GetAllEvents();
        Task<ReturnResult<IEnumerable<Events>>> GetEventById(int eventId);
    }
}