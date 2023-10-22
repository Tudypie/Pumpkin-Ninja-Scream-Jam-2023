using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    public string stateName;
    [SerializeField] protected Transform target;
    protected UnityEngine.AI.NavMeshAgent nma;
    private void Awake()
    {
        nma = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Start()
    {
        if(!target) target = GameObject.FindGameObjectWithTag("DefensePointTarget").transform;
    }

    public abstract void EnterState();
    public abstract void StateTick();
    public abstract void ExitState();

}
