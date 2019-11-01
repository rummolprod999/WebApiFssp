using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApiFssp.Models
{
    public class FsspPerson
    {
        [Required(ErrorMessage = "Укажите имя пользователя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Укажите отчество пользователя")]
        public string MiddleName { get; set; }
        
        [Required(ErrorMessage = "Укажите фамилию пользователя")]
        public string LastName { get; set; }
        
    }
}