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
    public class RegionController : AdminController
    {
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var model = this.UnitOfWork.RegionRepository.Get();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Regionname_desc" : "";
            ViewBag.AddressesSortParm = String.IsNullOrEmpty(sortOrder) ? "Address_desc" : "Address";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(a => a.Name.ToUpper().Contains(searchString.ToUpper()));
                if (model.Count() == 0)
                {
                    ViewBag.NoSearchResults = true;
                }
            }

            switch (sortOrder)
            {
                case "Regionname_desc":
                    model = model.OrderByDescending(a => a.Name);
                    break;
                case "Address":
                    model = model.OrderBy(a => a.Addresses);
                    break;
                case "Address_desc":
                    model = model.OrderByDescending(a => a.Addresses);
                    break;
                default:
                    model = model.OrderBy(a => a.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(model.ToPagedList(pageNumber, pageSize));
        }
        // CREATE ROLE
        public ActionResult Create()
        {
            var model = new Region();
            return View(model);
        }
        // CREATE ROLE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Region region)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception();

                this.UnitOfWork.RegionRepository.Insert(region);
                this.UnitOfWork.Save();

                return RedirectToAction("Index", "Region");
            }
            catch
            {
                return View(region);
            }
        }
        // EDIT ROLE
        [Authorize]
        public ActionResult Edit(Int32 id)
        {
            try
            {
                var model = this.UnitOfWork.RegionRepository.GetByID(id);
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Region");
            }
        }
        // EDIT ROLE POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Region region)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(region);

                var originalRegion = this.UnitOfWork.RegionRepository.GetByID(region.Id);

                if (originalRegion == null)
                    throw new Exception();

                originalRegion.Name = region.Name;

                this.UnitOfWork.RegionRepository.Update(originalRegion);
                this.UnitOfWork.Save();

                return RedirectToAction("Index", "Region");
            }
            catch
            {
                return View(region);
            }
        }
        // DELETE ROLE POST
        [Authorize]
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            try
            {
                var model = this.UnitOfWork.RegionRepository.GetByID(id);
                if (model == null)
                    throw new Exception();

                this.UnitOfWork.RegionRepository.Delete(model);
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
