using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject overlay;
    public GameObject p1WinText;
    public GameObject p2WinText;
    private void Update()
    {
        if(player1 == null)
        {
            overlay.SetActive(true);
            if (!p1WinText.activeSelf)
            {
                p2WinText.SetActive(true);
            }
        }
        
        if(player2 == null)
        {
            overlay.SetActive(true);
            if (!p2WinText.activeSelf)
            {
                p1WinText.SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
