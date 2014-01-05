using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using App.Data.orm;
using App.Models;

namespace ReUzze.Helpers
{
    public class ReUzzeRoleProvider : RoleProvider
    {
        #region
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

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

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

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            User user = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == username).FirstOrDefault();
            Role role = user.Role;
            /*Role role = this.repository.GetRoleForUser(username);
            if (!this.repository.RoleExists(role))
                return new string[] { string.Empty };*/

            return new string[] { role.Name };

            //throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            User user = this.UnitOfWork.UserRepository.Get().Where(u => u.UserName == username).FirstOrDefault();
            Role role = this.UnitOfWork.RoleRepository.Get().Where(r => r.Name == roleName).FirstOrDefault();

            /*if (!repository.UserExists(user))
                return false;
            if (!repository.RoleExists(role))
                return false;*/

            return user.Role.Id == role.Id;

            //throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
