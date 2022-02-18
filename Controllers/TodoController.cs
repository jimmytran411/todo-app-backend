using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApp.Models;
using TodoApp.Models.DTOs.Response;
using TodoApp.Models.DTOs.Request;
using TodoApp.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApp.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   [Authorize]
   public class TodoController : ControllerBase
   {
      private readonly TodoAppContext _todoAppContext;
      private readonly ITodoItemRepository itemRespository;

      public TodoController(TodoAppContext context, ITodoItemRepository todoItemRespository)
      {
         _todoAppContext = context;
         itemRespository = todoItemRespository;
      }
      // GET: api/<TodoController>
      [HttpGet]
      public IActionResult GetTodos([FromQuery(Name = "status")] string status)
      {
         var userId = User.FindFirstValue("Id");

         if (status is null)
         {
            return Ok(new ResponseResult<List<TodoItem>>()
            {
               Success = true,
               Payload = itemRespository.GetTodos(userId)
            });
         }

         _ = Enum.TryParse(status, out StatusTypes statusTypes);


         return Ok(new ResponseResult<List<TodoItem>>()
         {
            Success = true,
            Payload = itemRespository.GetTodosWithStatus(userId, statusTypes)
         });
      }

      // GET api/<TodoController>/:id
      [HttpGet("{id}")]
      public IActionResult GetTodoById(Guid id)
      {
         var todo = itemRespository.GetTodoById(id);

         if (todo == null)
         {
            return BadRequest(new ResponseResult<string>()
            {
               Success = false,
               Errors = new List<string> { "Item not found" }
            });
         }

         return Ok(new ResponseResult<TodoItem>()
         {
            Success = true,
            Payload = todo
         });
      }

      // POST api/<TodoController>
      [HttpPost]
      public async Task<IActionResult> CreateTodo([FromBody] CreateTodoRequest todo)
      {
         var userId = User.FindFirstValue("Id");
         var newTodo = await itemRespository.CreateTodoAsync(todo, userId);

         return Ok(new ResponseResult<TodoItemDTO>() { Success = true, Payload = newTodo });
      }

      // PUT api/<TodoController>/:id
      [HttpPut("{id}")]
      public async Task<IActionResult> UpdateTodo(Guid id, [FromBody] UpdateTodoRequest updateField)
      {
         var userId = User.FindFirstValue("Id");
         var todo = _todoAppContext.TodoItems.Where(item => item.UserId == userId).Where(item => item.Id == id).FirstOrDefault();
         if (todo == null)
         {
            return BadRequest(new ResponseResult<string>()
            {
               Success = false,
               Errors = new List<string> { "Item not found" }
            });
         }

         var updatedTodo = await itemRespository.UpdateTodoAsync(updateField, todo);

         return Ok(new ResponseResult<TodoItemDTO>()
         {
            Success = true,
            Payload = updatedTodo
         });
      }

      // DELETE api/<TodoController>/:id
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteTodoSync(Guid id)
      {
         var todo = _todoAppContext.TodoItems.Where(item => item.Id == id).FirstOrDefault();
         if (todo == null)
         {
            return BadRequest(new ResponseResult<string>()
            {
               Success = false,
               Errors = new List<string> { "Item not found" }
            });
         }

         var deletedTodo = await itemRespository.DeleteTodoAsync(todo);

         return Ok(new ResponseResult<TodoItemDTO>()
         {
            Success = true,
            Payload = deletedTodo
         });
      }
   }
}
