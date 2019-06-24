using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table ("Supplier")]
    public class Supplier : EntityBase
    {
        [Required]
        [MaxLength (40)]
        public string Name { get; set; }

        [Required]
        [MaxLength (255)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        //[Required]
        //public int ProductID { get; set; }

        //public IEnumerable Product { get; set; }
    }
}
