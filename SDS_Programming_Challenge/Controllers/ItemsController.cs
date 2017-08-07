using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SDS_Programming_Challenge.Models;

namespace SDS_Programming_Challenge.Controllers
{
    public class ItemsController : Controller
    {
        private TheInnEntities db = new TheInnEntities();

        // GET: Items
        public ActionResult Index()
        {
            return View(db.Items.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,Name,SellIn,Quality")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,Name,SellIn,Quality")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult QualityUpdate()
        {
            UpdateQuality();

            return View("Index", db.Items.ToList());
        }

        public void UpdateQuality()
        {
            List<Item> Items = db.Items.ToList();

            foreach (var item in Items)
            {
                item.SellIn = SellIn(item);

                if ((item.Quality > 0 && item.Quality < 50) ||
                    item.Name == "Aged Brie" && item.Quality < 50)
                {
                    item.Quality = Quality(item);
                }
            }
            db.SaveChanges();
        }

        public int? SellIn(Item item)
        {
            int? newItem = 0;

            switch (item.Name)
            {
                case "Sulfuras, Hand of Ragnaros":
                    newItem = item.SellIn;
                    break;
                default:
                    newItem = item.SellIn - 1;
                    break;
            }

            return newItem;
        }

        public int? Quality(Item item)
        {
            int? newValue = 0;
            switch (item.Name)
            {
                case "Aged Brie":
                    newValue = item.Quality + 1;
                    break;
                case "Sulfuras, Hand of Ragnaros":
                    newValue = item.Quality;
                    break;
                case "Backstage passes to a TAFKAL80ETC concert":
                    if (item.SellIn > 10)
                    {
                        newValue = item.Quality + 1;
                    }
                    else if (item.SellIn <= 10 && item.SellIn > 5)
                    {
                        newValue = item.Quality + 2;
                    }
                    else if (item.SellIn <= 5 && item.SellIn > 0)
                    {
                        newValue = item.Quality + 3;
                    }
                    else
                    {
                        newValue = 0;
                    }
                    break;
                case "Conjured Mana Cake":

                    item.Quality = CheckForZero(item.Quality);
                    if (item.Quality > 1)
                    {
                        newValue = item.Quality - 2;
                    }
                    break;
                default:
                    item.Quality = CheckForZero(item.Quality);
                    if (item.SellIn <= 0 && item.Quality > 1)
                    {
                        newValue = item.Quality - 2;
                    }
                    else if (item.Quality != 0)
                    {
                        newValue = item.Quality - 1;
                    }
                    break;
            }

            return newValue;
        }

        private int? CheckForZero(int? quality)
        {
            if (quality == 1)
            {
                quality = 0;
            }
            return quality;
        }

        //public void UpdateQuality()
        //{
        //    List<Item> Items = db.Items.ToList();

        //    for (var i = 0; i < Items.Count; i++)
        //    {
        //        if (Items[i].Name != "Aged Brie" && Items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
        //        {
        //            if (Items[i].Quality > 0)
        //            {
        //                if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
        //                {
        //                    Items[i].Quality = Items[i].Quality - 1;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (Items[i].Quality < 50)
        //            {
        //                Items[i].Quality = Items[i].Quality + 1;

        //                if (Items[i].Name == "Backstage passes to a TAFKAL80ETC concert")
        //                {
        //                    if (Items[i].SellIn < 11)
        //                    {
        //                        if (Items[i].Quality < 50)
        //                        {
        //                            Items[i].Quality = Items[i].Quality + 1;
        //                        }
        //                    }

        //                    if (Items[i].SellIn < 6)
        //                    {
        //                        if (Items[i].Quality < 50)
        //                        {
        //                            Items[i].Quality = Items[i].Quality + 1;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
        //        {
        //            Items[i].SellIn = Items[i].SellIn - 1;
        //        }

        //        if (Items[i].SellIn < 0)
        //        {
        //            if (Items[i].Name != "Aged Brie")
        //            {
        //                if (Items[i].Name != "Backstage passes to a TAFKAL80ETC concert")
        //                {
        //                    if (Items[i].Quality > 0)
        //                    {
        //                        if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
        //                        {
        //                            Items[i].Quality = Items[i].Quality - 1;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Items[i].Quality = Items[i].Quality - Items[i].Quality;
        //                }
        //            }
        //            else
        //            {
        //                if (Items[i].Quality < 50)
        //                {
        //                    Items[i].Quality = Items[i].Quality + 1;
        //                }
        //            }
        //        }
        //    }
        //    db.SaveChanges();
        //}
    }
}
