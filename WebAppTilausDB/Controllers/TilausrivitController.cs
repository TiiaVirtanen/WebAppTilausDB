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
    public class TilausrivitController : Controller
    {
        // GET: Tilausrivit
        TilausDBEntities db = new TilausDBEntities();
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                var tilausrivit = db.Tilausrivit.Include(t => t.Tilaukset).Include(t => t.Tuotteet);
                return View(tilausrivit.ToList());
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tilausrivit tilausrivit = db.Tilausrivit.Find(id);
            if (tilausrivit == null) return HttpNotFound();
            ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "", tilausrivit.TilausID);
            ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID", "Ahinta", tilausrivit.TuoteID);
            return View(tilausrivit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "TilausriviID,TilausID,TuoteID,Määrä,Ahinta")] Tilausrivit tilausrivit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilausrivit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "", tilausrivit.TilausID);
            ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID", "Ahinta", tilausrivit.TuoteID);
            return View(tilausrivit);
        }

        public ActionResult Create()
        {
            ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID");
            ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "TuoteID,Nimi,Ahinta")] Tilausrivit tilausrivit)
        {
            if (ModelState.IsValid)
            {
                db.Tilausrivit.Add(tilausrivit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "", tilausrivit.TilausID);
            ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID", "Ahinta", tilausrivit.TuoteID);
            return View(tilausrivit);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tilausrivit tilausrivit = db.Tilausrivit.Find(id);
            if (tilausrivit == null) return HttpNotFound();
            return View(tilausrivit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)
        {
            Tilausrivit tilausrivit = db.Tilausrivit.Find(id);
            db.Tilausrivit.Remove(tilausrivit);
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
    }
}