using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApiCsharp.DAL;
using TodoListApiCsharp.DTOs;
using TodoListApiCsharp.Models;
using TodoListApiCsharp.Validations.Todo;

namespace TodoListApiCsharp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            List<TodoDto> todos = new List<TodoDto>();
            var dbTodos = await _context.Todos.ToListAsync();

            foreach (var todo in dbTodos)
            {
                todos.Add(new TodoDto
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    IsComplete = todo.IsComplete
                });
            }

            return Ok(todos);
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo is null) return StatusCode(StatusCodes.Status404NotFound);

            return new TodoDto
            {
                Id = todo.Id,
                Title = todo.Title,
                IsComplete = todo.IsComplete
            };
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(int id, TodoDto todo)
        {
            TodoValidation validator = new TodoValidation();
            var result = validator.Validate(todo);
            if (!result.IsValid) BadRequest(result.Errors);

            var dbTodo = await _context.Todos.FindAsync(id);
            if (dbTodo is null) return StatusCode(StatusCodes.Status404NotFound);

            dbTodo.Title = todo.Title;
            dbTodo.IsComplete = todo.IsComplete;

            _context.Todos.Update(dbTodo);
            await _context.SaveChangesAsync();

            return Ok(dbTodo);
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoDto>> PostTodo(TodoDto todo)
        {
            TodoValidation validator = new TodoValidation();
            var result = validator.Validate(todo);
            if (!result.IsValid) BadRequest(result.Errors);

            var newTodo = new Todo
            {
                Id = todo.Id,
                Title = todo.Title,
                IsComplete = todo.IsComplete
            };
            _context.Todos.Add(newTodo);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodo", new { id = newTodo.Id }, todo);
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoDto>> DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo is null) return StatusCode(StatusCodes.Status404NotFound);

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
