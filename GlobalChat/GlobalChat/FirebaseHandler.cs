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
    // gets all the data from https://globalchat-4a89f-default-rtdb.europe-west1.firebasedatabase.app/Database.json and phases it to an object that i have defined in "Models/DatabaseStructure.cs"
    internal static DatabaseStructure GetData()
    {
        // creates a client to make a request for the data
        var client = new HttpClient();
        var response = client.GetAsync("https://globalchat-4a89f-default-rtdb.europe-west1.firebasedatabase.app/Database.json");

        // gets response from the firebase database and saves the response to a string called "responseStr"
        string responseStr = response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        // this is a error guard that checks if the response was empty and if so then return a null object
        if (string.IsNullOrEmpty(responseStr) || responseStr == "\"\"") return null;

        // deserializes the response from a json format to a C# object called "DatabaseStructure"
        return JsonConvert.DeserializeObject<DatabaseStructure>(responseStr);
    }

    // function to add a message to the database
    public static void AddMessage(string name, string content)
    {
        // gets the data object from the database
        var data = GetData();

        // if the data was null, then return a new list and if not null then formats the object into a list and saves it to "dumbList"
        var dumbList = (data != null) ? data.Messages.ToList() : new List<Message>();
        
        // adds a new message object that was defined in "Models/Message.cs"
        dumbList.Add(
            new Message() // creates the message object
            { 
                // assigns the values in the message object
                Name = name, 
                Content = content, 
                DatePosted = DateTimeOffset.Now.ToUnixTimeSeconds() // gets and sets the current time (unix timestamp)
            });

        // converts the "dumbList" into the DatabaseStructure with all the messages from before. 
        var output = new DatabaseStructure() 
        { 
            Messages = dumbList
            .Where(x=>x.DatePosted < DateTimeOffset.Now.ToUnixTimeSeconds() + 2000) // only allows messages that is newer than 33 minutes to be saved
            .ToArray() 
        };

        string json = JsonConvert.SerializeObject(output); // serializes the "output" back into a json object
        SendData(json); // I refuse explaining this line of code
    }

    // sends data to "https://globalchat-4a89f-default-rtdb.europe-west1.firebasedatabase.app"
    public static void SendData(string json)
    {
        // create a client that handles the requests
        var client = new HttpClient();

        // sets the base address to "https://globalchat-4a89f-default-rtdb.europe-west1.firebasedatabase.app"
        client.BaseAddress = new Uri("https://globalchat-4a89f-default-rtdb.europe-west1.firebasedatabase.app");

        // tells the client to send a json object
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // finally tells the client to send the data
        client.PutAsync("/Database.json", content);
    }
}
