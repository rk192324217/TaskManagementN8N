using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
//using System.Threading.Tasks;

namespace MyNotes
{
    internal class LinkDB
    {

        private static string filepath = "C:\\Users\\RajeshKanna\\source\\repos\\MyNotes\\MyNotes\\Notes.json";
         public static void Save(List<Note> notes)
        {
            string json = JsonSerializer.Serialize(notes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filepath, json);

        }
        public static List<Note> Load()
        {
            if (!File.Exists(filepath))
            {
                return new List<Note>(); 
            }
            string json= File.ReadAllText(filepath);
            return JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
        }
    }
}
