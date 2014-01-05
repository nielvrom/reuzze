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
    internal class RegionMapping : EntityTypeConfiguration<Region>
    {
        public RegionMapping()
            : base()
        {
            this.ToTable("regions", "reuzze");

            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("region_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name).HasColumnName("region_name").IsRequired().HasMaxLength(45);

        }
    }
}
