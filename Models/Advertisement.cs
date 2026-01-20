using System;
using System.ComponentModel.DataAnnotations;

namespace BnsBazarApp.Models
{
    public class Advertisement
    {
        public int Id { get; set; }

        [Required]
        public string Category { get; set; }

        [Required(ErrorMessage = "शीर्षक आवश्यक छ")]
        public string Title { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public string? Module { get; set; }

        [Required(ErrorMessage = "फोन नम्बर अनिवार्य छ")]
        [Phone(ErrorMessage = "फोन नम्बर गलत छ")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "इमेल गलत छ")]
        public string? Email { get; set; }

        public string? Agency { get; set; }

        [Required(ErrorMessage = "मूल्य आवश्यक छ")]
        public decimal Price { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public string? Image1Url { get; set; }
        public string? Image2Url { get; set; }
        public string? Image3Url { get; set; }
        public string? Image4Url { get; set; }
        public string? Image5Url { get; set; }

        public string? VideoUrl { get; set; }
    }
}