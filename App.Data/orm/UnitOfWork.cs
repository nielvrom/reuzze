using App.Data.orm.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.Data.orm
{
    public class UnitOfWork
    {
        #region VARIABLES
        private GDMContext _context = new GDMContext();
        private GDMRepository<User> _userRepository;
        private GDMRepository<Person> _personRepository;
        private GDMRepository<Media> _mediaRepository;
        private GDMRepository<Entity> _entityRepository;
        private GDMRepository<Bid> _bidRepository;
        private GDMRepository<Category> _categoryRepository;
        private GDMRepository<Address> _addressRepository;
        private GDMRepository<Region> _regionRepository;
        private GDMRepository<Role> _roleRepository;
        #endregion

        #region Repository Properties
        public GDMRepository<User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new GDMRepository<User>(_context);
                return _userRepository;
            }
        }
        public GDMRepository<Person> PersonRepository
        {
            get
            {
                if (_personRepository == null)
                    _personRepository = new GDMRepository<Person>(_context);
                return _personRepository;
            }
        }
        public GDMRepository<Media> MediaRepository
        {
            get
            {
                if (_mediaRepository == null)
                    _mediaRepository = new GDMRepository<Media>(_context);
                return _mediaRepository;
            }
        }
        public GDMRepository<Entity> EntityRepository
        {
            get
            {
                if (_entityRepository == null)
                    _entityRepository = new GDMRepository<Entity>(_context);
                return _entityRepository;
            }
        }
        public GDMRepository<Bid> BidRepository
        {
            get
            {
                if (_bidRepository == null)
                    _bidRepository = new GDMRepository<Bid>(_context);
                return _bidRepository;
            }
        }
        public GDMRepository<Category> CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new GDMRepository<Category>(_context);
                return _categoryRepository;
            }
        }
        public GDMRepository<Address> AddressRepository
        {
            get
            {
                if (_addressRepository == null)
                    _addressRepository = new GDMRepository<Address>(_context);
                return _addressRepository;
            }
        }
        public GDMRepository<Region> RegionRepository
        {
            get
            {
                if (_regionRepository == null)
                    _regionRepository = new GDMRepository<Region>(_context);
                return _regionRepository;
            }
        }
        public GDMRepository<Role> RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new GDMRepository<Role>(_context);
                return _roleRepository;
            }
        }
        #endregion

        #region METHODS
        public void Save()
        {
            _context.SaveChanges();
        }
        #endregion
    }
}
