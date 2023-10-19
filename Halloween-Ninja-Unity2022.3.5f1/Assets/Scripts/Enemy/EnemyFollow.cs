using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : EnemyBehaviour
{
    [SerializeField] float followSpeed = 5;

    public override void EnterState()
    {
        nma.speed = 5;
    }

    // Update is called once per frame
    public override void StateTick()
    {
        nma.SetDestination(target.position);
    }

    public override void ExitState()
    {

    }


}
