using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using twright_bugtracker.Models;
using twright_bugtracker.ViewModels;

namespace twright_bugtracker.Controllers
{
    public class ChartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChartData
        public JsonResult GetTicketTypeChartData()
        {
            var ticketTypes = db.TicketTypes.ToList();
            var chartData = new BarChartData();
            foreach (var type in ticketTypes)
            {
                chartData.Labels.Add(type.Name);
                chartData.Data.Add(db.Tickets.Where(t => t.TicketTypeId == type.Id).Count());
            }

            return Json(chartData);
        }
    }
}