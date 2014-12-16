using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PicturesAPI.Controllers
{
    public class ItemController : Controller
    {
        private PicturesEntities db = new PicturesEntities();

        //
        // GET: /Item/

        public ActionResult Index()
        {
            var items = db.Items.Include(i => i.CATEGORy);
            return View(items.ToList());
        }

        //
        // GET: /Item/Details/5

        public ActionResult Details(int id = 0)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // GET: /Item/Create

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.CATEGORIES, "CategoryId", "Name");
            return View();
        }

        //
        // POST: /Item/Create

        [HttpPost]
        public ActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                var uploadedImg = Request.Files["ImageUrl"];
                if (uploadedImg!=null)
                {
                    string extension = System.IO.Path.GetExtension(uploadedImg.FileName);
                    string filename = "Itm_" + item.Name.Replace(" ", "_") + extension;
                    string path = string.Format("{0}\\{1}", Server.MapPath("~/Content/Images"), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    uploadedImg.SaveAs(path);

                    item.ImageUrl = filename;
                }
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.CATEGORIES, "CategoryId", "Name", item.CategoryId);
            return View(item);
        }

        //
        // GET: /Item/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.CATEGORIES, "CategoryId", "Name", item.CategoryId);
            return View(item);
        }

        //
        // POST: /Item/Edit/5

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                var uploadedImg = Request.Files["ImageUrl"];
                if (uploadedImg!=null)
                {
                    string extension = System.IO.Path.GetExtension(uploadedImg.FileName);
                    string filename = "Itm_" + item.Name.Replace(" ", "_") + extension;
                    string path = string.Format("{0}\\{1}", Server.MapPath("~/Content/Images"), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    uploadedImg.SaveAs(path);

                    item.ImageUrl = filename;
                }
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.CATEGORIES, "CategoryId", "Name", item.CategoryId);
            return View(item);
        }

        //
        // GET: /Item/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Item/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}