using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
