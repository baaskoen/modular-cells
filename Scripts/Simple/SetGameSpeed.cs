using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameSpeed : MonoBehaviour
{
    public void setGameSpeed(float scale)
    {
        Time.timeScale = scale;
    }
}
