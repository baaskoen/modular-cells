using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public new SpriteRenderer renderer;

    public float radius;

    private Vector2 maxSize;

    void Awake()
    {
        this.renderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.maxSize = new Vector2(this.radius * 2, this.radius * 2);

        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newScale = transform.localScale;
        newScale.x += 15f * Time.deltaTime;
        newScale.y += 15f * Time.deltaTime;

        transform.localScale = Vector2.Min(newScale, maxSize);
        Color color = renderer.color;
        color.a -= 2 * Time.deltaTime;
        renderer.color = color;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, this.radius);
    }

}
