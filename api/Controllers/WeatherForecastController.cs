using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using models;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Users> Get()
        {
            IList<Users> lstUser = new List<Users>();
            
            for (int i = 0; i < 10; i++)
            {
                Users user = new Users();
                user.Name = i;
                user.Value = i;
                lstUser.Add(user);
            }
            
            return lstUser;
        }
    }
}
