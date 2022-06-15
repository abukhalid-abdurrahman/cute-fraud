using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fraud.App.Controllers
{
    [ApiController]
    [Route("api/[controller]/Scenario")]
    public class ScenarioController : ControllerBase
    {
        [HttpGet]
        public async Task GetScenario()
        {
            
        }

        [HttpPost]
        public async Task SaveScenario()
        {
            
        }

        [HttpDelete]
        public async Task DeleteScenario()
        {
            
        }
    }
}