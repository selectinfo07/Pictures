using PicturesAPI.Models;
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
    public class CategoryAPIController : ApiController
    {
        private PicturesEntities db = new PicturesEntities();

        // GET api/CategoryAPI
        public IEnumerable<CategoriesModel> GetCATEGORies(string imei)
        {
            PicturesAPI.User user = db.Users.FirstOrDefault(u => u.IEMINumber == imei);
            if (user == null)
            {
                user = new User();
                user.IEMINumber = imei;
                db.Users.Add(user);
                db.SaveChanges();
            }
            string baseUrl = ConfigurationManager.AppSettings["BaseUrlCat"];
            List<CategoriesModel> objModelList = new List<CategoriesModel>();
            AutoMapper.Mapper.CreateMap<CATEGORy, CategoriesModel>()
                .ForMember(emp => emp.ImageUrl, map => map.MapFrom(p => baseUrl + p.ImageUrl)); 

           
            var categories = db.CATEGORIES.ToList();

            AutoMapper.Mapper.Map(categories, objModelList);
            //foreach (var item in categories)
            //{
            //    item.ImageUrl = baseUrl + item.ImageUrl;
            //}

            return objModelList;
        }

        // GET api/CategoryAPI/5
        public CATEGORy GetCATEGORy(int id)
        {
            CATEGORy category = db.CATEGORIES.Find(id);
            if (category == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return category;
        }

        // PUT api/CategoryAPI/5
        public HttpResponseMessage PutCATEGORy(int id, CATEGORy category)
        {
            if (ModelState.IsValid && id == category.CategoryId)
            {
                db.Entry(category).State = EntityState.Modified;

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

        // POST api/CategoryAPI
        public HttpResponseMessage PostCATEGORy(CATEGORy category)
        {
            if (ModelState.IsValid)
            {
                db.CATEGORIES.Add(category);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, category);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = category.CategoryId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CategoryAPI/5
        public HttpResponseMessage DeleteCATEGORy(int id)
        {
            CATEGORy category = db.CATEGORIES.Find(id);
            if (category == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CATEGORIES.Remove(category);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, category);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}