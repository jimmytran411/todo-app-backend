using System;

namespace TodoApp.Models.DTOs.Response
{
    public class TodoItemDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public StatusTypes Status { get; set; }
    }
}
