﻿using System;
using System.ComponentModel.DataAnnotations;

namespace API.CRUD.Models
{
    public class Person
    {
        public int Id { get; set; }

        public DateTime CreatedOn => new DateTime(2001, 12, 25, 14, 15, 16);

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [Range(1,150)]
        public int Age { get; set; }

        public string Nickname { get; set; }
    }
}