using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthcounter;
    public TMP_Text gameover;
    public TMP_Text winmessage;
    public float health = 100f;
    public MazeRenderer mazemanager;

    private int targetsEliminated;

    public void Start()
    {
        gameover.gameObject.SetActive(false);
        winmessage.gameObject.SetActive(false);
        healthcounter.text = health.ToString();
        targetsEliminated = 0;
    }

    public void Hurt(float damage)
    {
        health -= damage;
        if (health >= 0)
        {
            healthcounter.text = health.ToString();
        }
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        gameover.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TargetEliminated()
    {
        targetsEliminated++;
        if (targetsEliminated == mazemanager.enemyNum)
        {
            StartCoroutine(Win());
        }
    }

    IEnumerator Win()
    {
        winmessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
    }
}
