using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppTilausDB.Models;

namespace WebAppTilausDB.Controllers
{
    public class PostitoimipaikatController : Controller
    {
        // GET: Postitoimipaikat
        TilausDBEntities db = new TilausDBEntities();
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                var ptpt = db.Postitoimipaikat;
                return View(ptpt.ToList());
            }
        }

        public ActionResult Edit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Postitoimipaikat ptpt = db.Postitoimipaikat.Find(id);
            if (ptpt == null) return HttpNotFound();
            return View(ptpt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Postinumero,Postitoimipaikka")] Postitoimipaikat ptpt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ptpt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ptpt);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Postinumero,Postitoimipaikka")] Postitoimipaikat ptpt)
        {
            if (ModelState.IsValid)
            {
                db.Postitoimipaikat.Add(ptpt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ptpt);
        }

        public ActionResult Delete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Postitoimipaikat ptpt = db.Postitoimipaikat.Find(id);
            if (ptpt == null) return HttpNotFound();
            return View(ptpt);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Postitoimipaikat ptpt = db.Postitoimipaikat.Find(id);
            db.Postitoimipaikat.Remove(ptpt);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}