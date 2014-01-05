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
    internal class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
            : base()
        {
            this.ToTable("users", "reuzze");

            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("user_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.UserName).HasColumnName("username").IsRequired().HasMaxLength(45);
            this.Property(t => t.Password).HasColumnName("password").IsRequired();
            this.Property(t => t.PasswordSalt).HasColumnName("salt").IsRequired();
            this.Property(t => t.Email).HasColumnName("user_email").IsRequired().HasMaxLength(255);
            this.Property(t => t.Rating).HasColumnName("user_rating").IsRequired();
            this.Property(t => t.CreatedDate).HasColumnName("user_created").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(t => t.ModifiedDate).HasColumnName("user_modified").IsOptional();
            this.Property(t => t.DeletedDate).HasColumnName("user_deleted").IsOptional();
            this.Property(t => t.LockedDate).HasColumnName("user_locked").IsOptional();
            this.Property(t => t.LastLoggedInDate).HasColumnName("user_lastlogin").IsOptional();

            this.Property(t => t.PersonId).HasColumnName("person_id").IsRequired();
            this.Property(t => t.RoleId).HasColumnName("role_id").IsRequired();

            //ONE-TO-ONE FOREIGN KEY MAPPING
            this.HasRequired(t => t.Person).WithMany().HasForeignKey(f => f.PersonId);
            this.HasRequired(t => t.Role).WithMany().HasForeignKey(f => f.RoleId);
        }
    }
}
