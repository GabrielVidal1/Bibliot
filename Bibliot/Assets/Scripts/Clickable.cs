using UnityEngine;

public abstract class Clickable : MonoBehaviour
{
    public virtual void Click()
    {
        
    }
}

[RequireComponent(typeof(Collider))]
public abstract class Interactible : BookStorage
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerTransport>().InteractWith(this);
        }
    }
}