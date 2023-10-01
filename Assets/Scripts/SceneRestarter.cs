using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
    [SerializeField]
    GameObject celebration;

    [SerializeField]
    GameObject lamentation;

    public void Restart(bool won)
    {
        StartCoroutine(Delay());

        if (won)
        {
            celebration.SetActive(true);
        }
        else
        {
            lamentation.SetActive(true);
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
