using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebApp.Controllers
{
    public class TestController : Controller
    {
        private readonly IToastNotification _notification;

        public TestController(IToastNotification notification)
        {
            _notification = notification;
        }

        public IActionResult Index()
        {
            _notification.AddSuccessToastMessage("Success", new ToastrOptions
            {
                ProgressBar = true,
                Title = "Success Bro",
                PositionClass = ToastPositions.TopRight,
                HideEasing = "swing"
            });
            return View();
        }
    }
}
