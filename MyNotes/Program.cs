using MyNotes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
class Note
{
    public int Id { get; set; }
    public  string Title { get; set; } = "";
    public  string Content { get; set; } = "";
}

class Program
{
    static List<Note> notes = new List<Note>();
    static int nextId = 1;
    

    static void Main()

    {
        notes = LinkDB.Load();
        if (notes.Count>0)
        {
            nextId = notes[notes.Count - 1].Id + 1;
        }

        while (true)
        {
            Console.WriteLine("\n--- NOTE APP ---");
            Console.WriteLine("1.Add Note \n2.View Note \n3.Update Note \n4.Delete Note \n5.Exit");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddNote();
                    break;
                case "2":
                    ViewNote();
                    break;
                case "3":
                    UpdateNote();
                    break;
                case "4":
                    DeleteNote();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("You have entered a Wrong Input");
                    break;
            }
        }


    }

    static async void AddNote()
    {
        Console.WriteLine("Enter the Title");
         string ?  title = Console.ReadLine();
        Console.WriteLine("Enter the Content");
        string? content = Console.ReadLine();

        Note add_note = new Note();
        add_note.Id = nextId++;
        add_note.Content = content;
        add_note.Title = title;
        notes.Add(add_note);
        LinkDB.Save(notes);
        Console.WriteLine("Note Added Locally");
        await SendNoteToN8n(add_note);
    }
    static void ViewNote()
    {
        if (notes.Count == 0)
        {
            Console.WriteLine("There is no Notes");
        }
        foreach (var note in notes)
        {
            Console.WriteLine($"({note.Id}). {note.Title} - {note.Content}");
        }

    }
    static void UpdateNote()
    {
        Console.WriteLine("Enter the Id of the note to be updated");
        if(!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("The Id is incorrect");
        }
        var note= notes.Find(n=>n.Id==id);
        if (note == null)
        {
            Console.WriteLine("The Id didn't match");
            return;
        }
        Console.WriteLine($"Replace {note.Title} with ... ");
        var title= Console.ReadLine();
        if (!string.IsNullOrEmpty(title))
        {
            note.Title = title;
        }
        var content= Console.ReadLine();
        if (!string.IsNullOrEmpty(content))
        {
            note.Content = content;
        }
        LinkDB.Save(notes);
        Console.WriteLine($"Note {note.Id} has been uploaded");
    }

    static void DeleteNote()
    {
        Console.WriteLine("Enter the Id of the note to delete");
        if(int.TryParse(Console.ReadLine(),out int id))
        {
            var note = notes.Find(n => n.Id == id);
            if (note != null)
            {
                notes.Remove(note);
                LinkDB.Save(notes);
                Console.WriteLine("Note Deleted");
            }
            else
            {
                Console.WriteLine("Id did not match");
            }
        }
        else
        {
            Console.WriteLine("Incorrect Input");
        }

    }
    static async Task SendNoteToN8n(Note note)
    {
        using (HttpClient client =new HttpClient())
        {
            var payload = new
            {
                title = note.Title,
                content = note.Content,
            };
            string json =JsonSerializer.Serialize(payload);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            string n8nWebhookUrl = "https://rk8115939.app.n8n.cloud/webhook-test/addNote";
            try
            {
                HttpResponseMessage response = await client.PostAsync(n8nWebhookUrl, data);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Note Sent");
                }
                else
                {
                    Console.WriteLine("Failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

}