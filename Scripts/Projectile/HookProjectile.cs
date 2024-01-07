using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookProjectile : Projectile
{
    public override void Update()
    {
        base.Update();

        Vector3 newRotation = transform.localEulerAngles;

        newRotation.z += Time.deltaTime * 1000;

        transform.localEulerAngles = newRotation;
    }

    protected override void onDestinationReached()
    {
        if (targetGameobject && causer)
        {
            Rigidbody2D rigidbody = targetGameobject.GetComponent<Rigidbody2D>();
            Vector2 pullDirection = (causer.transform.position - targetGameobject.transform.position).normalized;
            rigidbody.AddForce(pullDirection * (rigidbody.mass * 8), ForceMode2D.Impulse);
        }


        GameObject.Destroy(gameObject);
    }
}
