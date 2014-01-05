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
    [Authorize]
    public class CategoryController : AdminController
    {
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var model = this.UnitOfWork.CategoryRepository.Get();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
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
                case "Name_desc":
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

        public ActionResult Create()
        {
            var model = new ReUzze.Models.CategoryViewModel
            {
                Category = new Category(),
                Categories = this.UnitOfWork.CategoryRepository.Get()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReUzze.Models.CategoryViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception();

                this.UnitOfWork.CategoryRepository.Insert(viewModel.Category);
                this.UnitOfWork.Save();

                return RedirectToAction("Index", "Category");
            }
            catch
            {
                var model = new ReUzze.Models.CategoryViewModel
                {
                    Category = viewModel.Category,
                    Categories = this.UnitOfWork.CategoryRepository.Get()
                };
                return View(model);
            }
        }

        // EDIT ROLE
        public ActionResult Edit(Int16 id)
        {
            try
            {
                var category = this.UnitOfWork.CategoryRepository.GetByID(id);
                var model = new ReUzze.Models.CategoryViewModel
                {
                    Category = category,
                    Categories = this.UnitOfWork.CategoryRepository.Get()
                };
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Category");
            }
        }

        // EDIT ROLE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReUzze.Models.CategoryViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(viewModel);

                var originalCategory = this.UnitOfWork.CategoryRepository.GetByID(viewModel.Category.Id);

                if (originalCategory == null)
                    throw new Exception();

                originalCategory.Name = viewModel.Category.Name;
                originalCategory.ParentId = viewModel.Category.ParentId;
                originalCategory.Description = viewModel.Category.Description;
                originalCategory.ModifiedDate = DateTime.Now;

                this.UnitOfWork.CategoryRepository.Update(originalCategory);
                this.UnitOfWork.Save();

                return RedirectToAction("Index", "Category");
            }
            catch
            {
                return View(viewModel);
            }
        }

        // DELETE ROLE POST
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            try
            {
                var model = this.UnitOfWork.CategoryRepository.GetByID(id);
                if (model == null)
                    throw new Exception();

                this.UnitOfWork.CategoryRepository.Delete(model);
                this.UnitOfWork.Save();
                return Content(Boolean.TrueString);
            }
            catch
            {
                return Content(Boolean.FalseString);
            }
        }

        // NEEDS REFACTORING AND EXTRA!
        public ActionResult Setcategories()
        {
            try
            {
                // ADD CATEGORIES TO LIST
                var myDicList = new MultiDimDictList<string, string>();
                myDicList.Add("Auto's & Motoren", "Auto - Accessoires");
                myDicList.Add("Auto's & Motoren", "Auto - Onderdelen & Wisselstuk");
                myDicList.Add("Auto's & Motoren", "Auto - Hifi & Navigatie");
                myDicList.Add("Auto's & Motoren", "Auto - Tuning & Styling");

                myDicList.Add("Beauty", "Make-up");
                myDicList.Add("Beauty", "Parfum");
                myDicList.Add("Beauty", "Gezondheid & Welzijn");
                myDicList.Add("Beauty", "Sieraden & Horloges");

                myDicList.Add("Boeken, Films & Muziek", "Boeken & Strips");
                myDicList.Add("Boeken, Films & Muziek", "Dvd's, Video's & Films");
                myDicList.Add("Boeken, Films & Muziek", "Muziek & Instrumenten");

                myDicList.Add("Elektronica", "Computers & Tablets");
                myDicList.Add("Elektronica", "Smartphones & Mobiele telefonie");
                myDicList.Add("Elektronica", "Tv, Audio & Video");
                myDicList.Add("Elektronica", "Fotografie & Camera's");

                myDicList.Add("Huis & Tuin", "Meubels");
                myDicList.Add("Huis & Tuin", "Tuin & Terras");
                myDicList.Add("Huis & Tuin", "Woondecoratie & Design");

                myDicList.Add("Mode", "Dameskleding");
                myDicList.Add("Mode", "Damesschoenen");
                myDicList.Add("Mode", "Herenkleding");
                myDicList.Add("Mode", "Kinderkleding meisjes");

                myDicList.Add("Overige", "Business & Industrie");
                myDicList.Add("Overige", "Immobiliën");

                myDicList.Add("Verzamelen", "Kunst & Antiek");
                myDicList.Add("Verzamelen", "Munten & Bankbiljetten");
                myDicList.Add("Verzamelen", "Postzegels");
                myDicList.Add("Verzamelen", "Verzamelen");

                myDicList.Add("Vrije Tijd", "Doe-Het-Zelf & Hobby");
                myDicList.Add("Vrije Tijd", "Games & Consoles");
                myDicList.Add("Vrije Tijd", "Sport & Fietsen");
                myDicList.Add("Vrije Tijd", "Tickets & Reizen");

                var category = new Category();
                category.Name = "Auto's & Motoren";
                category.Description = "Auto's & Motoren";
                category.CreateDate = DateTime.Now;
                this.UnitOfWork.CategoryRepository.Insert(category);
                this.UnitOfWork.Save();
                foreach (string value in myDicList["Auto's & Motoren"])
                {
                    var childcategory = new Category();
                    childcategory.Name = value;
                    childcategory.Description = value;
                    childcategory.CreateDate = DateTime.Now;
                    childcategory.ParentCategory = category;
                    this.UnitOfWork.CategoryRepository.Insert(childcategory);
                }
                this.UnitOfWork.Save();
            }
            catch(Exception ex)
            {
                
            }
            return RedirectToAction("Index", "Category");
        }

        public class MultiDimDictList<K, T> : Dictionary<K, List<T>>
        {
            public void Add(K key, T addObject)
            {
                if (!ContainsKey(key)) Add(key, new List<T>());
                base[key].Add(addObject);
            }
        }
    }
}
