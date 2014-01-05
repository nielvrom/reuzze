using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReUzze.Controllers;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net.Mail;
using System.Web.Security;
using PagedList;
using PagedList.Mvc;

namespace ReUzze.Controllers
{
    [Authorize]
    public class UserController : AdminController
    {
        private ReUzze.Helpers.reuzzeRepository repository = new ReUzze.Helpers.reuzzeRepository();

        // SORT ON USERNAME, EMAIL, FULL NAME (FIRST NAME), USER RATING, REGION, ROLE, PLACED BIDS, CREATED ON
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var model = this.UnitOfWork.UserRepository.Get();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.UsernameSortParm = String.IsNullOrEmpty(sortOrder) ? "Username_desc" : "";
            ViewBag.EmailSortParm = String.IsNullOrEmpty(sortOrder) ? "Email_desc" : "Email";
            ViewBag.FullNameSortParm = String.IsNullOrEmpty(sortOrder) ? "FullName_desc" : "FullName";
            ViewBag.RatingSortParm = String.IsNullOrEmpty(sortOrder) ? "Rating_desc" : "Rating";
            ViewBag.RegionSortParm = String.IsNullOrEmpty(sortOrder) ? "Region_desc" : "Region";
            ViewBag.RoleSortParm = String.IsNullOrEmpty(sortOrder) ? "Role_desc" : "Role";
            ViewBag.BidsSortParm = String.IsNullOrEmpty(sortOrder) ? "Bids_desc" : "Bids";
            ViewBag.CreatedSortParm = String.IsNullOrEmpty(sortOrder) ? "Created_desc" : "Created";

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
                model = model.Where(a => a.UserName.ToUpper().Contains(searchString.ToUpper()) || a.Email.ToUpper().Contains(searchString.ToUpper()) || a.Person.FirstName.ToUpper().Contains(searchString.ToUpper()) || a.Person.Address.Region.Name.ToUpper().Contains(searchString.ToUpper()) || a.Role.Name.ToUpper().Contains(searchString.ToUpper()));
                if(model.Count() == 0)
                {
                    ViewBag.NoSearchResults = true;
                }
            }

