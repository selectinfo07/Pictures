using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PicturesAPI.Controllers
{
    public class CategoryController : Controller
    {
        private PicturesEntities db = new PicturesEntities();

        //
        // GET: /Category/

        public ActionResult Index()
        {
            return View(db.CATEGORIES.ToList());
        }

        //
        // GET: /Category/Details/5

        public ActionResult Details(int id = 0)
        {
            CATEGORy category = db.CATEGORIES.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(CATEGORy category)
        {
            if (ModelState.IsValid)
            {
                var uploadedImg = Request.Files["ImageUrl"];
                if (uploadedImg!=null)
                {
                    string extension = System.IO.Path.GetExtension(uploadedImg.FileName);
                    string filename = "Cat_" + category.Name.Replace(" ", "_") + extension;
                    string path = string.Format("{0}\\{1}", Server.MapPath("~/Content/Images"), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    uploadedImg.SaveAs(path);

                    category.ImageUrl = filename;
                }
                db.CATEGORIES.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        //
        // GET: /Category/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CATEGORy category = db.CATEGORIES.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult Edit(CATEGORy category)
        {
            if (ModelState.IsValid)
            {
                var uploadedImg = Request.Files["ImageUrl"];
                if (uploadedImg!=null)
                {
                    string extension = System.IO.Path.GetExtension(uploadedImg.FileName);
                    string filename = "Cat_" + category.Name.Replace(" ", "_") + extension;
                    string path = string.Format("{0}\\{1}", Server.MapPath("~/Content/Images"), filename);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    uploadedImg.SaveAs(path);

                    category.ImageUrl = filename;
                }
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /Category/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CATEGORy category = db.CATEGORIES.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CATEGORy category = db.CATEGORIES.Find(id);
            db.CATEGORIES.Remove(category);
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