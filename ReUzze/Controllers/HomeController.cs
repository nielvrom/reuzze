using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Models;

namespace ReUzze.Controllers
{
    public class HomeController : AdminController
    {
        private ReUzze.Helpers.reuzzeRepository repository = new ReUzze.Helpers.reuzzeRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PVAmountPerEntity()
        {
            var model = new ReUzze.Models.AmountPerEntityViewModel
            {
                AmountUsers = this.UnitOfWork.UserRepository.Get().Count<User>(),
                AmountCategories = this.UnitOfWork.CategoryRepository.Get().Count<Category>(),
                AmountRoles = this.UnitOfWork.RoleRepository.Get().Count<Role>(),
                AmountEntities = repository.GetAllEntities().Count(),
                AmountAddresses = this.UnitOfWork.AddressRepository.Get().Count<Address>(),
                AmountBids = this.UnitOfWork.BidRepository.Get().Count<Bid>(),
                AmountImages = this.UnitOfWork.MediaRepository.Get().Count<Media>(),
                AmountRegions = this.UnitOfWork.RegionRepository.Get().Count<Region>()
            };

            return PartialView("_AmountPerEntityPartial", model);
        }

        public ActionResult PVLastEntities()
        {
            var model = repository.GetLastEntities().ToList();

            foreach (ReUzze.Models.entity entity in model)
            {
                entity.user = repository.GetUser(entity.user_id);
            }

            foreach (ReUzze.Models.entity entity in model)
            {
                for (int i = 0; i < entity.favorites.Count; i++)
                {
                    var user = repository.GetUser(entity.favorites.ElementAt(i).user_id);
                    entity.favorites.ElementAt(i).user = user;
                }
            }

            return PartialView("_LastEntitiesPartial", model);
        }

        public ActionResult PVLastBids()
        {
            var model = repository.GetLastBids().ToList();

            foreach (ReUzze.Models.bid bid in model)
            {
                bid.user = repository.GetUser(bid.user_id);
            }

            return PartialView("_LastBidsPartial", model);
        }

        public ActionResult PVLastUsers()
        {
            var model = this.UnitOfWork.UserRepository.Get().OrderByDescending(u => u.Id).Take(10);

            return PartialView("_LastUsersPartial", model);
        }
    }
}
