using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Models;
using ReUzze.Models;

namespace ReUzze.Controllers
{
    [Authorize]
    public class MediaController : AdminController
    {
        private ReUzze.Helpers.reuzzeRepository repository = new ReUzze.Helpers.reuzzeRepository();

        public ActionResult Index()
        {
            var model = repository.GetAllMedia().ToList();
            foreach (medium media in model)
            {
                media.entity = repository.GetEntity(media.entity_id);
            }
            //var model = this.UnitOfWork.MediaRepository.Get();
            return View(model);
        }

        // YOU CAN'T INSERT IMAGES ONLY, THEY HAVE TO BE A PART OF AN ENTITY

    }
}
