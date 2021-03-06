using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.DTOs.Request
{
   public class RegistrationRequest
   {
      [Required]
      [EmailAddress]
      public string Email { get; set; }
      [Required]
      public string Password { get; set; }
   }
}
