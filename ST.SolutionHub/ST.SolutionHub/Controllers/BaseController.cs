using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ST.SolutionHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("AllowAll")]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {

        }
    }
}
