using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.DTOs.Request
{
   public class UpdateTodoRequest
   {
      public string Name { get; set; }
      public string Description { get; set; }
      public string Status { get; set; }
   }
}
