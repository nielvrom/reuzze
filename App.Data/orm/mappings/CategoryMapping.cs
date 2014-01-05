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
    internal class CategoryMapping : EntityTypeConfiguration<Category>
    {
        public CategoryMapping()
            : base()
        {
            this.ToTable("categories", "reuzze");

            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("category_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name).HasColumnName("category_name").IsRequired().HasMaxLength(45);
            this.Property(t => t.Description).HasColumnName("category_description").IsRequired().HasMaxLength(255);
            this.Property(t => t.CreateDate).HasColumnName("category_created").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(t => t.ModifiedDate).HasColumnName("category_modified").IsOptional();
            this.Property(t => t.DeletedDate).HasColumnName("category_deleted").IsOptional();
            this.Property(t => t.ParentId).HasColumnName("category_parentid").IsOptional();

            //FOREIGN KEYS
            this.HasOptional(t => t.ParentCategory).WithMany(t => t.ChildCategories).HasForeignKey(t => t.ParentId);

        }
    }
}
