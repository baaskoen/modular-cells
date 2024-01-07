using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Transform target;

    public Vector2 offset = Vector2.zero;

    public TextMeshProUGUI textComponent;

    private RectTransform own;

    private float maxYOffset, maxXOffset;

    private float speed = 25;

    public int fontSize = 24;

    // Start is called before the first frame update
    void Start()
    {
        this.own = GetComponent<RectTransform>();

        this.maxYOffset = Random.Range(3f, 6f);
        this.maxXOffset = Random.Range(-3f, 3f);

        this.textComponent.fontSize = fontSize;

        Destroy(gameObject, 2);

        this.updatePosition();
    }

    // Update is called once per frame
    void Update()
    {


        if (this.offset.y < this.maxYOffset)
        {
            this.offset.y += speed * Time.deltaTime;
        }

        if (this.offset.x < this.maxXOffset)
        {
            this.offset.x += speed * Time.deltaTime;
        }


        this.updatePosition();
    }

    void updatePosition()
    {
        if (!this.target)
        {
            return;
        }

        this.own.SetPositionAndRotation(
            new Vector3(
                target.transform.position.x + (offset.x / 10),
                target.transform.position.y + (offset.y / 10),
                0
            ),
            own.rotation
        );
    }
}
