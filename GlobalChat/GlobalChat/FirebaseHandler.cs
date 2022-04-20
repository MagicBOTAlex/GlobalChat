using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GlobalChat.Models;
using Newtonsoft.Json;

public static class FirebaseHandler 
{
    internal static DatabaseStructure GetData()
    {
        var client = new HttpClient();
        var response = client.GetAsync("https://globalchat-4a89f-default-rtdb.europe-west1.firebasedatabase.app/Database.json");

        string responseStr = response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (string.IsNullOrEmpty(responseStr) || responseStr == "\"\"") return null;

        return JsonConvert.DeserializeObject<DatabaseStructure>(responseStr);
    }

    public static void AddMessage(string name, string content)
    {
        var data = GetData();
        var dumbList = (data != null) ? data.Messages.ToList() : new List<Message>();
        dumbList.Add(new Message() { Name = name, Content = content, DatePosted = DateTimeOffset.Now.ToUnixTimeSeconds() });
        var output = new DatabaseStructure() { Messages = dumbList.ToArray() };

        string json = JsonConvert.SerializeObject(output);
        SendData(json);

        //firebase.Child("database").PatchAsync(JsonConvert.SerializeObject(data)).Wait();
    }

    public static void SendData(string json)
    {

        var client = new HttpClient();
        client.BaseAddress = new Uri("https://globalchat-4a89f-default-rtdb.europe-west1.firebasedatabase.app");
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        client.PutAsync("/Database.json", content);
    }
}
