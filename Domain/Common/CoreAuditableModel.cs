using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common
{
    public abstract class CoreAuditableModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsTransient()
        {
            return this.Id == default(Int32);
        }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}