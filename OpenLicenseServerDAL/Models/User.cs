using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenLicenseServerDAL.Enums;

namespace OpenLicenseServerDAL.Models;

public class User : BaseEntity
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
    
    public int AddedById { get; set; }
    
    [ForeignKey(nameof(AddedById))]
    public User AddedBy { get; set; }
    
    public DateTime AddedOn { get; set; }
    
    public UserStatus UserStatus { get; set; }
}