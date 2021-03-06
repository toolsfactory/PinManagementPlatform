﻿using Microsoft.AspNetCore.Mvc;

namespace PinPlatform.Services.ClientAPI.Controllers
{
    [ApiVersionNeutral]
    [ApiController]
    [Route("/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
