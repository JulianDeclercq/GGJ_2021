using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Package p = other.GetComponent<Package>();
        if (p == null)
            return;

        if (p.Winner)
        {
            int target = SceneManager.GetActiveScene().buildIndex + 1;
            if (target < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(target, LoadSceneMode.Single);
            }
            else
            {
                GameManager.Instance.WinScreen();
            }
        }
    }
}
