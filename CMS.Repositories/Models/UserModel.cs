using CMS.Repositories.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Repositories.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [StringLength(50, MinimumLength = 4)]
        [RegularExpression("^[a-zA-Z\\s.]+$", ErrorMessage = "Only alphabets and space are allowed")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please Enter Email")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please Enter Password")]
        [StringLength(20, ErrorMessage = "Must be between 8 and 20 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(20, ErrorMessage = "Must be between 8 and 20 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Compare Password doesn't match")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please Enter Mobile Number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [StringLength(10, MinimumLength = 10)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Please choose image")]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        [MaxFileSize(20 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".gif", ".jpeg" })]
        public IFormFile ImageFile { get; set; }

        public string Image { get; set; }


        [StringLength(30, MinimumLength = 4)]
        [Required]
        [RegularExpression("^[a-zA-Z\\s.]+$", ErrorMessage = "Only alphabets and space are allowed")]
        public string Designation { get; set; }

        
        public int CountryId { get; set; }
        

        [Required(ErrorMessage = "Please select country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

    }
}
