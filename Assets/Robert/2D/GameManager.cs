using Assets.Calin.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<int> deadEnemies;
    public List<GameObject> enemies;

    private int ID;
    private int IDCheckpoint;

    void Start()
    {
        ID = 1;
        IDCheckpoint = 1;
        SetEnemyID();
        loadJson();
    }

    void SetEnemyID()
    {
        //Sets all the enemies IDs in the order they are in the scene hierarchy
        enemies =new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        for(int i = 0;i<enemies.Count;i++)
        {
            enemies[i].GetComponent<EnemyManager>().SetID(ID);
            ID++;
        }

        var checkpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint"));
        checkpoints.ForEach(e =>
        {
            e.GetComponent<CheckPointBehaviour>().ID = IDCheckpoint;
            IDCheckpoint++;
        });
    }

    public void AddDeadEnemy(int enemyID)
    {
        deadEnemies.Add(enemyID);
        GameObject.Find("Main Camera").GetComponent<SaveLoad>().enemyDied(enemyID);
    }

    public void loadJson()
    {
        if (!File.Exists("./saveFile.json"))
        {
            return;
        }
        SaveObject save;
        using (StreamReader r = new StreamReader("./saveFile.json"))
        {
            if (r == null) return;
            string json = r.ReadToEnd();
            if (json == ""){
                return;
            }
            save = JsonUtility.FromJson<SaveObject>(json);
            r.Close();
        }
        if(save == null)
        {
            return;
        }
        enemies.Where(e => save.deadEnemiesIds.Contains(e.GetComponent<EnemyManager>().GetID())).ToList().ForEach(e =>
        {
            e.GetComponent<EnemyManager>().SetIsDead();
        });
        var position = GameObject.FindGameObjectsWithTag("Checkpoint").FirstOrDefault(e => e.GetComponent<CheckPointBehaviour>().ID == save.playerCheckpoint)?.transform.position;
        if (position.HasValue)
        {
            GameObject.Find("Player").transform.position = position.Value + new Vector3(0, 0.01f, 0);
        }
        GameObject.Find("Main Camera").GetComponent<SaveLoad>().SetStartSave(save);
    }

}
