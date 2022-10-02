using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediaMonitoring.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;

namespace MediaMonitoring.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(IFormCollection formCollection)
        {
            var fullName = formCollection["name"].ToString();
            var email = formCollection["email"].ToString();
            var phone = formCollection["phone"].ToString();
            var company = formCollection["company"].ToString();
            var message = formCollection["message"].ToString();

            message = "Message Below From " + fullName + Environment.NewLine + " Mail: " + email + Environment.NewLine + " Phone: " + phone + Environment.NewLine + " Company: " + company + Environment.NewLine + message;
            try
            {
                using (var smtpClient = HttpContext.RequestServices.GetRequiredService<SmtpClient>())
                {
                    await smtpClient.SendMailAsync(new MailMessage("info@mediamonitoring.com.ng", "info@mediamonitoring.com.ng", "Media Monitoring Services Nig. Ltd.", message));

                }
                //mm.IsBodyHtml = true;

            }
            catch (Exception ex)
            {
                throw ex;        
            }
            return View();
        }

        public IActionResult Electronic()
        {
            return View();
        }

        public IActionResult Outdoor()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Cookies()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ExceptionPath = exceptionDetails.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            ViewBag.ExceptionStackTrace = exceptionDetails.Error.StackTrace;

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
