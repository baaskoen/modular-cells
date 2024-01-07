using System.Collections;

using UnityEngine;

public class FollowsTransform : MonoBehaviour
{
    public Transform target;

    public Vector2 offset = Vector2.zero;

    private RectTransform own;

    // Start is called before the first frame update
    void Start()
    {
        this.own = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
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
