using Assets.Calin.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<int> deadEnemies;
    public List<GameObject> enemies;

    private int ID;

    void Start()
    {
        ID = 1;
        SetEnemyID(); 
    }

    void SetEnemyID()
    {
        enemies =new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        for(int i = 0;i<enemies.Count;i++)
        {
            enemies[i].GetComponent<EnemyManager>().SetID(ID);
            ID++;
        }
    }

    public void AddDeadEnemy(int enemyID)
    {
        deadEnemies.Add(enemyID);
        GameObject.Find("Main Camera").GetComponent<SaveLoad>().enemyDied(enemyID);
    }

    
}
