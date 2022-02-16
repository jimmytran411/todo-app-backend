using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApp.Models;
using TodoApp.Models.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly TodoAppContext _todoAppContext;

        public TodoController(TodoAppContext context)
        {
            _todoAppContext = context;
        }
        // GET: api/<TodoController>
        [HttpGet]
        public IActionResult GetTodos([FromQuery(Name = "status")] string? status)
        {
            var userId = User.FindFirstValue("Id");

            _ = Enum.TryParse(status, out StatusTypes statusTypes);

            var todoList = _todoAppContext.TodoItems
                .Where(item => item.UserId == userId)
                .Where(item => item.Status == statusTypes)
                .ToArray();

            return Ok(new ResponseResult<TodoItem[]>()
            {
                Success = true,
                Payload = todoList
            });
        }

        // GET api/<TodoController>/:id
        [HttpGet("{id}")]
        public IActionResult GetTodoById(Guid id)
        {
            var todo = _todoAppContext.TodoItems
                .Where(item => item.Id == id).FirstOrDefault();

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
        public async Task<IActionResult> CreateTodo([FromBody] CreateTodoDTO todo)
        {
            var userId = User.FindFirstValue("Id");
            var newTodo = await _todoAppContext.TodoItems.AddAsync(new TodoItem()
            {
                Name = todo.Name,
                Description = todo.Description,
                UserId = userId,
                Status = StatusTypes.NotStarted,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Id = Guid.NewGuid()
            });

            if (newTodo == null)
            {
                return BadRequest(new ResponseResult<string>()
                {
                    Success = false,
                    Errors = new List<string>()
                        {
                            "Something went wrong"
                        }
                });
            }

            await _todoAppContext.SaveChangesAsync();

            return Ok(new ResponseResult<string>() { Success = true });
        }

        // PUT api/<TodoController>/:id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(Guid id, [FromBody] UpdateTodoDTO updateField)
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


            if (updateField.Name != null) todo.Name = updateField.Name;
            if (updateField.Description != null) todo.Description = updateField.Description;
            if (updateField.Status != null)
            {
                _ = Enum.TryParse(updateField.Status, out StatusTypes statusTypes);
                todo.Status = statusTypes;
            }
            todo.UpdatedAt = DateTime.Now;
            await _todoAppContext.SaveChangesAsync();

            return Ok(new ResponseResult<string>() 
            {
                Success = true
            });
        }

        // DELETE api/<TodoController>/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoSync(Guid id)
        {
           var todo =  _todoAppContext.TodoItems.Where(item => item.Id == id).FirstOrDefault();
            if (todo == null)
            {
                return BadRequest(new ResponseResult<string>()
                {
                    Success = false,
                    Errors = new List<string> { "Item not found" }
                });
            }

            _todoAppContext.TodoItems.Remove(todo);
            await _todoAppContext.SaveChangesAsync();

            return Ok(new ResponseResult<string>()
            {
                Success = true
            });
        }
    }
}
