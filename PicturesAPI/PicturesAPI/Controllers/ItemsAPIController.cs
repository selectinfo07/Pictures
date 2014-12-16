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
    public class ItemsAPIController : ApiController
    {
        private PicturesEntities db = new PicturesEntities();

        // GET api/ItemsAPI
        public IEnumerable<ItemModel> GetItems(int categoryId, string iemi)
        {
            List<Item> items = new List<Item>();
            if (categoryId == 1)
            {
                var user = db.Users.FirstOrDefault(u => u.IEMINumber == iemi);
                if (user != null)
                {
                    var favItems = db.FavouriteItems.Where(f => f.UserId == user.UserID).Select(d => d.ItemId).ToArray();
                    items = db.Items.Include("CATEGORy").Where(i => favItems.Contains(i.ItemId)).ToList();
                }
            }
            else
            {
                items = db.Items.Include("CATEGORy").Where(i => i.CategoryId == categoryId).ToList();
            }

            string baseUrl = ConfigurationManager.AppSettings["ItemBaseUrl"];
            List<ItemModel> objModelList = new List<ItemModel>();

            AutoMapper.Mapper.CreateMap<Item, ItemModel>()
                    .ForMember(emp => emp.CategoryName, map => map.MapFrom(p => p.CATEGORy.Name))
                    .ForMember(emp => emp.MainImageUrl, map => map.MapFrom(p => baseUrl + p.ImageUrl));

            AutoMapper.Mapper.Map(items, objModelList);
            //foreach (var item in items)
            //{
            //    item.ImageUrl = baseUrl + item.ImageUrl;
            //}
            return objModelList;
        }

        // GET api/ItemsAPI/5
        public ItemDetailsModel GetItem(int id, string iemi)
        {
            ItemDetailsModel objModel = new ItemDetailsModel();
            Item item = db.Items.Include("CATEGORy").Include("ItemImages").FirstOrDefault(i => i.ItemId == id);
            string baseUrl = ConfigurationManager.AppSettings["ItemBaseUrl"];
            AutoMapper.Mapper.CreateMap<ItemImage, ItemImageModel>();
            AutoMapper.Mapper.CreateMap<Item, ItemDetailsModel>()
                   .ForMember(emp => emp.CategoryName, map => map.MapFrom(p => p.CATEGORy.Name))
                   .ForMember(emp => emp.MainImageUrl, map => map.MapFrom(p => baseUrl + p.ImageUrl));
            if (item == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

             var user = db.Users.FirstOrDefault(u => u.IEMINumber == iemi);
           

            AutoMapper.Mapper.Map(item, objModel);
            if (objModel.ItemImages != null)
            {
                foreach (var image in objModel.ItemImages)
                {
                    image.ImageUrl = baseUrl + image.ImageUrl;
                }
            }

            if (user != null)
            {
                var favItems = db.FavouriteItems.Where(f => f.UserId == user.UserID).Select(d => d.ItemId).ToArray();
                if (favItems != null && favItems.Contains(id))
                {
                    objModel.IsFavourite = true;
                }

            }

            return objModel;
        }

        // GET api/ItemsAPI/5
        public bool AddFavourite(int id, string iemi, int fav)
        {
            ItemDetailsModel objModel = new ItemDetailsModel();

            User user = db.Users.FirstOrDefault(u => u.IEMINumber == iemi);
            if (user != null)
            {
                Item item = db.Items.Include("CATEGORy").Include("ItemImages").FirstOrDefault(i => i.ItemId == id);
                if (item != null)
                {

                    FavouriteItem favItem = new FavouriteItem();
                    if (fav == 1)
                    {
                        favItem.UserId = user.UserID;
                        favItem.ItemId = item.ItemId;
                        db.FavouriteItems.Add(favItem);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        favItem = db.FavouriteItems.Where(f => f.ItemId == item.ItemId).FirstOrDefault();
                        if (favItem != null)
                        {
                            db.FavouriteItems.Remove(favItem);
                            db.SaveChanges();
                        }

                        return true;
                    }

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


            //string baseUrl = ConfigurationManager.AppSettings["ItemBaseUrl"];
            //AutoMapper.Mapper.CreateMap<ItemImage, ItemImageModel>();
            //AutoMapper.Mapper.CreateMap<Item, ItemDetailsModel>()
            //       .ForMember(emp => emp.CategoryName, map => map.MapFrom(p => p.CATEGORy.Name))
            //       .ForMember(emp => emp.ImageUrl, map => map.MapFrom(p => baseUrl + p.ImageUrl));


            ////AutoMapper.Mapper.Map(item, objModel);
            //if (objModel.ItemImages != null)
            //{
            //    foreach (var image in objModel.ItemImages)
            //    {
            //        image.ImageUrl = baseUrl + image.ImageUrl;
            //    }
            //}
            return true;
        }

        // PUT api/ItemsAPI/5
        public HttpResponseMessage PutItem(int id, Item item)
        {
            if (ModelState.IsValid && id == item.ItemId)
            {
                db.Entry(item).State = EntityState.Modified;

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

        // POST api/ItemsAPI
        public HttpResponseMessage PostItem(Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, item);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = item.ItemId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/ItemsAPI/5
        public HttpResponseMessage DeleteItem(int id)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Items.Remove(item);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}