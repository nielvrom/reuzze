using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Category
    {
        public Int16 Id { get; set; }
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(45)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category Description is required")]
        [StringLength(255)]
        [DisplayName("Category Description")]
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }

        public Nullable<Int16> ParentId { get; set; }

        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<Entity> Entities { get; set; }
    }
}
