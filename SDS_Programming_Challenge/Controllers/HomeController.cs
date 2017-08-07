using SDS_Programming_Challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDS_Programming_Challenge.Controllers
{
    public class HomeController : Controller
    {
        private TheInnEntities db = new TheInnEntities();
        public ActionResult Index()
        {
            InitializeDatabase();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "We value your service as an employee.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Website refreshed by Brandon Roberts.";

            return View();
        }

        public void InitializeDatabase()
        {
            List<Item> ExistingItems = db.Items.ToList();

            for (int i = 0; i < ExistingItems.Count; i++)
            {
                db.Items.Remove(ExistingItems[i]);
                db.SaveChanges();
            }

            IList<Item> Items = new List<Item>
           {
                new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 },
                new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 },
                new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 },
                new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 },
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 },
                new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 }
            };

            for (int i = 0; i < Items.Count; i++)
            {
                db.Items.Add(Items[i]);
                db.SaveChanges();
            }
        }
    }
}