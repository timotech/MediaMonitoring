using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediaMonitoring.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace MediaMonitoring.Controllers
{
    [Authorize]
    public class ElectronicController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;

        public ElectronicController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
            ViewBag.User = userDetails.FirstName;

            int year = DateTime.Now.Year;

            var beginDate = new SqlParameter("@BeginDate", new DateTime(year, 1, 1));
            var endDate = new SqlParameter("@EndDate", new DateTime(year, 12, 31));

            var getSpend = db.TopAdvertisers.FromSqlRaw("stp_GetMediaSpendAllMedia_sel @BeginDate, @EndDate", parameters: new[] { beginDate, endDate }).ToList();

            var spendTotal = 0.00;
            var spend = new List<TopAdvertisersDTO>();
            foreach (var item in getSpend.Take(10))
            {
                var spendItem = new TopAdvertisersDTO();
                spendItem.Value = FormatNumber(Convert.ToInt64(item.Value));
                spendItem.Advertiser = item.Advertiser;
                spend.Add(spendItem);
                spendTotal += item.Value;
            }

            ViewBag.TotalSpend = FormatNumber(Convert.ToInt64(spendTotal));
            ViewBag.TopSpends = spend;
            ViewBag.Advertizers = getSpend.Take(10).Select(x => x.Advertiser).ToList();
            ViewBag.Spots = getSpend.Take(10).Select(x => x.Spots).ToList();

            var product = new SqlParameter("@ProductId", userDetails.CategoryId);
            var getBrands = db.TopBrandsInCategories.FromSqlRaw("onl_GetMediaSpendAllMediaByBrandAndCategory_sel @BeginDate, @EndDate, @ProductId", parameters: new[] { beginDate, endDate, product }).ToList().Take(5);

            foreach (var item in getBrands)
            {
                if (item.Brand == "MTN")
                {
                    item.Style = "bg-four";
                    item.Colors = "#f2fc04";
                }                    
                else if (item.Brand == "AIRTEL")
                {
                    item.Style = "bg-two";
                    item.Colors = "#C60607";
                }                    
                else if (item.Brand == "GLOBACOM")
                {
                    item.Style = "bg-one";
                    item.Colors = "#2FAB26";
                }                    
                else if (item.Brand == "NTEL")
                {
                    item.Style = "bg-three";
                    item.Colors = "#FF5252";
                }                    
                else if (item.Brand == "9MOBILE")
                {
                    item.Style = "bg-five";
                    item.Colors = "#62e611";
                }                    
            }

            ViewBag.Brands = getBrands.Select(x => x.Brand).ToList();
            ViewBag.Values = getBrands.Select(x => x.Value).ToList();

            ViewBag.CategorySpend = getBrands;
            return View();
        }

        [HttpGet]
        public IActionResult GetAdvertizerSpend(string selection)
        {
            DateTime date = new DateTime();
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            int year = DateTime.Now.Year;
            SqlParameter beginDate;
            SqlParameter endDate;

            if(selection == "This Month")
            {
                beginDate = new SqlParameter("@BeginDate", firstDayOfMonth);
                endDate = new SqlParameter("@EndDate", firstDayOfMonth.AddMonths(1).AddDays(-1));
            } 
            else if(selection == "Last Month")
            {
                beginDate = new SqlParameter("@BeginDate", firstDayOfMonth.AddMonths(-1));
                endDate = new SqlParameter("@EndDate", firstDayOfMonth.AddDays(-1));
            }
            else
            {
                beginDate = new SqlParameter("@BeginDate", new DateTime(year, 1, 1));
                endDate = new SqlParameter("@EndDate", new DateTime(year, 12, 31));
            }

            
            var getSpend = db.TopAdvertisers.FromSqlRaw("stp_GetMediaSpendAllMedia_sel @BeginDate, @EndDate", parameters: new[] { beginDate, endDate }).ToList();

            var spend = new List<TopAdvertisersDTO>();
            foreach (var item in getSpend.Take(10))
            {
                var spendItem = new TopAdvertisersDTO();
                spendItem.Value = FormatNumber(Convert.ToInt64(item.Value));
                spendItem.Advertiser = item.Advertiser;
                spend.Add(spendItem);
            }

            return Json(spend);
        }

        private static string FormatNumber(long num)
        {
            // Ensure number has max 3 significant digits (no rounding up can happen)
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;

            if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + "B";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + "M";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + "K";

            return num.ToString("#,0");
        }

        [Authorize(Roles = "Administrators, Electronics, Watchdog")]
        [HttpGet]
        public async Task<IActionResult> WatchDog()
        {
            var model = new ReportsViewModel();

            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            var brandsArr = userDetails.Brand.Split(",");

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId && brandsArr.Contains(x.BrandId) && x.FK_ProductId == userDetails.CategoryId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();

            return View(model);
        }

        [Authorize(Roles = "Administrators, Electronics, Watchdog")]
        [HttpPost]
        public async Task<IActionResult> WatchDog(ReportsViewModel model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                string brands = formCollection["brand"];
                string[] brandsSplit = brands.Split(" ");

                model.Brands = brandsSplit[1];
                model.Identifiers = formCollection["identifier"];
                model.Stations = formCollection["station"];
                if (string.IsNullOrEmpty(model.Operation))
                    model.Operation = "Compliance";

                return RedirectToAction("WatchDogReport", model);
            }

            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();

            return View(model);
        }

        [Authorize(Roles = "Administrators, Electronics, Watchdog")]
        [HttpGet]
        public async Task<IActionResult> WatchDogReport(ReportsViewModel model)
        {
            var allModels = new WatchDogViewModel();

            try
            {
                var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
                var companyId = new SqlParameter("@AdvertizerId", userDetails.CompanyId);
                var beginDate = new SqlParameter("@BeginDate", model.BeginDate);
                var endDate = new SqlParameter("@EndDate", model.EndDate);
                var zone = new SqlParameter("@Zone", model.Zone);
                var station = new SqlParameter("@Station", model.Stations);
                var brand = new SqlParameter("@Brand", model.Brands);
                var identifiers = new SqlParameter("@Identifier", model.Identifiers);

                if (model.Operation == "Compliance")
                {
                    if (model.MediaType == "Television")
                    {
                        var getSpend = await db.WatchDogs.FromSqlRaw("onl_GetWatchDogReport_sel @AdvertizerId, @BeginDate, @EndDate, @Zone, @Station, @Brand, @Identifier", parameters: new[] { companyId, beginDate, endDate, zone, station, brand, identifiers }).ToListAsync();

                        allModels.WatchDogs = getSpend;
                    }
                    else if (model.MediaType == "Radio")
                    {
                        var getSpend = await db.WatchDogRadios.FromSqlRaw("onl_GetWatchDogRadioReport_sel @AdvertizerId, @BeginDate, @EndDate, @Zone, @Station, @Brand, @Identifier", parameters: new[] { companyId, beginDate, endDate, zone, station, brand, identifiers }).ToListAsync();

                        allModels.WatchDogRadios = getSpend;
                    }
                    else if (model.MediaType == "Press")
                    {
                        var getSpend = await db.WatchDogPresses.FromSqlRaw("onl_GetWatchDogPressReport_sel @AdvertizerId, @BeginDate, @EndDate, @Zone, @Brand, @Identifier", parameters: new[] { companyId, beginDate, endDate, zone, brand, identifiers }).ToListAsync();

                        allModels.WatchDogPresses = getSpend;
                    }
                    else if (model.MediaType == "Outdoor")
                    {
                        var getSpend = await db.WatchDogOutdoors.FromSqlRaw("onl_GetWatchDogOutdoorReport_sel @AdvertizerId, @BeginDate, @EndDate, @Zone, @Brand, @Identifier", parameters: new[] { companyId, beginDate, endDate, zone, brand, identifiers }).ToListAsync();

                        allModels.WatchDogOutdoors = getSpend;
                    }
                }
                else if (model.Operation == "Summary")
                {
                    if (model.MediaType == "Television")
                    {
                        var getSpend = await db.WatchDogSummaries.FromSqlRaw("onl_GetWatchDogReportSummaryTelevision_sel @AdvertizerId, @BeginDate, @EndDate, @Zone, @Brand, @Identifier", parameters: new[] { companyId, beginDate, endDate, zone, brand, identifiers }).ToListAsync();

                        allModels.WatchDogSummaries = getSpend;
                    }
                    else if (model.MediaType == "Radio")
                    {
                        var getSpend = await db.WatchDogSummaries.FromSqlRaw("onl_GetWatchDogReportSummaryRadio_sel @AdvertizerId, @BeginDate, @EndDate, @Zone, @Brand, @Identifier", parameters: new[] { companyId, beginDate, endDate, zone, brand, identifiers }).ToListAsync();

                        allModels.WatchDogSummaries = getSpend;
                    }
                    else if (model.MediaType == "Press")
                    {
                        var getSpend = await db.WatchDogSummaryPresses.FromSqlRaw("onl_GetWatchDogReportSummaryPress_sel @AdvertizerId, @BeginDate, @EndDate, @Zone, @Brand, @Identifier", parameters: new[] { companyId, beginDate, endDate, zone, brand, identifiers }).ToListAsync();

                        allModels.WatchDogSummaryPresses = getSpend;
                    }
                    else if (model.MediaType == "Outdoor")
                    {
                        var getSpend = await db.watchDogSummaryOutdoors.FromSqlRaw("onl_GetWatchDogReportSummaryOutdoor_sel @AdvertizerId, @BeginDate, @EndDate, @Zone, @Brand, @Identifier", parameters: new[] { companyId, beginDate, endDate, zone, brand, identifiers }).ToListAsync();

                        allModels.WatchDogSummaryOutdoors = getSpend;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return View(allModels);
        }

        [Authorize(Roles = "Administrators, Electronics, AdCluster")]
        [HttpGet]
        public async Task<IActionResult> Adcluster()
        {
            var model = new ReportsViewModel();

            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();

            return View(model);
        }

        [Authorize(Roles = "Administrators, Electronics, AdCluster")]
        [HttpPost]
        public async Task<IActionResult> Adcluster(ReportsViewModel model, IFormCollection formCollection)
        {
            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                return RedirectToAction("AdclusterReport", model);
            }
            
            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();
            return View();
        }

        [Authorize(Roles = "Administrators, Electronics, AdCluster")]
        [HttpGet]
        public async Task<IActionResult> AdclusterReport(ReportsViewModel model)
        {
            var allModels = new AdClusterViewModel();
            try
            {
                var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
                var productId = new SqlParameter("@ProductId", userDetails.CategoryId);
                var beginDate = new SqlParameter("@BeginDate", model.BeginDate);
                var endDate = new SqlParameter("@EndDate", model.EndDate);
                var zone = new SqlParameter("@Zone", model.Zone);
                //var station = new SqlParameter("@Station", model.Stations);
                //var brand = new SqlParameter("@Brand", userDetails.Brand);

                if (model.Operation == "Daily")
                {
                    if (model.MediaType == "All Media")
                    {
                        var getSpend = await db.AdClusterAllMedias.FromSqlRaw("onl_GetAdClusterAllMediumReportByTimeSegment_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterAllMedias = getSpend;
                    }
                    else if (model.MediaType == "Television")
                    {
                        var getSpend = await db.AdClusters.FromSqlRaw("onl_GetAdClusterReportTvByDays_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusters = getSpend;
                    }
                    else if (model.MediaType == "Radio")
                    {
                        var getSpend = await db.AdClusters.FromSqlRaw("onl_GetAdClusterReportRadioByDays_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusters = getSpend;
                    }
                    else if (model.MediaType == "Press")
                    {
                        var getSpend = await db.AdClusterPresses.FromSqlRaw("onl_GetAdClusterPressReportByDays_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterPresses = getSpend;
                    }
                    else if (model.MediaType == "Outdoor")
                    {
                        var getSpend = await db.AdClusterOutdoors.FromSqlRaw("onl_GetAdClusterOutdoorReportByDays_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterOutdoors = getSpend;
                    }
                }
                else if (model.Operation == "Weekly")
                {
                    if (model.MediaType == "All Media")
                    {
                        var getSpend = await db.AdClusterAllMedias.FromSqlRaw("onl_GetAdClusterAllMediumReportByTimeSegment_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterAllMedias = getSpend;
                    }
                    else if (model.MediaType == "Television")
                    {
                        var getSpend = await db.AdClusterByWeeks.FromSqlRaw("onl_GetAdClusterTelevisionReportByWeeks_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByWeeks = getSpend;
                    }
                    else if (model.MediaType == "Radio")
                    {
                        var getSpend = await db.AdClusterByWeeks.FromSqlRaw("onl_GetAdClusterRadioReportByWeeks_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByWeeks = getSpend;
                    }
                    else if (model.MediaType == "Press")
                    {
                        var getSpend = await db.AdClusterByWeekPresses.FromSqlRaw("onl_GetAdClusterPressReportByWeeks_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByWeekPresses = getSpend;
                    }
                    else if (model.MediaType == "Outdoor")
                    {
                        var getSpend = await db.AdClusterByWeekOutdoors.FromSqlRaw("onl_GetAdClusterOutdoorReportByWeeks_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByWeekOutdoors = getSpend;
                    }
                }
                else if (model.Operation == "Monthly")
                {
                    if (model.MediaType == "All Media")
                    {
                        var getSpend = await db.AdClusterAllMedias.FromSqlRaw("onl_GetAdClusterAllMediumReportByTimeSegment_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterAllMedias = getSpend;
                    }
                    else if (model.MediaType == "Television")
                    {
                        var getSpend = await db.AdClusterByMonths.FromSqlRaw("onl_GetAdClusterTelevisionReportByMonths_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByMonths = getSpend;
                    }
                    else if (model.MediaType == "Radio")
                    {
                        var getSpend = await db.AdClusterByMonths.FromSqlRaw("onl_GetAdClusterRadioReportByMonths_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByMonths = getSpend;
                    }
                    else if (model.MediaType == "Press")
                    {
                        var getSpend = await db.AdClusterByMonthPresses.FromSqlRaw("onl_GetAdClusterPressReportByMonths_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByMonthPresses = getSpend;
                    }
                    else if (model.MediaType == "Outdoor")
                    {
                        var getSpend = await db.AdClusterByMonthOutdoors.FromSqlRaw("onl_GetAdClusterOutdoorReportByMonths_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByMonthOutdoors = getSpend;
                    }
                }
                else if (model.Operation == "AdvertDuration")
                {
                    if (model.MediaType == "All Media")
                    {
                        var getSpend = await db.AdClusterAllMedias.FromSqlRaw("onl_GetAdClusterAllMediumReportByTimeSegment_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterAllMedias = getSpend;
                    }
                    else if (model.MediaType == "Television")
                    {
                        var getSpend = await db.AdClusterByMaterialLengths.FromSqlRaw("onl_GetAdClusterTelevisionReportByMaterialLength_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByMaterialLengths = getSpend;
                    }
                    else if (model.MediaType == "Radio")
                    {
                        var getSpend = await db.AdClusterByMaterialLengths.FromSqlRaw("onl_GetAdClusterRadioReportByMaterialLength_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByMaterialLengths = getSpend;
                    }
                    else if (model.MediaType == "Press")
                    {
                        var getSpend = await db.AdClusterByMaterialLengthPresses.FromSqlRaw("onl_GetAdClusterPressReportByMaterialLength_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByMaterialLengthPresses = getSpend;
                    }
                    else if (model.MediaType == "Outdoor")
                    {
                        var getSpend = await db.AdClusterByMaterialLengthOutdoors.FromSqlRaw("onl_GetAdClusterOutdoorReportByMaterialLength_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByMaterialLengthOutdoors = getSpend;
                    }
                }
                else if (model.Operation == "TimeDuration")
                {
                    if (model.MediaType == "All Media")
                    {
                        var getSpend = await db.AdClusterAllMedias.FromSqlRaw("onl_GetAdClusterAllMediumReportByTimeSegment_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterAllMedias = getSpend;
                    }
                    else if (model.MediaType == "Television")
                    {
                        var getSpend = await db.AdClusterByTimeSegments.FromSqlRaw("onl_GetAdClusterTelevisionReportByTimeSegment_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByTimeSegments = getSpend;
                    }
                    else if (model.MediaType == "Radio")
                    {
                        var getSpend = await db.AdClusterByTimeSegments.FromSqlRaw("onl_GetAdClusterRadioReportByTimeSegment_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByTimeSegments = getSpend;
                    }
                    else if (model.MediaType == "Press")
                    {
                        var getSpend = await db.AdClusterByTimeSegmentPresses.FromSqlRaw("onl_GetAdClusterPressReportByTimeSegment_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByTimeSegmentPresses = getSpend;
                    }
                    else if (model.MediaType == "Outdoor")
                    {
                        var getSpend = await db.AdClusterByTimeSegmentOutdoors.FromSqlRaw("onl_GetAdClusterOutdoorReportByTimeSegment_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                        allModels.AdClusterByTimeSegmentOutdoors = getSpend;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(allModels);
        }

        [Authorize(Roles = "Administrators, Electronics, MediaSpend")]
        [HttpGet]
        public async Task<IActionResult> MediaSpend()
        {
            var model = new ReportsViewModel();

            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();

            return View(model);
        }

        [Authorize(Roles = "Administrators, Electronics, MediaSpend")]
        [HttpPost]
        public async Task<IActionResult> MediaSpend(ReportsViewModel model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("MediaSpendReport", model);
            }

            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();
            return View();
        }

        [Authorize(Roles = "Administrators, Electronics, MediaSpend")]
        [HttpGet]
        public async Task<IActionResult> MediaSpendReport(ReportsViewModel model)
        {
            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
            var productId = new SqlParameter("@ProductId", userDetails.CategoryId);
            var beginDate = new SqlParameter("@BeginDate", model.BeginDate);
            var endDate = new SqlParameter("@EndDate", model.EndDate);
            var zone = new SqlParameter("@Zone", model.Zone);
            //var station = new SqlParameter("@Station", model.Stations);
            //var brand = new SqlParameter("@Brand", model.Brands);
            var allModels = new MediaSpendViewModel();

            try
            {
                if (model.Operation == "TopAdvertisers")
                {
                    if (model.MediaType == "All Media")
                    {
                        var getSpend = await db.MediaSpends.FromSqlRaw("onl_GetMediaSpendAllMedia_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpends = getSpend;
                    }
                    else if (model.MediaType == "Television")
                    {
                        var getSpend = await db.MediaSpends.FromSqlRaw("onl_GetMediaSpendTelevision_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpends = getSpend;
                    }
                    else if (model.MediaType == "Radio")
                    {
                        var getSpend = await db.MediaSpends.FromSqlRaw("onl_GetMediaSpendRadio_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpends = getSpend;
                    }
                    else if (model.MediaType == "Press")
                    {
                        var getSpend = await db.MediaSpends.FromSqlRaw("onl_GetMediaSpendPress_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpends = getSpend;
                    }
                    else if (model.MediaType == "Outdoor")
                    {
                        var getSpend = await db.MediaSpends.FromSqlRaw("onl_GetMediaSpendOutdoor_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpends = getSpend;
                    }
                }
                else if (model.Operation == "TopBrands")
                {
                    if (model.MediaType == "All Media")
                    {
                        var getSpend = await db.MediaSpendByBrands.FromSqlRaw("onl_GetMediaSpendAllMediaByBrand_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpendByBrands = getSpend;
                    }
                    else if (model.MediaType == "Television")
                    {
                        var getSpend = await db.MediaSpendByBrands.FromSqlRaw("onl_GetMediaSpendTelevisionByBrand_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpendByBrands = getSpend;
                    }
                    else if (model.MediaType == "Radio")
                    {
                        var getSpend = await db.MediaSpendByBrands.FromSqlRaw("onl_GetMediaSpendRadioByBrand_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpendByBrands = getSpend;
                    }
                    else if (model.MediaType == "Press")
                    {
                        var getSpend = await db.MediaSpendByBrands.FromSqlRaw("onl_GetMediaSpendPressByBrand_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpendByBrands = getSpend;
                    }
                    else if (model.MediaType == "Outdoor")
                    {
                        var getSpend = await db.MediaSpendByBrands.FromSqlRaw("onl_GetMediaSpendOutdoorByBrand_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpendByBrands = getSpend;
                    }
                }
                else if (model.Operation == "TopProducts")
                {
                    if (model.MediaType == "All Media")
                    {
                        var getSpend = await db.MediaSpendByProducts.FromSqlRaw("onl_GetMediaSpendAllMediaByProduct_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpendByProducts = getSpend;
                    }
                    else if (model.MediaType == "Television")
                    {
                        var getSpend = await db.MediaSpendByProducts.FromSqlRaw("onl_GetMediaSpendTelevisionByProduct_sel  @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpendByProducts = getSpend;
                    }
                    else if (model.MediaType == "Radio")
                    {
                        var getSpend = await db.MediaSpendByProducts.FromSqlRaw("onl_GetMediaSpendRadioByProduct_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpendByProducts = getSpend;
                    }
                    else if (model.MediaType == "Press")
                    {
                        var getSpend = await db.MediaSpendByProducts.FromSqlRaw("onl_GetMediaSpendPressByProduct_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpendByProducts = getSpend;
                    }
                    else if (model.MediaType == "Outdoor")
                    {
                        var getSpend = await db.MediaSpendByProducts.FromSqlRaw("onl_GetMediaSpendOutdoorByProduct_sel @BeginDate, @EndDate, @Zone", parameters: new[] { beginDate, endDate, zone }).ToListAsync();

                        allModels.MediaSpendByProducts = getSpend;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(allModels);
        }

        [Authorize(Roles = "Administrators, Electronics, ShareOfVoice")]
        [HttpGet]
        public async Task<IActionResult> Competitive()
        {
            var model = new ReportsViewModel();

            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();

            return View(model);
        }

        [Authorize(Roles = "Administrators, Electronics, ShareOfVoice")]
        [HttpPost]
        public async Task<IActionResult> Competitive(ReportsViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("CompetitiveReport", model);
            }
            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();
            return View(model);
        }

        [Authorize(Roles = "Administrators, Electronics, ShareOfVoice")]
        [HttpGet]
        public async Task<IActionResult> CompetitiveReport(ReportsViewModel model)
        {
            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
            var productId = new SqlParameter("@ProductId", userDetails.CategoryId);
            var beginDate = new SqlParameter("@BeginDate", model.BeginDate);
            var endDate = new SqlParameter("@EndDate", model.EndDate);
            var zone = new SqlParameter("@Zone", model.Zone);
            //var station = new SqlParameter("@Station", model.Stations);
            //var brand = new SqlParameter("@Brand", model.Brands);

            var allModels = new ShareOfVoiceViewModel();

            if (model.Operation == "SOE")
            {
                if (model.MediaType == "All Media")
                {
                    var getSpend = await db.ShareOfExpeditureAllMedias.FromSqlRaw("onl_GetShareOfVoiceReportByValueByAllMedia_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfExpeditureAllMedias = getSpend;
                }
                else if (model.MediaType == "Television")
                {
                    var getSpend = await db.ShareOfExpenditures.FromSqlRaw("onl_GetShareOfVoiceReportByValueTelevision_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfExpenditures = getSpend;
                }
                else if (model.MediaType == "Radio")
                {
                    var getSpend = await db.ShareOfExpenditures.FromSqlRaw("onl_GetShareOfVoiceReportByValueRadio_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfExpenditures = getSpend;
                }
                else if (model.MediaType == "Press")
                {
                    var getSpend = await db.ShareOfExpenditures.FromSqlRaw("onl_GetShareOfVoiceReportByValuePress_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfExpenditures = getSpend;
                }
                else if (model.MediaType == "Outdoor")
                {
                    var getSpend = await db.ShareOfExpenditures.FromSqlRaw("onl_GetShareOfVoiceReportByValueOutdoor_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfExpenditures = getSpend;

                }
            }
            else if (model.Operation == "SOV")
            {
                if (model.MediaType == "All Media")
                {
                    var getSpend = await db.ShareOfVoiceAllMedias.FromSqlRaw("onl_GetShareOfVoiceReportBySpotsByAllMedia_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfVoiceAllMedias = getSpend;
                }
                else if (model.MediaType == "Television")
                {
                    var getSpend = await db.ShareOfVoices.FromSqlRaw("onl_GetShareOfVoiceReportBySpotsTelevision_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfVoices = getSpend;
                }
                else if (model.MediaType == "Radio")
                {
                    var getSpend = await db.ShareOfVoices.FromSqlRaw("onl_GetShareOfVoiceReportBySpotsRadio_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfVoices = getSpend;
                }
                else if (model.MediaType == "Press")
                {
                    var getSpend = await db.ShareOfVoices.FromSqlRaw("onl_GetShareOfVoiceReportBySpotsPress_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfVoices = getSpend;
                }
                else if (model.MediaType == "Outdoor")
                {
                    var getSpend = await db.ShareOfVoices.FromSqlRaw("onl_GetShareOfVoiceReportBySpotsOutdoor_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfVoices = getSpend;
                }
            }
            else if (model.Operation == "Competitive")
            {
                if (model.MediaType == "All Media")
                {
                    var getSpend = await db.ShareOfCompetitiveWatchDogs.FromSqlRaw("onl_GetAdClusterReportTvByDays_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfCompetitiveWatchDogs = getSpend;
                }
                else if (model.MediaType == "Television")
                {
                    var getSpend = await db.ShareOfCompetitiveWatchDogs.FromSqlRaw("onl_GetAdClusterReportTvByDays_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfCompetitiveWatchDogs = getSpend;
                }
                else if (model.MediaType == "Radio")
                {
                    var getSpend = await db.ShareOfCompetitiveWatchDogs.FromSqlRaw("onl_GetAdClusterReportTvByDays_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfCompetitiveWatchDogs = getSpend;
                }
                else if (model.MediaType == "Press")
                {
                    var getSpend = await db.ShareOfCompetitiveWatchDogs.FromSqlRaw("onl_GetAdClusterReportTvByDays_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfCompetitiveWatchDogs = getSpend;
                }
                else if (model.MediaType == "Outdoor")
                {
                    var getSpend = await db.ShareOfCompetitiveWatchDogs.FromSqlRaw("onl_GetAdClusterReportTvByDays_sel @ProductId, @BeginDate, @EndDate, @Zone", parameters: new[] { productId, beginDate, endDate, zone }).ToListAsync();

                    allModels.ShareOfCompetitiveWatchDogs = getSpend;
                }
            }

            return View(allModels);
        }

        [HttpGet]
        public IActionResult GetIdentifiers(string brandId, string productId)
        {
            if (!string.IsNullOrEmpty(brandId) && !string.IsNullOrEmpty(productId))
            {
                var depts = db.Identifiers
                .Where(x => x.FK_ProductId == productId && x.FK_BrandId == brandId)
                .OrderBy(x => x.Description)
                .ToList();
                return Json(depts);
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetStations(string medium)
        {
            if (medium == "Television")
            {
                var depts = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();
                return Json(depts);
            }
            else if (medium == "Radio")
            {
                var depts = await db.Stations.Where(x => x.StationId.StartsWith("B")).OrderBy(x => x.Description).ToListAsync();
                return Json(depts);
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> StationAudit()
        {
            var model = new ReportsViewModel();

            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> StationAudit(ReportsViewModel model)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("StationAuditReport", model);
            }

            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> StationAuditReport(ReportsViewModel model)
        {
            var allModels = new WatchDogViewModel();

            try
            {
                var beginDate = new SqlParameter("@BeginDate", model.BeginDate);
                var endDate = new SqlParameter("@EndDate", model.EndDate);
                var zone = new SqlParameter("@Zone", model.Zone);
                var station = new SqlParameter("@Station", model.Stations);
                var brand = new SqlParameter("@Brand", model.Brands);

                if (model.MediaType == "Television")
                {
                    var getSpend = await db.WatchDogStationAudits.FromSqlRaw("onl_GetWatchDogReportTelevisionStationAudit_sel @BeginDate, @EndDate, @Zone, @Station", parameters: new[] { beginDate, endDate, zone, station }).ToListAsync();

                    allModels.WatchDogStationAudits = getSpend;
                }
                else if (model.MediaType == "Radio")
                {
                    var getSpend = await db.WatchDogStationAuditRadios.FromSqlRaw("onl_GetWatchDogReportRadioStationAudit_sel @BeginDate, @EndDate, @Zone, @Station", parameters: new[] { beginDate, endDate, zone, station }).ToListAsync();

                    allModels.WatchDogStationAuditRadios = getSpend;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reconciliation()
        {
            var model = new ReportsViewModel();

            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Reconciliation(ReportsViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("ReconciliationReport", model);
            }

            var userDetails = await db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();

            ViewBag.Brands = await db.Brands.Where(x => x.FK_AdvertizerId == userDetails.CompanyId).OrderBy(x => x.Description).ToListAsync();
            ViewBag.Stations = await db.Stations.Where(x => x.StationId.StartsWith("A")).OrderBy(x => x.Description).ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ReconciliationReport(ReportsViewModel model)
        {
            var allModels = new WatchDogViewModel();

            try
            {
                var beginDate = new SqlParameter("@BeginDate", model.BeginDate);
                var endDate = new SqlParameter("@EndDate", model.EndDate);
                var zone = new SqlParameter("@Zone", model.Zone);
                var station = new SqlParameter("@Station", model.Stations);
                var brand = new SqlParameter("@Brand", model.Brands);

                if (model.MediaType == "Television")
                {
                    var getSpend = await db.WatchDogStationAudits.FromSqlRaw("onl_GetWatchDogReportTelevisionStationAudit_sel @BeginDate, @EndDate, @Zone, @Station", parameters: new[] { beginDate, endDate, zone, station }).ToListAsync();

                    allModels.WatchDogStationAudits = getSpend;
                }
                else if (model.MediaType == "Radio")
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

            return View();
        }

        [HttpPost]
        public IActionResult Export(string GridHtml, string operation)
        {
            return File(System.Text.Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", operation + ".xls");
        }

        //[HttpPost]
        //public IActionResult Export(string operation)
        //{
        //    List<string> columns = new List<string>();
        //    //DataTable table = ToDataTable(model);
        //    DataTable table = TempData["Result"] as DataTable;

        //    //Create new Excel Workbook
        //    var workbook = new HSSFWorkbook();

        //    //Create new Excel Sheet
        //    var sheet = workbook.CreateSheet();
        //    IRow row = sheet.CreateRow(0);
        //    int columnIndex = 0;

        //    //Create a header row
        //    foreach (DataColumn column in table.Columns)
        //    {
        //        columns.Add(column.ColumnName);
        //        row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
        //        columnIndex++;
        //    }

        //    //(Optional) freeze the header row so it is not scrolled
        //    sheet.CreateFreezePane(0, 1, 0, 1);

        //    //Populate the sheet with values from the grid data

        //    int rowIndex = 1;
        //    foreach (DataRow dsrow in table.Rows)
        //    {
        //        row = sheet.CreateRow(rowIndex);
        //        int cellIndex = 0;
        //        foreach (String col in columns)
        //        {
        //            row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
        //            cellIndex++;
        //        }

        //        rowIndex++;
        //    }

        //    //Write the Workbook to a memory stream
        //    MemoryStream output = new MemoryStream();
        //    workbook.Write(output);

        //    //Return the result to the end user
        //    return File(output.ToArray(),   //The binary data of the XLS file
        //        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //    operation + "_Report_" + DateTime.Now.ToString() + ".xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        //}

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            //put a breakpoint here and check datatable
            return dataTable;
        }

        static DataTable ConvertListToDataTable(dynamic list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }

            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }
    }
}
