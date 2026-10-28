using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public bool Remove(Guid id) => _todos.RemoveAll(i => i.Id == id) > 0;
        public IEnumerable<TodoItem> Find(string substr) => _todos.Where(i => i.Title.Contains(substr ?? string.Empty, StringComparison.OrdinalIgnoreCase));
        public int Count => _todos.Count;
        public void Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Путь к файлу не указан.", nameof(path));
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(Todos, options);
            File.WriteAllText(path, json);
        }
        public void Load(string path)
        {
            if (string.IsNullOrWhiteSpace(path))throw new ArgumentException("Путь к файлу не указан.", nameof(path));

            if (!File.Exists(path))throw new FileNotFoundException("Файл с задачами не найден.", path);

            string json = File.ReadAllText(path);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var loadedItems =
                JsonSerializer.Deserialize<List<TodoItem>>(
                    json, options);

            if (loadedItems == null)
                throw new JsonException(
                    "Не удалось загрузить список задач.");

            _todos.Clear();
            _todos.AddRange(loadedItems);
        }
    
    }
}
