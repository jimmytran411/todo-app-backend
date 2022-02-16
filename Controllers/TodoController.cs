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
            }) ;
        }

        // GET api/<TodoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

        // PUT api/<TodoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TodoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
