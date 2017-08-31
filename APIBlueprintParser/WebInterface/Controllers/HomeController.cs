using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebInterface.Models;
using System.IO;
using Newtonsoft.Json;

namespace WebInterface.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var path = System.Environment.GetEnvironmentVariable("HOME") + "/APIS/.store.json";
            var readed = JsonConvert.DeserializeObject<ServiceDataSet[]>(System.IO.File.ReadAllText(path)).ToList();
            ViewData["data"] = readed.ToList();
            ViewBag.Data = readed.ToList();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
