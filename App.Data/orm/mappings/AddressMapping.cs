using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.Data.orm.mappings
{
    internal class AddressMapping : EntityTypeConfiguration<Address>
    {
        public AddressMapping()
            : base()
        {
            this.ToTable("addresses", "reuzze");

            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("address_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Street).HasColumnName("address_street").IsRequired().HasMaxLength(45);
            this.Property(t => t.City).HasColumnName("address_city").IsRequired().HasMaxLength(45);
            this.Property(t => t.Latitude).HasColumnName("address_lat").IsOptional();
            this.Property(t => t.Longitude).HasColumnName("address_lon").IsOptional();
            this.Property(t => t.StreetNumber).HasColumnName("address_streetnr").IsRequired();

            this.Property(t => t.RegionId).HasColumnName("region_id").IsRequired();

            //FOREIGN KEY MAPPINGS
            this.HasRequired(t => t.Region).WithMany(p => p.Addresses).HasForeignKey(f => f.RegionId).WillCascadeOnDelete(false);

        }
    }
}
