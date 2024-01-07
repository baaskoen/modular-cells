
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttachesText : MonoBehaviour
{
    public string text = "Hello world";

    public Vector2 offset = Vector2.zero;

    public int fontSize = 20;

    void Start()
    {
        GameObject go = Instantiate(Resources.Load("UIText")) as GameObject;
        FollowsTransform follow = go.GetComponent<FollowsTransform>();
        follow.target = gameObject.transform;
        follow.offset = offset;
        TextMeshProUGUI textComponent = go.GetComponent<TextMeshProUGUI>();
        textComponent.SetText(text);
        textComponent.fontSize = fontSize;
        go.transform.SetParent(Game.getGameCanvas().transform);
    }
}
