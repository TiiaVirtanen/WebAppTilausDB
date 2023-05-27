using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppTilausDB.Models;

namespace WebAppTilausDB.Controllers
{
    public class TilauksetController : Controller
    {
        // GET: Tilaukset
        TilausDBEntities db = new TilausDBEntities();
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                var tilaukset = db.Tilaukset.Include(t => t.Tilausrivit).Include(t => t.Asiakkaat).Include(t => t.Postitoimipaikat);
                return View(tilaukset.ToList());
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null) return HttpNotFound();
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero", tilaukset.Postinumero);
            return View(tilaukset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilaukset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TilausID = new SelectList(db.Tilausrivit, "TilausID", "TilausID", tilaukset.TilausID);
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "AsiakasID", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero", tilaukset.Postinumero);
            return View(tilaukset);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            return View(tilaukset);
        }

        public ActionResult Create()
        {
            ViewBag.TilausID = new SelectList(db.Tilausrivit, "TilausID", "TilausID");
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "AsiakasID");
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Tilaukset.Add(tilaukset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TilausID = new SelectList(db.Tilausrivit, "TilausID", "TilausID");
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "AsiakasID");
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero");
            return View(tilaukset);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null) return HttpNotFound();
            return View(tilaukset);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)
        {
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            db.Tilaukset.Remove(tilaukset);
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