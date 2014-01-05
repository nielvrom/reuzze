using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReUzze.Controllers;
using PagedList;
using PagedList.Mvc;

namespace ReUzze.Controllers
{
    public class RoleController : AdminController
    {
        // LIST OF ROLES
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var model = this.UnitOfWork.RoleRepository.Get();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Rolename_desc" : "";
            ViewBag.DescriptionSortParm = String.IsNullOrEmpty(sortOrder) ? "Description_desc" : "Description";

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
                model = model.Where(a => a.Name.ToUpper().Contains(searchString.ToUpper()) || a.Description.ToUpper().Contains(searchString.ToUpper()));
                if (model.Count() == 0)
                {
                    ViewBag.NoSearchResults = true;
                }
            }

            switch (sortOrder)
            {
                case "Rolename_desc":
                    model = model.OrderByDescending(a => a.Name);
                    break;
                case "Description":
                    model = model.OrderBy(a => a.Description);
                    break;
                case "Description_desc":
                    model = model.OrderByDescending(a => a.Description);
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
            var model = new Role();
            return View(model);
        }

        // CREATE ROLE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception();

                this.UnitOfWork.RoleRepository.Insert(role);
                this.UnitOfWork.Save();

                return RedirectToAction("Index", "Role");
            }
            catch
            {
                return View(role);
            }
        }
        // EDIT ROLE
        [Authorize]
        public ActionResult Edit(Int16 id)
        {
            try
            {
                var model = this.UnitOfWork.RoleRepository.GetByID(id);
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Role");
            }
        }
        // EDIT ROLE POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Role role)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(role);

                var originalRole = this.UnitOfWork.RoleRepository.GetByID(role.Id);

                if (originalRole == null)
                    throw new Exception();

                originalRole.Name = role.Name;
                originalRole.Description = role.Description;
                originalRole.ModifiedDate = DateTime.Now;

                this.UnitOfWork.RoleRepository.Update(originalRole);
                this.UnitOfWork.Save();

                return RedirectToAction("Index", "Role");
            }
            catch
            {
                return View(role);
            }
        }
        // DELETE ROLE POST
        [Authorize]
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            try
            {
                var model = this.UnitOfWork.RoleRepository.GetByID(id);
                if (model == null)
                    throw new Exception();

                this.UnitOfWork.RoleRepository.Delete(model);
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
