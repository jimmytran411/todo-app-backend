using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.DTOs
{
    public class UpdatePasswordDTO
    {
   
        [Required]
        public string NewPassword { get; set; }
    }
}
