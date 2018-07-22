using ChartJs.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using MVCExample.Models;

namespace ChartJs.Controllers
{
    public class LineChartController : Controller
    {
        private static int meandata ;
        private static int totalday ;

        private DiabetEntities db = new DiabetEntities();
        // GET: LineChart
        public ActionResult Index()
        {
            //TempData["MeanValue"] = meandata;
            //TempData["Totalday"] = totalday;

            return View();
        }
        private string GetCookie(string name)
        {
            //Böyle bir cookie mevcut mu kontrol ediyoruz
            if (Request.Cookies.AllKeys.Contains(name))
            {
                //böyle bir cookie varsa bize geri değeri döndürsün
                return Request.Cookies[name].Value;
            }
            return null;
        }
        public JsonResult LineChartData()
        {
            string id = GetCookie("userid");
            int userid = int.Parse(id);
            IEnumerable<AddBloodSugar> addBloodSugars = (from s in db.AddBloodSugar where s.UserId == userid orderby s.BloodSugarEntranceDate descending select s).Take(30);
            var result = addBloodSugars.Select(s => s.BloodSugarEntranceDate);

            Chart _chart = new Chart();
            List<Datasets> _dataSet = new List<Datasets>();
            _chart.datasets = new List<Datasets>();


            int p = 30;
            int count = 0;
            if (addBloodSugars.Count() < 30) { p = addBloodSugars.Count(); }
            int[] data1 = new int[p];
            _chart.labels = new string[p];
            foreach (var k in addBloodSugars)
            {
                //if (count < addBloo)
                //{

                    data1[count] = k.BloodSugarValue;
                    _chart.labels[count] = k.BloodSugarEntranceDate.Value.Day.ToString() + "." + k.BloodSugarEntranceDate.Value.Month.ToString() + "." + k.BloodSugarEntranceDate.Value.Year.ToString() + " " + k.BloodSugarEntranceDate.Value.Hour.ToString() + ":" + k.BloodSugarEntranceDate.Value.Minute.ToString();
                    count++;
                //}
            }

            int i = 0;
            int sumdata = 0;
            while (i < count)
            {
                sumdata += data1[i];
                i++;
            }
            totalday = count;
            TempData["Totalday"] = totalday;
            meandata = sumdata / count;
            Session["meandata"] = meandata;
            TempData["MeanValue"] = meandata;
            ViewBag.MeanValue = meandata;


            _dataSet.Add(new Datasets()
            {
                label = "Current Data",
                data = data1,
                borderColor = new string[] { "#a80117" },
                borderWidth = "1"
            });
            _chart.datasets = new List<Datasets>();

            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MultiLineChartData()
        {
            Chart _chart = new Chart();
            _chart.labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "Novemeber", "December" };
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Current Year",
                data = new int[] { 28, 48, 40, 19, 86, 27, 90, 20, 45, 65, 34, 22 },
                borderColor = new string[] { "rgba(75,192,192,1)" },
                backgroundColor = new string[] { "rgba(75,192,192,0.4)" },
                borderWidth = "1"
            });
            _dataSet.Add(new Datasets()
            {
                label = "Last Year",
                data = new int[] { 65, 59, 80, 81, 56, 55, 40, 55, 66, 77, 88, 34 },
                borderColor = new string[] { "rgba(75,192,192,1)" },
                backgroundColor = new string[] { "rgba(75,192,192,0.4)" },
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
    }
}