using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BakeryAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpController : ControllerBase
    {
        
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "LoggedIn!!!!!";
        }

        [HttpPost]
        public ActionResult<string> Post()
        {
            return "LoggedIn!!!!!";
        }

    }
}