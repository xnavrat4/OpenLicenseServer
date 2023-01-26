﻿using System.ComponentModel.DataAnnotations;

namespace OpenLicenseServerDAL.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
