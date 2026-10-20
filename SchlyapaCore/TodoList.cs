using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchlyapaCore
{
    public class TodoList
    {
        private readonly List<TodoItem> _todos = new();
        public IReadOnlyList<TodoItem> Todos => _todos.AsReadOnly();
        public TodoItem Add(string title)
        {
            var item = new TodoItem(title);
            _todos.Add(item);
            return item;
        }
        public bool Remove(Guid id) => _todos.RemoveAll(i =>  i.Id == id) > 0;
        public IEnumerable<TodoItem> Find(string substr) => _todos.Where(i => i.Title.Contains(substr ?? string.Empty, StringComparison.OrdinalIgnoreCase));
        public int Count => _todos.Count;
    }
}
