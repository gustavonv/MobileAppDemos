using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.AppService;
using LinqToTwitter;
using System.Threading.Tasks;

namespace MobileCloudHackDayService.Controllers
{
    public class DevTestController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/DevTest
        public async Task<string> Get()
        {
            return "";
        }
        


    }
}
