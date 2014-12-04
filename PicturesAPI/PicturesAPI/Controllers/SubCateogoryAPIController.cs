using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PicturesAPI.Controllers
{
    public class SubCateogoryAPIController : ApiController
    {
        private PicturesEntities db = new PicturesEntities();

        // GET api/SubCateogoryAPI
        public IEnumerable<SubCategory> GetSubCategories(int categoryId)
        {
            string baseUrl = ConfigurationManager.AppSettings["BaseUrlCat"];
            var subCategories = db.SubCategories.Where(a => a.CategoryId == categoryId).ToList();
            foreach (var item in subCategories)
            {
                item.ImageUrl = baseUrl + item.ImageUrl;
            }

            return subCategories;
        }

        // GET api/SubCateogoryAPI/5
        public SubCategory GetSubCategory(int id)
        {
            SubCategory subcategory = db.SubCategories.Find(id);
            if (subcategory == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return subcategory;
        }

        // PUT api/SubCateogoryAPI/5
        public HttpResponseMessage PutSubCategory(int id, SubCategory subcategory)
        {
            if (ModelState.IsValid && id == subcategory.SubCategoryId)
            {
                db.Entry(subcategory).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/SubCateogoryAPI
        public HttpResponseMessage PostSubCategory(SubCategory subcategory)
        {
            if (ModelState.IsValid)
            {
                db.SubCategories.Add(subcategory);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, subcategory);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = subcategory.SubCategoryId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/SubCateogoryAPI/5
        public HttpResponseMessage DeleteSubCategory(int id)
        {
            SubCategory subcategory = db.SubCategories.Find(id);
            if (subcategory == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.SubCategories.Remove(subcategory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, subcategory);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}