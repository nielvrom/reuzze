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
    internal class RoleMapping : EntityTypeConfiguration<Role>
    {
        public RoleMapping()
            : base()
        {
            this.ToTable("roles", "reuzze");

            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("role_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name).HasColumnName("role_name").IsRequired().HasMaxLength(45);
            this.Property(t => t.Description).HasColumnName("role_description").IsOptional().HasMaxLength(255);
            this.Property(t => t.CreateDate).HasColumnName("role_created").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(t => t.ModifiedDate).HasColumnName("role_modified").IsOptional();
            this.Property(t => t.DeletedDate).HasColumnName("role_deleted").IsOptional();

            //FOREIGN KEYS
            /*this.HasMany(t => t.Users)
                .WithMany(t => t.Roles)
                .Map(mc =>
                {
                    mc.ToTable("users_has_roles");
                    mc.MapLeftKey("role_id");
                    mc.MapRightKey("user_id");
                });*/

        }
    }
}
