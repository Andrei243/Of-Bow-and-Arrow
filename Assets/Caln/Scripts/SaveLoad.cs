using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Calin.Scripts
{
    class SaveLoad : MonoBehaviour
    {
        public struct SaveObject
        {
            public SaveObject(int checkpoint, List<int> deadEnemies)
            {
                playerCheckpoint = checkpoint;
                deadEnemiesIds = deadEnemies;
            }
            public int playerCheckpoint { get; set; }
            public List<int> deadEnemiesIds { get; set; }
        }
        private SaveObject saveFile;

        public SaveLoad()
        {
            saveFile = new SaveObject(0, new List<int>());
        }

        public void NewCheckpoint(int checkpointId)
        {
            if (checkpointId > saveFile.playerCheckpoint)
            {
                saveFile.playerCheckpoint = checkpointId;
                saveFile.deadEnemiesIds.Clear();
            }
        }

        public void enemyDied(int enemyId)
        {
            saveFile.deadEnemiesIds.Add(enemyId);
        }

        public void playerDied()
        {
            //return player to checkpoint
            //reset enemies (saveFile.deadEmeniesIds)
            saveFile.deadEnemiesIds.Clear();
        }

        public void saveJson()
        {
            string json = JsonUtility.ToJson(saveFile);
            if (!File.Exists("./saveFile.json"))
            {
                File.Create("./saveFile.json").Close();

            }
            using (StreamWriter file = new StreamWriter("./saveFile.json"))
            {
                File.WriteAllText("./saveFile.json", json);
                file.WriteLine("./saveFile.json");
                file.Close();
            }
        }

        public void loadJson()
        {
            using (StreamReader r = new StreamReader("./saveFile.json"))
            {
                string json = r.ReadToEnd();
                SaveObject save = JsonUtility.FromJson<SaveObject>(json);
                saveFile.playerCheckpoint = save.playerCheckpoint;
                saveFile.deadEnemiesIds.Clear();
            }
        }
    }
}
