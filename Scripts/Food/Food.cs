using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int nutrition = 30;

    public float destroyTimer = 60;

    void Update()
    {
        this.destroyTimer -= Time.deltaTime;

        if (this.destroyTimer <= 0 || this.nutrition <= 0)
        {
            Destroy(gameObject);
        }
    }
}
