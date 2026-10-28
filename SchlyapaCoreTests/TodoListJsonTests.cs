using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SchlyapaCore;

namespace SchlyapaCoreTests
{
    public class TodoListJsonTests
    {

        [Fact]
        public void Save_CreatesJsonFile()
        {
            var list = new TodoList();
            list.Add("Купить продукты");

            string path = Path.GetTempFileName();

            try
            {
                list.Save(path);

                Assert.True(File.Exists(path));

                string json = File.ReadAllText(path);

                var savedItems =
                    System.Text.Json.JsonSerializer.Deserialize<
                        List<TodoItem>>(json);

                Assert.NotNull(savedItems);
                Assert.Single(savedItems);
                Assert.Equal("Купить продукты", savedItems[0].Title);
            }
            finally
            {
                File.Delete(path);
            }
        }


        [Fact]
        public void Load_RestoresTasks()
        {
            var list = new TodoList();
            var item = list.Add("Сделать домашнее задание");

            string path = Path.GetTempFileName();

            try
            {
                list.Save(path);

                var loadedList = new TodoList();
                loadedList.Load(path);

                Assert.Equal(1, loadedList.Count);
                Assert.Equal(
                    "Сделать домашнее задание",
                    loadedList.Todos[0].Title);
                Assert.Equal(
                    item.Id,
                    loadedList.Todos[0].Id);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void Load_RestoresCompletedStatus()
        {
            var list = new TodoList();

            var item = list.Add("Пройти тестирование");
            item.MarkDone();

            string path = Path.GetTempFileName();

            try
            {
                list.Save(path);

                var loadedList = new TodoList();
                loadedList.Load(path);

                Assert.True(loadedList.Todos[0].IsDone);
                Assert.Equal(item.Id, loadedList.Todos[0].Id);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void Load_ThrowsException_WhenFileNotFound()
        {
            var list = new TodoList();

            string path = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid() + ".json");

            Assert.Throws<FileNotFoundException>(
                () => list.Load(path));
        }

        [Fact]
        public void Load_ThrowsException_WhenJsonInvalid()
        {
            var list = new TodoList();

            string path = Path.GetTempFileName();

            try
            {
                File.WriteAllText(path, "{ invalid json }");

                Assert.Throws<JsonException>(
                    () => list.Load(path));
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
