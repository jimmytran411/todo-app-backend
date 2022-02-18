using TodoApp.Models;
using TodoApp.Models.DTOs.Response;

namespace TodoApp.Utils
{
    public static class Extensions
    {
        public static TodoItemDTO AsTodoItemDTO(this TodoItem todoItem)
        {
            return new TodoItemDTO()
            {
                Name = todoItem.Name,
                Description = todoItem.Description,
                Status = todoItem.Status,
                CreatedAt = todoItem.CreatedAt,
                UpdatedAt = todoItem.UpdatedAt,

            };
        }
    }
}
