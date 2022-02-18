using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.DTOs.Request
{
    public class UpdatePasswordDTO
    {
   
        [Required]
        public string NewPassword { get; set; }
    }
}
