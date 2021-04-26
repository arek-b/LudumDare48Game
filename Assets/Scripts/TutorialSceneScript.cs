using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialSceneScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            text.text = "LOADING...";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            enabled = false;
        }
    }
}
