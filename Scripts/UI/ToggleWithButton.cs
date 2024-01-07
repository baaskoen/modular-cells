using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWithButton : MonoBehaviour
{
    public void toggle(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
