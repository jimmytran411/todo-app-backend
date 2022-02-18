using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models.DTOs.Request
{
    public class UpdateTodoDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
