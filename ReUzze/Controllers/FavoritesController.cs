using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Models;
using ReUzze.Models;

namespace ReUzze.Controllers
{
    [Authorize]
    public class FavoritesController : AdminController
    {
        private ReUzze.Helpers.reuzzeRepository repository = new ReUzze.Helpers.reuzzeRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Favoritesfromuser(Int32 id, string username)
        {
            if (username != "null")
            {
                id = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == username).FirstOrDefault().Id;
            }
            var model = repository.GetFavoritesFromUser(id).ToList(); 

            foreach (ReUzze.Models.favorite favorite in model)
            {
                favorite.entity.user = repository.GetUser(favorite.user_id);
            }

            return View(model);
        }

        public ActionResult AddToFavorites(int entityID)
        {
            try
            {
                var originalUser = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

                var originalEntity = repository.GetEntity(entityID);

                if (originalEntity == null || originalUser == null)
                    throw new Exception();

                var favorite = new favorite();
                favorite.entity_id = entityID;
                favorite.user_id = originalUser.Id;

                repository.AddFavorite(favorite);

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }

            return Json(new { success = false });
        }

        public ActionResult DeleteFromFavorites(int entityID)
        {
            try
            {
                var originalUser = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

                var originalEntity = repository.GetEntity(entityID);

                if (originalEntity == null || originalUser == null)
                    throw new Exception();

                var favorite = repository.GetFavorite(originalUser.Id, originalEntity.entity_id);

                repository.DeleteFavorite(favorite);

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }

            return Json(new { success = false });
        }

    }
}
