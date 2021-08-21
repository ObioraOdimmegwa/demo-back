using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server
{
    [ApiController, Route("/test")]
    public class TestController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Json(new {
                Message = "WELCOME"
            });
        }
    }
}