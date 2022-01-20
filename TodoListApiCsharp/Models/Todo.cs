using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApiCsharp.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
