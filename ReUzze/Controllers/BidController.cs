using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Models;
using PagedList;
using PagedList.Mvc;

namespace ReUzze.Controllers
{
    public class BidController : AdminController
    {
        private ReUzze.Helpers.reuzzeRepository repository = new ReUzze.Helpers.reuzzeRepository();

        // LIST OF ROLES
        public ActionResult Index(int? page)
        {
            var model = repository.GetAllBids().ToList();

            foreach (ReUzze.Models.bid bid in model)
            {
                bid.user = repository.GetUser(bid.user_id);
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult BidsFromUser(int id)
        {
            var model = repository.GetBidsFromUser(id).ToList();

            if (model.Count > 0)
            {
                foreach (ReUzze.Models.bid bid in model)
                {
                    bid.user = repository.GetUser(bid.user_id);
                }

                foreach (ReUzze.Models.bid bid in model)
                {
                    bid.entity = repository.GetEntity(bid.entity_id);

                    bid.entity.user = repository.GetUser(bid.entity.user_id);
                }
            }

            return View(model);
        }


        // CREATE ROLE
        [Authorize]
        public ActionResult Create()
        {
            var model = new Bid();
            return View(model);
        }
        // CREATE ROLE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bid bid)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception();

                bid.EntityId = 2;
                bid.UserId = 1;
                bid.Date = DateTime.Now;
                this.UnitOfWork.BidRepository.Insert(bid);
                this.UnitOfWork.Save();

                return RedirectToAction("Index", "Bid");
            }
            catch
            {
                return View(bid);
            }
        }

        // DELETE ROLE POST
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            try
            {
                var model = this.UnitOfWork.BidRepository.GetByID(id);
                if (model == null)
                    throw new Exception();

                this.UnitOfWork.BidRepository.Delete(model);
                this.UnitOfWork.Save();
                return Content(Boolean.TrueString);
            }
            catch
            {
                return Content(Boolean.FalseString);
            }
        }

    }
}
