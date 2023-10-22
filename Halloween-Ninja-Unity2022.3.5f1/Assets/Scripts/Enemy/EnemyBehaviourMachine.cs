using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourMachine : MonoBehaviour
{

    Dictionary<string, EnemyBehaviour> behaviourRef = new Dictionary<string, EnemyBehaviour>();
    [SerializeField] string defaultStateName;

    string currentStateName; // This is for debugging only
    EnemyBehaviour currentState;
    Health health;


    void Awake()
    {
        health = GetComponent<Health>();


        EnemyBehaviour[] allBehaviours = GetComponents<EnemyBehaviour>();
        for (int i = 0; i < allBehaviours.Length; i++)
        {
            behaviourRef.Add(allBehaviours[i].stateName, allBehaviours[i]);
        }
        currentState = behaviourRef[defaultStateName];

    }

    private void OnEnable()
    {
        health.OnDeath += EnemyDeath;
    }

    private void OnDisable()
    {
        health.OnDeath -= EnemyDeath;
    }

    private void Start()
    {
        currentState.EnterState();
    }

    private void Update()
    {
        currentState.StateTick();
    }

    private void EnemyDeath(object sender, EventArgs e)
    {
        WaveSystem.Instance.KillEnemy();
        ComboSystem.Instance.AddCombo();
        FMODAudio.Instance.PlayAudio(FMODAudio.Instance.pumpkinExplosion);
    }

    public void ChangeState(string newStateName)
    {
        currentState.ExitState();
        currentState = behaviourRef[newStateName];
        currentStateName = newStateName; // This is for debugging only
        currentState.EnterState();
    }

}
