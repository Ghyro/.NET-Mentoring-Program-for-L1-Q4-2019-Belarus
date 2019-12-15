using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Potestas.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Redirect("http://localhost:61779/api/EnergyObservation");
        }
    }
}
