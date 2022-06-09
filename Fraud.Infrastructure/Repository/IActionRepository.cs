using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Action = Fraud.Entities.Models.Action;

namespace Fraud.Infrastructure.Repository
{
    public interface IActionRepository : IDisposable
    {
        Task<IEnumerable<Action>> GetAllActions();
        Task<IEnumerable<Action>> GetActionById(int actionId);
    }
}