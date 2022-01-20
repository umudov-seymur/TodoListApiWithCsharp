using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApiCsharp.DTOs;

namespace TodoListApiCsharp.Validations.Todo
{
    public class TodoValidation : AbstractValidator<TodoDto>
    {
        public TodoValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.IsComplete).NotNull().WithMessage("Completed status true or false");
        }
    }
}
