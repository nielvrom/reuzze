using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Models;
using ReUzze.Models;
using PagedList;
using PagedList.Mvc;

namespace ReUzze.Controllers
{
    public class AddressController : AdminController
    {
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var model = this.UnitOfWork.AddressRepository.Get();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CitySortParm = String.IsNullOrEmpty(sortOrder) ? "City_desc" : "";
            ViewBag.StreetSortParm = String.IsNullOrEmpty(sortOrder) ? "Street_desc" : "Street";
            ViewBag.RegionSortParm = String.IsNullOrEmpty(sortOrder) ? "Region_desc" : "Region";

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
                model = model.Where(a => a.City.ToUpper().Contains(searchString.ToUpper()) || a.Street.ToUpper().Contains(searchString.ToUpper()) || a.Region.Name.ToUpper().Contains(searchString.ToUpper()));
                if (model.Count() == 0)
                {
                    ViewBag.NoSearchResults = true;
                }
            }

            switch (sortOrder)
            {
                case "City_desc":
                    model = model.OrderByDescending(a => a.City);
                    break;
                case "Street":
                    model = model.OrderBy(a => a.Street);
                    break;
                case "Street_desc":
                    model = model.OrderByDescending(a => a.Street);
                    break;
                case "Region":
                    model = model.OrderBy(a => a.Region.Name);
                    break;
                case "Region_desc":
                    model = model.OrderByDescending(a => a.Region.Name);
                    break;
                default:
                    model = model.OrderBy(a => a.City);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            var model = new ReUzze.Models.AddressViewModel
            {
                Address = new Address(),
                Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReUzze.Models.AddressViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception();

                var regions = this.UnitOfWork.RegionRepository.Get().Count();
                if (regions == 0)
                {
                    var model = new ReUzze.Models.AddressViewModel
                    {
                        StatusMessage = "There are no regions in the database ...",
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                if (viewModel.SelectRegionId == 0)
                {
                    var model = new ReUzze.Models.AddressViewModel
                    {
                        StatusMessage = "You need to select a region!",
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                viewModel.Address.Latitude = Convert.ToDecimal(viewModel.Address.Latitude);
                viewModel.Address.Longitude = Convert.ToDecimal(viewModel.Address.Longitude);

                viewModel.Address.RegionId = viewModel.SelectRegionId;
                this.UnitOfWork.AddressRepository.Insert(viewModel.Address);
                this.UnitOfWork.Save();

                return RedirectToAction("Index", "Address");
            }
            catch
            {
                var model = new ReUzze.Models.AddressViewModel
                {
                    Address = viewModel.Address,
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                };
                return View(model);
            }
        }

        public ActionResult Edit(Int32 id)
        {
            try
            {
                var address = this.UnitOfWork.AddressRepository.GetByID(id);
                var model = new ReUzze.Models.AddressViewModel
                {
                    Address = address,
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name"),
                    SelectRegionId = address.RegionId
                };
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Address");
            }
        }

        // EDIT ROLE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReUzze.Models.AddressViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(viewModel);

                var originalAddress = this.UnitOfWork.AddressRepository.GetByID(viewModel.Address.Id);

                if (originalAddress == null)
                    throw new Exception();

                originalAddress.City = viewModel.Address.City;
                originalAddress.Street = viewModel.Address.Street;
                originalAddress.StreetNumber = viewModel.Address.StreetNumber;
                originalAddress.RegionId = viewModel.SelectRegionId;

                this.UnitOfWork.AddressRepository.Update(originalAddress);
                this.UnitOfWork.Save();

                return RedirectToAction("Index", "Address");
            }
            catch
            {
                var model = new ReUzze.Models.AddressViewModel
                {
                    Address = viewModel.Address,
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                };
                return View(model);
            }

        }

        // DELETE ROLE POST
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            try
            {
                var model = this.UnitOfWork.AddressRepository.GetByID(id);
                if (model == null)
                    throw new Exception();

                this.UnitOfWork.AddressRepository.Delete(model);
                this.UnitOfWork.Save();
                return Content(Boolean.TrueString);
            }
            catch
            {
                return Content(Boolean.FalseString);
            }
        }

        // DELETE ROLE POST
        public ActionResult Map()
        {
            var model = this.UnitOfWork.AddressRepository.Get().Select(m => new MapViewModel
            {
                Latitude = m.Latitude,
                Longitude = m.Longitude,
            }).AsEnumerable();
            return View(model);
        }
    }
}
