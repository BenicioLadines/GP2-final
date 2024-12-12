using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject opponent;
    public GameObject overlay;
    private void Update()
    {
        if(opponent == null)
        {
            overlay.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
