using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.DTOs.Request
{
   public class CreateTodoRequest
   {
      [Required]
      public string Name { get; set; }
      public string Description { get; set; }

   }
}
