using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Models;
using TodoApp.Models.DTOs.Request;
using TodoApp.Models.DTOs.Response;

namespace TodoApp.Services
{
   public interface ITodoItemRepository
   {
      List<TodoItem> GetTodos(string userId);
      List<TodoItem> GetTodosWithStatus(string userId, StatusTypes status);
      TodoItem GetTodoById(Guid id);
      Task<TodoItemDTO> CreateTodoAsync(CreateTodoDTO todo, string userId);
      Task<TodoItemDTO> UpdateTodoAsync(UpdateTodoDTO updateField, TodoItem todo);
      Task<TodoItemDTO> DeleteTodoAsync(TodoItem todo);
   }
}
