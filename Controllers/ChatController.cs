using Microsoft.AspNetCore.Mvc;

namespace SignalR1.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
