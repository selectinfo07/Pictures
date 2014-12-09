using PicturesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PicturesAPI.Controllers
{
    public class FavouriteAPIController : ApiController
    {
        private PicturesEntities db = new PicturesEntities();
        // GET api/favouriteapi
        public bool Get(int itemId, string iemi, int fav)
        {
            ItemDetailsModel objModel = new ItemDetailsModel();

            User user = db.Users.FirstOrDefault(u => u.IEMINumber == iemi);
            if (user != null)
            {
                Item item = db.Items.Include("CATEGORy").Include("ItemImages").FirstOrDefault(i => i.ItemId == itemId);
                if (item != null)
                {

                    FavouriteItem favItem = new FavouriteItem();
                    if (fav == 1)
                    {
                        favItem = db.FavouriteItems.Where(f => f.ItemId == item.ItemId && f.UserId == user.UserID).FirstOrDefault();
                        if (favItem == null)
                        {
                            favItem = new FavouriteItem();
                            favItem.UserId = user.UserID;
                            favItem.ItemId = item.ItemId;
                            db.FavouriteItems.Add(favItem);
                            db.SaveChanges();
                        }
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

        // GET api/favouriteapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/favouriteapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/favouriteapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/favouriteapi/5
        public void Delete(int id)
        {
        }
    }
}
