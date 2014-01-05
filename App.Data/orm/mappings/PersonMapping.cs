using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using App.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.orm.mappings
{
    internal class PersonMapping : EntityTypeConfiguration<Person>
    {
        public PersonMapping()
            : base()
        {
            this.ToTable("persons", "reuzze");

            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("person_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.FirstName).HasColumnName("person_firstname").IsRequired().HasMaxLength(45);
            this.Property(t => t.SurName).HasColumnName("person_surname").IsRequired().HasMaxLength(255);
            this.Property(t => t.CreateDate).HasColumnName("person_created").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(t => t.ModifiedDate).HasColumnName("person_modified").IsOptional();
            this.Property(t => t.DeletedDate).HasColumnName("person_deleted").IsOptional();

            this.Property(t => t.AddressId).HasColumnName("address_id").IsRequired();

            //FOREIGN KEY MAPPINGS
            this.HasRequired(t => t.Address).WithMany(p => p.Persons).HasForeignKey(f => f.AddressId).WillCascadeOnDelete(false);

        }
    }
}
