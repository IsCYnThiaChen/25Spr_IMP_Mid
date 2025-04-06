using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyLV1 : EnemyBase
{

    protected override void TimerContent()
    {
        //Recover health
        nav.SetDestination(target.position);
    }

    public override void Damaged(int damage)
    {
        damage = 10;
        gameManager.score = Mathf.Max(0, gameManager.score - damage);
    }
}
