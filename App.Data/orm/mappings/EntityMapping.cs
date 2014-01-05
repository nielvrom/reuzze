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
    internal class EntityMapping : EntityTypeConfiguration<Entity>
    {
        public EntityMapping()
            : base()
        {
            this.ToTable("entities", "reuzze");

            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("entity_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Title).HasColumnName("entity_title").IsRequired().HasMaxLength(255);
            this.Property(t => t.Description).HasColumnName("entity_description").IsRequired();
            this.Property(t => t.StartTime).HasColumnName("entity_starttime").IsRequired();
            this.Property(t => t.EndTime).HasColumnName("entity_endtime").IsRequired();
            //this.Property(t => t.Type).HasColumnName("entity_type").IsRequired();
            this.Property(t => t.InstantSellingPrice).HasColumnName("entity_instantsellingprice").IsRequired();
            this.Property(t => t.ShippingPrice).HasColumnName("entity_shippingprice").IsOptional();
            this.Property(t => t.Condition).HasColumnName("entity_condition").IsRequired();
            this.Property(t => t.Views).HasColumnName("entity_views").IsOptional();
            this.Property(t => t.CreateDate).HasColumnName("entity_created").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(t => t.ModifiedDate).HasColumnName("entity_modified").IsOptional();
            this.Property(t => t.DeletedDate).HasColumnName("entity_deleted").IsOptional();

            this.Property(t => t.UserId).HasColumnName("user_id").IsRequired();
            this.Property(t => t.RegionId).HasColumnName("region_id").IsRequired();
            this.Property(t => t.CategoryId).HasColumnName("category_id").IsRequired();

            //FOREIGN KEY MAPPINGS
            this.HasRequired(t => t.User).WithMany(p => p.Entities).HasForeignKey(f => f.UserId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.Region).WithMany(p => p.Entities).HasForeignKey(f => f.RegionId);
            this.HasRequired(t => t.Category).WithMany(p => p.Entities).HasForeignKey(f => f.CategoryId);

            //MANY_TO_MANY MAPPINGS
            /*this.HasMany(t => t.Categories)
                .WithMany(t => t.Entities)
                .Map(mc =>
                {
                    mc.ToTable("entities_has_categories");
                    mc.MapLeftKey("entity_id");
                    mc.MapRightKey("category_id");
                });*/
            this.HasMany(t => t.Favorites)
                .WithMany(t => t.Favorites)
                .Map(mc =>
                {
                    mc.ToTable("favorites");
                    mc.MapLeftKey("entity_id");
                    mc.MapRightKey("user_id");
                });

            /*this.HasMany(t => t.Bids)
                .WithMany(t => t.Bids)
                .Map(mc =>
                {
                    mc.ToTable("bids");
                    mc.MapLeftKey("entity_id");
                    mc.MapRightKey("user_id");
                });*/
        }
    }
}
