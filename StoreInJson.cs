using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuickAccess
{
    class Registers 
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }

    class StoreInJson
    {
        public static List<Registers> LoadItems()
        {
            List<Registers> registers = new List<Registers>();
            string filePath = VerifyFile();

            if (new FileInfo(filePath).Length > 0) 
            {
                string json = File.ReadAllText(filePath);
                registers = JsonConvert.DeserializeObject<List<Registers>>(json);
            }
            return registers;
        }

        public static void SaveItems(List<Registers> registers) 
        {
            string filePath = VerifyFile();
            string json = JsonConvert.SerializeObject(registers, Formatting.Indented);
            string path = filePath;
            File.WriteAllText(path, json);
        }

        public static void DeleteRegister(List<Registers> registers, string attribute)
        {
            Registers registerToRemove = registers.FirstOrDefault(r => r.Name == attribute);
            registers.Remove(registerToRemove);
            SaveItems(registers);
        }

        private static string VerifyFile() 
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(appPath, "important");
            string filePath = Path.Combine(folderPath, "registers.json");

            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(folderPath);
                File.Create(filePath).Close();
                System.Threading.Thread.Sleep(100);
            }
            return filePath;
        }
    }
}
