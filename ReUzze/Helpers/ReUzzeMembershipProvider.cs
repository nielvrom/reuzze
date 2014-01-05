using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using App.Data.orm;
using App.Models;

namespace ReUzze.Helpers
{
    public class ReUzzeMembershipProvider : MembershipProvider
    {
        #region unitOfWork
        private UnitOfWork _unitOfWork;
        public UnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                    _unitOfWork = new UnitOfWork();
                return _unitOfWork;
            }
        }
        #endregion

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            User user = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == username).FirstOrDefault();

            user.PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt(12);
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword, user.PasswordSalt);

            this.UnitOfWork.UserRepository.Update(user);
            this.UnitOfWork.Save();

            return true;

            //throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(string city, string street, int streetnumber, decimal latitude, decimal longitude, int regionid, string firstname, string surname, string username, string email, string password)
        {
            try
            {
                var adr = new Address();
                adr.City = city;
                adr.Street = street;
                adr.StreetNumber = streetnumber;
                adr.Latitude = latitude;
                adr.Longitude = longitude;
                adr.RegionId = regionid;

                this.UnitOfWork.AddressRepository.Insert(adr);

                var pers = new Person();
                pers.FirstName = firstname;
                pers.SurName = surname;
                pers.Address = adr;

                this.UnitOfWork.PersonRepository.Insert(pers);

                var roleid = this.UnitOfWork.RoleRepository.Get().Where(r => r.Name == "Administrator").FirstOrDefault().Id;

                var usr = new User();
                usr.UserName = username;
                usr.PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt(12);
                usr.Password = BCrypt.Net.BCrypt.HashPassword(password, usr.PasswordSalt);
                usr.Rating = 10;
                usr.Email = email;
                usr.RoleId = roleid;
                usr.Person = pers;

                this.UnitOfWork.UserRepository.Insert(usr);
                this.UnitOfWork.Save();
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
        }

        public void ChangeUser(string city, string street, int streetnumber, int regionid, string firstname, string surname, string address, string username, string email, string password)
        {

            var adr = new Address();
            adr.City = city;
            adr.Street = street;
            adr.StreetNumber = streetnumber;
            adr.RegionId = regionid;

            this.UnitOfWork.AddressRepository.Insert(adr);

            var pers = new Person();
            pers.FirstName = firstname;
            pers.SurName = surname;
            pers.Address = adr;

            this.UnitOfWork.PersonRepository.Insert(pers);

            var usr = new User();
            usr.UserName = username;
            usr.Password = password;
            usr.RoleId = 1;
            usr.Person = pers;

            this.UnitOfWork.UserRepository.Insert(usr);
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return 6;
            }

            //get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(password.Trim()) || string.IsNullOrEmpty(username.Trim()))
                return false;

            // COMPARE WITH USERNAME/PASSWORD IN DATABASE
            return this.UnitOfWork.UserRepository.Get().Any(user => (user.UserName == username.Trim()) && (BCrypt.Net.BCrypt.HashPassword(password, user.PasswordSalt) == user.Password));
        }
    }
}
