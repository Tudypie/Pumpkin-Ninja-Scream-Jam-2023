using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerRelay : MonoBehaviour
{
    public class CollisionEvent : EventArgs 
    {
        public Collider other;
        public CollisionEvent(Collider other)
        {
            this.other = other;
        }
    }

    public EventHandler<CollisionEvent> OnTriggerEnterEvent;
    public EventHandler<CollisionEvent> OnTriggerExitEvent;
    public EventHandler<CollisionEvent> OnTriggerStayEvent;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(this, new CollisionEvent(other));
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(this, new CollisionEvent(other));
    }
    
    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayEvent?.Invoke(this, new CollisionEvent(other));
    }


}
