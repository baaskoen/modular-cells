using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool explodes = false;

    public float explosionRadius = 2;

    public float speed = 2;

    public float destinationDistance = 0.1f;

    protected GameObject targetGameobject;

    protected Vector2 targetPosition;

    protected bool active = true;

    protected Entity causer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        GameObject.Destroy(gameObject, 10);

        if (this.targetGameobject)
        {
            this.targetPosition = this.targetGameobject.transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (this.explodes)
        {
            Gizmos.DrawWireSphere(transform.position, this.explosionRadius);
        }

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!active)
        {
            return;
        }

        if (this.targetGameobject)
        {
            this.targetPosition = this.targetGameobject.transform.position;
        }

        this.transform.position = Vector2.MoveTowards(transform.position, this.targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(this.transform.position, targetPosition) <= destinationDistance)
        {
            this.onDestinationReached();

            this.active = false;
        }
    }

    protected virtual void onDestinationReached()
    {

    }

    public void setCauser(Entity entity)
    {
        this.causer = entity;
    }

    public void setTarget(GameObject gameObject)
    {
        this.targetGameobject = gameObject;
    }

    public void setTarget(Vector2 position)
    {
        this.targetPosition = position;
    }
}
