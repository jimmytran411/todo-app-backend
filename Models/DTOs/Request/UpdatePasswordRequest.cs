using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.DTOs.Request
{
   public class UpdatePasswordRequest
   {

      [Required]
      public string NewPassword { get; set; }
   }
}
