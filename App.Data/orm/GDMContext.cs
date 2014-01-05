using App.Data.orm.mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.orm
{
    public class GDMContext:DbContext
    {
        public GDMContext() : base("reuzzeCS") 
        {
            //base.Configuration.LazyLoadingEnabled = false; 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //REMOVE STANDARD MAPPING IN ENTITY FRAMEWORK
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //REGISTER MAPPERS
            modelBuilder.Configurations.Add(new UserMapping());
            modelBuilder.Configurations.Add(new PersonMapping());
            modelBuilder.Configurations.Add(new RoleMapping());

            modelBuilder.Configurations.Add(new EntityMapping());
            modelBuilder.Configurations.Add(new MediaMapping());
            modelBuilder.Configurations.Add(new BidMapping()); 
            modelBuilder.Configurations.Add(new CategoryMapping());
            modelBuilder.Configurations.Add(new AddressMapping());
            modelBuilder.Configurations.Add(new RegionMapping()); 
        }
    }
}
