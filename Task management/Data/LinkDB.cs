using System.Text.Json;
using MyNotesApi.Models;

namespace MyNotesApi.Data
{
    public static class LinkDB
    {
        private static readonly string taskFilePath = "D:\\NEWMERK\\OneDrive\\NEWMERK\\PROGRAMMING\\C#\\MyNotes\\MyNotes\\MyNotes\\Notes.json";

        public static void SaveTasks(List<TaskItem> tasks)
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(taskFilePath, json);
        }

        public static List<TaskItem> LoadTasks()
        {
            if (!File.Exists(taskFilePath))
                return new List<TaskItem>();

            string json = File.ReadAllText(taskFilePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
    }
}
