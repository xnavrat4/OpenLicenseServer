using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenLicenseServerDAL.Models
{
    public abstract class BaseEntity
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
    }
}
