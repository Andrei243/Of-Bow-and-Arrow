using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Calin.Scripts
{


    [Serializable]
    internal class SaveObject
    {
        public SaveObject()
        {
            playerCheckpoint = 0;
            deadEnemiesIds = new List<int>();
        }
        public int playerCheckpoint;
        public List<int> deadEnemiesIds;
    }
    class SaveLoad : MonoBehaviour
    {
        
        private SaveObject saveFile;

        public void Awake()
        {
            saveFile = new SaveObject();
            
        }

        public void NewCheckpoint(int checkpointId)
        {
            if (checkpointId > saveFile.playerCheckpoint)
            {
                saveFile.playerCheckpoint = checkpointId;
                saveJson();
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

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//just need to reset the scene
        }

        public void saveJson()
        {
            string json = JsonUtility.ToJson(saveFile);
            if (!File.Exists("./saveFile.json"))
            {
                var file = File.Create("./saveFile.json");
                file.Close();
            }
            File.WriteAllText("./saveFile.json", json);
        }

        public void SetStartSave(SaveObject obj)
        {
            saveFile = obj;
        }


    }
}
