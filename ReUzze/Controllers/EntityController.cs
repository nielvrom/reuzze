using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Models;
using ReUzze.Helpers;
using PagedList;
using PagedList.Mvc;

namespace ReUzze.Controllers
{
    [Authorize]
    public class EntityController : AdminController
    {
        private ReUzze.Helpers.reuzzeRepository repository = new ReUzze.Helpers.reuzzeRepository();

        // SORT ON TITLE, DESSCRIPTION, INSTANT SELLING PRICE, CONDITION, CREATED BY USER, REGION, CATEGORY, BIDS
        public ActionResult Index(int? page)
        {
            var model = repository.GetAllEntities().ToList();

            foreach(ReUzze.Models.entity entity in model)
            {
                entity.user = repository.GetUser(entity.user_id);
            }

            foreach (ReUzze.Models.entity entity in model)
            {
                for(int i = 0; i < entity.favorites.Count; i++)
                {
                    var user = repository.GetUser(entity.favorites.ElementAt(i).user_id);
                    entity.favorites.ElementAt(i).user = user;
                }
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(model.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Detail(Int64 id)
        {
            var model = repository.GetEntityByID(id).ToList();
            foreach (ReUzze.Models.entity entity in model)
            {
                entity.user = repository.GetUser(entity.user_id);
            }

            foreach (ReUzze.Models.entity entity in model)
            {
                for (int i = 0; i < entity.bids.Count; i++)
                {
                    var user = repository.GetUser(entity.bids.ElementAt(i).user_id);
                    entity.bids.ElementAt(i).user = user;
                }
            }

            // ADD IMAGES TO VIEWBAG
            List<string> imageslist = new List<string>();
            var images = this.UnitOfWork.MediaRepository.Get().Where(m => m.EntityId == id);
            foreach(App.Models.Media media in images)
            {
                imageslist.Add(media.URL);
            }
            ViewBag.ImagesList = imageslist;

            return View(model.ToList());
        }

        [HttpPost]
        public ActionResult Detail(ReUzze.Models.bid bid)
        {
            try
            {
                if (bid.entity.user.username != User.Identity.Name)
                {
                    var bidd = new Bid();
                    bidd.EntityId = bid.entity_id;
                    var user = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                    bidd.UserId = user.Id;
                    bidd.Amount = bid.bid_amount;
                    bidd.Date = DateTime.Now;
                    this.UnitOfWork.BidRepository.Insert(bidd);
                    this.UnitOfWork.Save();
                }
                else
                {

                }

                return RedirectToAction("Detail", "Entity");
            }
            catch(Exception e)
            {

            }

            return RedirectToAction("Detail", "Entity");
            
        }

        public ActionResult Create()
        {
            ViewBag.DropDownList = ReUzze.Helpers.EnumHelper.SelectListFor<Condition>();

            var model = new ReUzze.Models.EntityViewModel();
            PutTypeDropDownInto(model);
            return View(model);
        }

        [NonAction]
        private void PutTypeDropDownInto(ReUzze.Models.EntityViewModel model)
        {
            model.GroupedTypeOptions = this.UnitOfWork.CategoryRepository.Get()
                .Where(t => t.ParentCategory != null)
                .OrderBy(t => t.ParentCategory.Name).ThenBy(t => t.Name)
                .Select(t => new GroupedSelectListItem
                {
                    GroupKey = t.ParentId.ToString(),
                    GroupName = t.ParentCategory.Name,
                    Text = t.Name,
                    Value = t.Id.ToString()
                }
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReUzze.Models.EntityViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(viewModel);

                if (viewModel.SelectedCategoryId == null)
                {
                    ViewBag.DropDownList = ReUzze.Helpers.EnumHelper.SelectListFor<Condition>();

                    var model = new ReUzze.Models.EntityViewModel()
                    {
                        StatusMessage = "You need to select a category!",
                        Categories = new SelectList(this.UnitOfWork.CategoryRepository.Get(), "Id", "Name")
                    };
                    PutTypeDropDownInto(model);
                    return View(model);
                }

                User usr = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

                var entity = new Entity();
                entity.Title = viewModel.Title;
                entity.Description = viewModel.Description;
                entity.StartTime = viewModel.StartTime;
                entity.EndTime = viewModel.EndTime;
                entity.InstantSellingPrice = viewModel.InstantSellingPrice;
                if (viewModel.Condition.GetValueOrDefault().ToString() == "Used")
                {
                    entity.Condition = Condition.Used;
                }
                else
                {
                    entity.Condition = Condition.New;
                }
                entity.UserId = usr.Id;
                entity.RegionId = usr.Person.Address.RegionId;
                entity.CategoryId = Convert.ToInt16(viewModel.SelectedCategoryId);

                this.UnitOfWork.EntityRepository.Insert(entity);
                this.UnitOfWork.Save();

                // SAVE THE IMGES
                if (viewModel.Files.FirstOrDefault() != null)
                {
                    foreach (var file in viewModel.Files)
                    {
                        if (file.ContentLength > 0)
                        {
                            repository.AddMedia(entity.Id, Path.GetFileName(file.FileName), file.ContentType);

                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Resources/uploads"), fileName);
                            file.SaveAs(path);
                        }
                    }
                }

                return RedirectToAction("Index", "Entity");
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

            }

            return RedirectToAction("Create", "Entity");
        }

        public ActionResult Edit(Int64 id)
        {
            var entity = repository.GetEntity(id);
            if (entity == null)
                throw new Exception();

            var model = new ReUzze.Models.EntityViewModel
            {
                Title = entity.entity_title,
                Description = entity.entity_description,
                StartTime = entity.entity_starttime,
                EndTime = entity.entity_endtime,
                InstantSellingPrice = entity.entity_instantsellingprice,
                ShippingPrice = entity.entity_shippingprice
            };

            return View(model);
        }

        [Authorize] // USER NEEDS TO BE AUTHORIZED
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReUzze.Models.EntityViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(viewModel);

                

                return RedirectToAction("Index", "Entity");
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

            }

            return RedirectToAction("Create", "Entity");
        }

        // DELETE ROLE POST
        [HttpPost]
        public ActionResult Delete(Int64 id)
        {
            try
            {
                var model = repository.GetEntity(id);
                if (model == null)
                    throw new Exception();

                repository.DeleteEntity(model);

                return Content(Boolean.TrueString);
            }
            catch
            {
                return Content(Boolean.FalseString);
            }
        }

        public ActionResult Entitiesfromuser(int id)
        {
            var model = repository.GetEntitiesFromUser(id).ToList();

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

            return View(model);
        }

    }
}
