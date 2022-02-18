using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;
using TodoApp.Models.DTOs.Request;
using TodoApp.Models.DTOs.Response;
using TodoApp.Utils;

namespace TodoApp.Services
{
   public class TodoItemRepository : ITodoItemRepository
   {
      private readonly TodoAppContext todoAppContext;

      public TodoItemRepository(TodoAppContext todoAppContext)
      {
         this.todoAppContext = todoAppContext;
      }

      public List<TodoItem> GetTodosWithStatus(string userId, StatusTypes status)
      {
         return todoAppContext.TodoItems.Where(t => t.UserId == userId).Where(t => t.Status == status).ToList();
      }

      public List<TodoItem> GetTodos(string userId)
      {
         return todoAppContext.TodoItems.Where(t => t.UserId == userId).ToList();
      }

      public TodoItem GetTodoById(Guid id)
      {
         return todoAppContext.TodoItems.Where(t => t.Id == id).FirstOrDefault();
      }

      public async Task<TodoItemDTO> CreateTodoAsync(CreateTodoRequest todo, string userId)
      {
         var newTodo = (await todoAppContext.TodoItems.AddAsync(new TodoItem()
         {
            Name = todo.Name,
            Description = todo.Description,
            UserId = userId,
            Status = StatusTypes.NotStarted,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Id = Guid.NewGuid()
         })).Entity;

         await todoAppContext.SaveChangesAsync();

         return newTodo.AsTodoItemDTO();
      }

      public async Task<TodoItemDTO> UpdateTodoAsync(UpdateTodoRequest updateField, TodoItem todo)
      {

         if (updateField.Name != null) todo.Name = updateField.Name;
         if (updateField.Description != null) todo.Description = updateField.Description;
         if (updateField.Status != null)
         {
            _ = Enum.TryParse(updateField.Status, out StatusTypes statusTypes);
            todo.Status = statusTypes;
         }
         todo.UpdatedAt = DateTime.Now;

         await todoAppContext.SaveChangesAsync();
         return todo.AsTodoItemDTO();
      }

      public async Task<TodoItemDTO> DeleteTodoAsync(TodoItem todo)
      {
         todoAppContext.TodoItems.Remove(todo);
         await todoAppContext.SaveChangesAsync();

         return todo.AsTodoItemDTO();
      }
   }
}
