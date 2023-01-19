using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodliveryDAL.Models;

public class Customer : BaseEntity
{
    [MaxLength(64)]
    public string FirstName { get; set; }
    [MaxLength(64)]
    public string LastName { get; set; }
    [Phone]
    [MaxLength(20)]
    public string PhoneNumber { get; set; }
    [EmailAddress]
    [MaxLength(64)]
    public string Email { get; set; }
    
}