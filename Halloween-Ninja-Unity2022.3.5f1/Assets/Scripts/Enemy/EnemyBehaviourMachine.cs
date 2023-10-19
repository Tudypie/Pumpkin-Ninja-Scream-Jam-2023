using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourMachine : MonoBehaviour
{

    Dictionary<string, EnemyBehaviour> behaviourRef = new Dictionary<string, EnemyBehaviour>();
    [SerializeField] string defaultStateName;

    string currentStateName; // This is for debugging only
    EnemyBehaviour currentState;


    void Awake()
    {
        EnemyBehaviour[] allBehaviours = GetComponents<EnemyBehaviour>();
        for (int i = 0; i < allBehaviours.Length; i++)
        {
            behaviourRef.Add(allBehaviours[i].stateName, allBehaviours[i]);
        }
        currentState = behaviourRef[defaultStateName];
        currentState.EnterState();
    }

    private void Update()
    {
        currentState.StateTick();
    }

    public void ChangeState(string newStateName)
    {
        currentState.ExitState();
        currentState = behaviourRef[newStateName];
        currentStateName = newStateName; // This is for debugging only
        currentState.EnterState();
    }

}
