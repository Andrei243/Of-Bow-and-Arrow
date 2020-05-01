using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool isMelee;
    public bool isRanged;

    public void SetID(int ID)
    {
        if (isMelee)
            gameObject.GetComponent<MeleeEnemyScript>().ID = ID;
        if (isRanged)
            gameObject.GetComponent<RangedEnemyScript>().ID = ID;

    }
    public int GetID()
    {
        if (isMelee)
            return gameObject.GetComponent<MeleeEnemyScript>().ID;
        else
            return gameObject.GetComponent<RangedEnemyScript>().ID;
    }

    public bool GetIsDead()
    {
        if (isMelee)
            return gameObject.GetComponent<MeleeEnemyScript>().isDead;
        if (isRanged)
            return gameObject.GetComponent<RangedEnemyScript>().isDead;

        return false;
    }

    public void SetIsDead()
    {
        
        if (isMelee)
            gameObject.GetComponent<MeleeEnemyScript>().isDead = false;
        if (isRanged)
            gameObject.GetComponent<RangedEnemyScript>().isDead = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
