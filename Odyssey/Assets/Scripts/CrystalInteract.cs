using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystalInteract : MonoBehaviour
{
    public GameObject interactText;
    public GameObject player;
    public GameObject endScreen;
    public GameObject endGameText;
    public GameObject menuButton;

    public Animator endScreenAnim;

    public static bool gameOver = false;

    private void OnMouseOver()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (!gameOver)
        {
            if (distance <= 50)
            {
                interactText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("INTERACT");
                    StartCoroutine(EndGame());
                }
            }
            else
            {
                interactText.SetActive(false);
            }
        }
    }

    private void OnMouseExit()
    {
        interactText.SetActive(false);
    }

    private IEnumerator EndGame()
    {
        gameOver = true;
        endScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        
        yield return new WaitForSeconds(2f);
        
        endGameText.SetActive(true);
        endScreenAnim.SetInteger("Index", 1);
        
        yield return new WaitForSeconds(2f);
        
        menuButton.SetActive(true);
        endScreenAnim.SetInteger("Index", 2);

        yield return new WaitForSeconds(1f);
        
        Time.timeScale = 0f;

    }

    public void MainMenu()
    {
        gameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
}
