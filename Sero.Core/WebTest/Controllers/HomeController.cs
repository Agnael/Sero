using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sero.Core;
using WebTest.Models;

namespace WebTest.Controllers
{
    public class HomeController : Controller
    {
        public readonly IAppInfoService AppInfoService;
        public readonly IRequestInfoService RequestInfoService;

        public HomeController(IAppInfoService appInfoService, IRequestInfoService reqInfoService)
        {
            this.AppInfoService = appInfoService;
            this.RequestInfoService = reqInfoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromBody] TestPostModel model)
        {
            string reqBody = null;

            try
            {
                reqBody = RequestInfoService.RequestBody;
            }
            catch(Exception ex)
            {

            }

            return View();
        }
    }
}
