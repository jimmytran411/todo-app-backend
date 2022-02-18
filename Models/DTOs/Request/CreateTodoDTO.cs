using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.DTOs.Request
{
    public class CreateTodoDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
