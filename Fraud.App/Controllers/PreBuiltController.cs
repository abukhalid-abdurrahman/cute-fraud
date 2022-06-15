using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fraud.App.Controllers
{
    [ApiController]
    [Route("api/[controller]/PreBuiltList")]
    public class PreBuiltController : ControllerBase
    {
        [HttpGet]
        public async Task GetPreBuiltList()
        {
            
        }
    }
}