using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public float blinkTime = 2;
    private float blinkTimer;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        this.sprite = GetComponent<SpriteRenderer>();
        this.blinkTimer = this.blinkTime;
    }

    // Update is called once per frame
    void Update()
    {
        this.blinkTimer -= Time.deltaTime;

        if (this.blinkTimer <= 0)
        {
            this.sprite.enabled = !this.sprite.enabled;

            this.blinkTimer = this.blinkTime;
        }
    }
}
