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
    internal class MediaMapping : EntityTypeConfiguration<Media>
    {
        public MediaMapping()
            : base()
        {
            this.ToTable("media", "reuzze");

            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("medium_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.URL).HasColumnName("medium_url").IsRequired();
            this.Property(t => t.MediaType).HasColumnName("medium_type").IsRequired();
            this.Property(t => t.MimeType).HasColumnName("medium_mimetype").IsRequired();
            this.Property(t => t.IsExternal).HasColumnName("medium_isexternal").IsOptional();

            this.Property(t => t.EntityId).HasColumnName("entity_id").IsRequired();

            // FOREIGN KEY MAPPING
            this.HasRequired(t => t.Entity).WithMany().HasForeignKey(f => f.EntityId);
        }
    }
}
