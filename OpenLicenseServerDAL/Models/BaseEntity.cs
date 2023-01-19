using System.ComponentModel.DataAnnotations;

namespace FoodliveryDAL.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
