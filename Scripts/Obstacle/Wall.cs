using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rigidbody == null)
        {
            return;
        }

        ContactPoint2D[] contacts = collision.contacts;

        if (contacts.Length > 0)
        {
            // Calculate an average collision point based on contact points
            Vector2 averageCollisionPoint = Vector2.zero;

            foreach (ContactPoint2D contact in contacts)
            {
                averageCollisionPoint += contact.point;
            }

            averageCollisionPoint /= contacts.Length;

            rigidbody.AddForce(
                (rigidbody.transform.position - new Vector3(averageCollisionPoint.x, averageCollisionPoint.y, 0)).normalized * 1.5f,
                 ForceMode2D.Impulse
            );
        }


    }
}
