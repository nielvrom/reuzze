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
    internal class BidMapping : EntityTypeConfiguration<Bid>
    {
        public BidMapping()
            : base()
        {
            this.ToTable("bids", "reuzze");

            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasColumnName("bid_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.UserId).HasColumnName("user_id").IsRequired();
            this.Property(t => t.EntityId).HasColumnName("entity_id").IsRequired();

            this.Property(t => t.Amount).HasColumnName("bid_amount").IsRequired();
            this.Property(t => t.Date).HasColumnName("bid_date").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            // FOREIGN KEYS MAPPING
            this.HasRequired(t => t.User).WithMany(p => p.Bids).HasForeignKey(f => f.UserId);
            this.HasRequired(t => t.Entity).WithMany(p => p.Bids).HasForeignKey(f => f.EntityId);

        }
    }
}
