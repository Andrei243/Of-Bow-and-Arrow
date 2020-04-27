using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool isMelee;
    public bool isRanged;
    
    public void SetID(int ID)
    {
        if(isMelee)
            gameObject.GetComponent<MeleeEnemyScript>().ID = ID;
        if(isRanged)
            gameObject.GetComponent<RangedEnemyScript>().ID = ID;

    }

    public bool getIsDead()
    {
         if(isMelee)
            return gameObject.GetComponent<MeleeEnemyScript>().isDead;
         if(isRanged)
            return gameObject.GetComponent<RangedEnemyScript>().isDead;

        return false;
    }
}
