using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBackButton : MonoBehaviour
{
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FindObjectOfType<GameManager>().BackToMainMenu();
            }

        }
    }
}

