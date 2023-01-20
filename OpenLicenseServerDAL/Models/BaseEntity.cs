using System.ComponentModel.DataAnnotations;

namespace OpenLicenseServerDAL.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
