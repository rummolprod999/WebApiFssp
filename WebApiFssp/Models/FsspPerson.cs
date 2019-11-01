using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApiFssp.Models
{
    public class FsspPerson
    {
        [Required(ErrorMessage = "FirstName is empty")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "MiddleName is empty")]
        public string MiddleName { get; set; }
        
        public string LastName { get; set; }
        
        public List<int> Regions { get; set; }
        
    }
}