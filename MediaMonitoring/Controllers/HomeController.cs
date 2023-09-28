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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MediaMonitoring.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index(string search3)
        {
            //get top 10 running adverts
            var topAds = db.TopCampaigns.FromSqlRaw("onl_GetCurrentCampaigns_sel").ToList().OrderByDescending(x => x.AdDate);

            var lstTopAds = new List<TopCampaignsViewModel>();
            foreach(var item in topAds)
            {
                var lstItem = new TopCampaignsViewModel();
                lstItem.AdDate = item.AdDate.ToString("dd/MM/yyyy HH:mm:ss"); //GetRelativeTime(item.AdDate);
                lstItem.Brand = item.Brand;
                lstItem.Campaign = item.Campaign;
                lstItem.Duration = item.Duration;
                lstItem.Medium = item.Medium;
                lstItem.Station = item.Station;
                lstTopAds.Add(lstItem);
            }

            ViewBag.CurrentCampaigns = lstTopAds;

            //get top advertizers
            int year = DateTime.Now.Year;

            var beginDate = new SqlParameter("@BeginDate", new DateTime(year, 1, 1));
            var endDate = new SqlParameter("@EndDate", new DateTime(year, 12, 31));

            List<TopAdvertisers2> barData = null;

            if (!string.IsNullOrEmpty(search3) && search3 != "Top Advertisers")
            {
                ViewBag.TopBar = search3;
                if(search3 == "Top Television Advertisers")
                {
                    barData = db.TopAdvertisers2s.FromSqlRaw("stp_GetMediaSpendTelevision_sel @BeginDate, @EndDate", parameters: new[] { beginDate, endDate }).ToList();
                }
                else if (search3 == "Top Radio Advertisers")
                {
                    barData = db.TopAdvertisers2s.FromSqlRaw("stp_GetMediaSpendRadio_sel @BeginDate, @EndDate", parameters: new[] { beginDate, endDate }).ToList();
                }
                else if (search3 == "Top Press Advertisers")
                {
                    barData = db.TopAdvertisers2s.FromSqlRaw("stp_GetMediaSpendPress_sel @BeginDate, @EndDate", parameters: new[] { beginDate, endDate }).ToList();
                }
                else if (search3 == "Top Outdoor Advertisers")
                {
                    barData = db.TopAdvertisers2s.FromSqlRaw("stp_GetMediaSpendOutdoor_sel @BeginDate, @EndDate", parameters: new[] { beginDate, endDate }).ToList();
                }

                ViewBag.Advertizers = barData.Take(5).Select(x => x.Advertiser).ToList();
                ViewBag.Spots = barData.Take(5).Select(x => x.Spots).ToList();
            }
            else
            {
                ViewBag.TopBar = "Top Advertisers";

                var getSpend = db.TopAdvertisers.FromSqlRaw("onl_GetMediaSpendAllMedia2_sel @BeginDate, @EndDate", parameters: new[] { beginDate, endDate }).ToList();

                ViewBag.Advertizers = getSpend.Take(5).Select(x => x.Advertiser).ToList();
                ViewBag.Spots = getSpend.Take(5).Select(x => x.Spots).ToList();
            }

            return View();
        }

        private string GetRelativeTime(DateTime AdDate)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - AdDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
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
