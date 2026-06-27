using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using CocoQuest.Models;

namespace CocoQuest.Services
{
    public class SaveService
    {
        private readonly string path = "save.json";

        public void Save(UserState state)
        {
            var json = JsonSerializer.Serialize(state, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);
        }

        public UserState Load()
        {
            if (!File.Exists(path))
                return new UserState();

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<UserState>(json) ?? new UserState();
        }
    }
}
