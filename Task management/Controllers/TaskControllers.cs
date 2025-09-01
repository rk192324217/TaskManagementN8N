using Microsoft.AspNetCore.Mvc;
using MyNotesApi.Models;
using MyNotesApi.Data;
using System.Text;
using System.Text.Json;

namespace MyNotesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private static List<TaskItem> tasks = LinkDB.LoadTasks();
        private static int nextId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;

        private readonly IHttpClientFactory _httpClientFactory;

        public TasksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetAll()
        {
            return Ok(tasks);
        }

        // GET api/tasks/1
        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetById(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // POST api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem newTask)
        {
            newTask.Id = nextId++;
            tasks.Add(newTask);
            LinkDB.SaveTasks(tasks);

            // 🔗 Send data to n8n webhook
            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(newTask);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Replace this with your actual webhook URL from n8n
            string webhookUrl = "https://rk8115939.app.n8n.cloud/webhook-test/receive-task";

            await client.PostAsync(webhookUrl, content);

            return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
        }

        // PUT api/tasks/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.AssignedTo = updatedTask.AssignedTo;
            task.IsCompleted = updatedTask.IsCompleted;

            LinkDB.SaveTasks(tasks);

            // 🔗 Optionally notify n8n on update
            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string webhookUrl = "https://rk8115939.app.n8n.cloud/webhook/receive-task";
            await client.PostAsync(webhookUrl, content);

            return NoContent();
        }

        // DELETE api/tasks/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            tasks.Remove(task);
            LinkDB.SaveTasks(tasks);
            return NoContent();
        }
    }
}