            switch (sortOrder)
            {
                case "Username_desc":
                    model = model.OrderByDescending(a => a.UserName);
                    break;
                case "Email":
                    model = model.OrderBy(a => a.Email);
                    break;
                case "Email_desc":
                    model = model.OrderByDescending(a => a.Email);
                    break;
                case "FullName":
                    model = model.OrderBy(a => a.Person.FirstName);
                    break;
                case "FullName_desc":
                    model = model.OrderByDescending(a => a.Person.FirstName);
                    break;
                case "Rating":
                    model = model.OrderBy(a => a.Rating);
                    break;
                case "Rating_desc":
                    model = model.OrderByDescending(a => a.Rating);
                    break;
                case "Region":
                    model = model.OrderBy(a => a.Person.Address.Region.Name);
                    break;
                case "Region_desc":
                    model = model.OrderByDescending(a => a.Person.Address.Region.Name);
                    break;
                case "Role":
                    model = model.OrderBy(a => a.Role.Name);
                    break;
                case "Role_desc":
                    model = model.OrderByDescending(a => a.Role.Name);
                    break;
                case "Bids":
                    model = model.OrderBy(a => a.Bids);
                    break;
                case "Bids_desc":
                    model = model.OrderByDescending(a => a.Bids);
                    break;
                default:
                    model = model.OrderBy(a => a.UserName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Detail(int id)
        {
            var model = this.UnitOfWork.UserRepository.Get().Where(u => u.Id == id);
            return View(model.FirstOrDefault());
        }

        public ActionResult Create()
        {
            var model = new ReUzze.Models.UserViewModel
            {
                Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReUzze.Models.UserViewModel viewModel)
        {
            try
            {
                // Check if there are roles in the database
                var roles = this.UnitOfWork.RoleRepository.Get().Count();
                if (roles == 0)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        StatusMessage = "There are no roles in the database ...",
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                // Check if there are regions in database
                var regions = this.UnitOfWork.RegionRepository.Get().Count();
                if (regions == 0)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        StatusMessage = "There are no regions in the database ...",
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                // Check if user has selected region
                if (viewModel.SelectRegionId == 0)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        StatusMessage = "You need to select a region!",
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                // Check if user has selected role
                if (viewModel.SelectRoleId == 0)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        StatusMessage = "You need to select a role!",
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                // Check if username already exists
                var usrname = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == viewModel.UserName).Count();
                if (usrname != 0)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        StatusMessage = "Please choose another username. This one already exists.",
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                // Check if user email already exists
                var email = this.UnitOfWork.UserRepository.Get().Where(u => u.Email == viewModel.Email).Count();
                if (email != 0)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        StatusMessage = "This email address is already registered.",
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                if (!ModelState.IsValid)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                // ADDRESS
                var address = new Address();
                address.City = viewModel.City;
                address.Street = viewModel.Street;
                address.StreetNumber = viewModel.StreetNumber;
                address.Latitude = viewModel.Latitude;
                address.Longitude = viewModel.Longitude;
                address.RegionId = viewModel.SelectRegionId;
               
                this.UnitOfWork.AddressRepository.Insert(address);

                // PERSON
                var person = new Person();
                person.FirstName = viewModel.FirstName;
                person.SurName = viewModel.SurName;
                person.Address = address;
                this.UnitOfWork.PersonRepository.Insert(person);

                // USER

                var user = new User();
                user.UserName = viewModel.UserName;
                user.Email = viewModel.Email;
                user.PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt(12);
                user.Password = BCrypt.Net.BCrypt.HashPassword(viewModel.Password, user.PasswordSalt);
                user.Rating = 10;
                user.RoleId = viewModel.SelectRoleId;
                user.Person = person;
                this.UnitOfWork.UserRepository.Insert(user);

                // ROLE NEEDS TO BE ADDED TO USER

                this.UnitOfWork.Save();

                return RedirectToAction("Index", "User");
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

            return RedirectToAction("Create", "User");

        }

        public ActionResult Edit(Int32 id)
        {
            try
            {
                var user = this.UnitOfWork.UserRepository.GetByID(id);
                if (user == null)
                    throw new Exception();

                var model = new ReUzze.Models.UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.Person.FirstName,
                    SurName = user.Person.SurName,
                    Street = user.Person.Address.Street,
                    City = user.Person.Address.City,
                    StreetNumber = user.Person.Address.StreetNumber,
                    UserName = user.UserName,
                    Email = user.Email,
                    Rating = user.Rating,
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name"),
                    SelectRegionId = user.Person.Address.RegionId,
                    Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                    SelectRoleId = user.RoleId
                };
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "User");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReUzze.Models.UserViewModel viewModel)
        {
            try
            {
                // Check if there are roles in the database
                var roles = this.UnitOfWork.RoleRepository.Get().Count();
                if (roles == 0)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        StatusMessage = "There are no roles in the database ...",
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                // Check if there are regions in database
                var regions = this.UnitOfWork.RegionRepository.Get().Count();
                if (regions == 0)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        StatusMessage = "There are no regions in the database ...",
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                // Check if user has selected region
                if (viewModel.SelectRegionId == 0)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        StatusMessage = "You need to select a region!",
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                // Check if user has selected role
                if (viewModel.SelectRoleId == 0)
                {
                    var model = new ReUzze.Models.UserViewModel
                    {
                        StatusMessage = "You need to select a role!",
                        Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                        Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name")
                    };
                    return View(model);
                }

                if (!ModelState.IsValid)
                    throw new Exception();

                var originalUser = this.UnitOfWork.UserRepository.GetByID(viewModel.Id);

                if (originalUser == null)
                    throw new Exception();

                originalUser.Person.FirstName = viewModel.FirstName;
                originalUser.Person.SurName = viewModel.SurName;
                originalUser.Person.Address.Street = viewModel.Street;
                originalUser.Person.Address.City = viewModel.City;
                originalUser.Person.Address.StreetNumber = viewModel.StreetNumber;
                originalUser.Person.Address.RegionId = viewModel.SelectRegionId;
                originalUser.UserName = viewModel.UserName;
                originalUser.Email = viewModel.Email;
                originalUser.Rating = viewModel.Rating;
                originalUser.RoleId = viewModel.SelectRoleId;
                originalUser.PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt(12);
                originalUser.Password = BCrypt.Net.BCrypt.HashPassword(viewModel.Password, originalUser.PasswordSalt);
                originalUser.ModifiedDate = DateTime.Now;


                this.UnitOfWork.UserRepository.Update(originalUser);
                this.UnitOfWork.Save();

                return RedirectToAction("Index", "User");
            }
            catch
            {
                var originalUser = this.UnitOfWork.UserRepository.GetByID(viewModel.Id);
                var model = new ReUzze.Models.UserViewModel
                {
                    FirstName = originalUser.Person.FirstName,
                    SurName = originalUser.Person.SurName,
                    Street = originalUser.Person.Address.Street,
                    City = originalUser.Person.Address.City,
                    StreetNumber = originalUser.Person.Address.StreetNumber,
                    UserName = originalUser.UserName,
                    Email = originalUser.Email,
                    Rating = originalUser.Rating,
                    Regions = new SelectList(this.UnitOfWork.RegionRepository.Get(), "Id", "Name"),
                    SelectRegionId = originalUser.Person.Address.RegionId,
                    Roles = new SelectList(this.UnitOfWork.RoleRepository.Get(), "Id", "Name"),
                    SelectRoleId = originalUser.RoleId
                };
                return View(model);
            }
        }

        public ActionResult Message(Int32 id)
        {
            try
            {
                var user = this.UnitOfWork.UserRepository.GetByID(id);
                if (user == null)
                    throw new Exception();

                var model = new ReUzze.Models.MessageViewModel
                {
                    Email = user.Email
                };
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "User");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Message(ReUzze.Models.MessageViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var model = new ReUzze.Models.MessageViewModel();
                    return View(model);
                }

                User usr = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(usr.Email);
                mail.To.Add(viewModel.Email);
                mail.Subject = viewModel.Subject;
                mail.Body = viewModel.Body;

                SmtpServer.Send(mail);

                if (viewModel.Copy == true)
                {
                    mail.From = new MailAddress(usr.Email);
                    mail.To.Add(usr.Email);
                    mail.Subject = "Copy: " + viewModel.Subject;
                    mail.Body = "Copy: " + viewModel.Body;

                    SmtpServer.Send(mail);
                }

                return RedirectToAction("Message", "User");
            }
            catch 
            {

            }

            return RedirectToAction("Message", "User");

        }

        // DELETE ROLE POST
        [HttpPost]
        public ActionResult Delete(Int32 id)
        {
            try
            {
                var model = this.UnitOfWork.UserRepository.GetByID(id);
                if (model == null)
                    throw new Exception();

                // DELETE PERSON/USER/ADDRESS
                this.UnitOfWork.PersonRepository.Delete(model.Person);
                this.UnitOfWork.AddressRepository.Delete(model.Person.Address);
                this.UnitOfWork.UserRepository.Delete(model);
                this.UnitOfWork.Save();

                // IF USER DELETE IS EQUAL TO THE ONE LOGGED IN => LOG OFF
                if (User.Identity.Name == model.UserName)
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", "Home");
                }

                return Content(Boolean.TrueString);
            }
            catch
            {
                return Content(Boolean.FalseString);
            }
        }
    }
}
