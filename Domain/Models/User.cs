using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Users")]
    public class User : AuditableModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
                {
                    return string.Format("{0}, {1}", this.LastName, this.FirstName);
                }
                if (!string.IsNullOrWhiteSpace(FirstName))
                {
                    return this.FirstName;
                }
                return this.UserName;
            }
        }

        public string Email { get; set; }

        //public virtual IList<UserRole> Roles { get; set; }

        public bool IsActive { get; set; }
    }
}