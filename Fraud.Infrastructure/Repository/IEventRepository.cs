using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.Models;
namespace Fraud.Infrastructure.Repository
{
    public interface IEventRepository : IDisposable
    {
        Task<IEnumerable<Events>> GetAllEvents();
        Task<IEnumerable<Events>> GetEventById(int eventId);
    }
}