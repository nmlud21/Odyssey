using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerUI : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject textBox;
    public Text gameText;

    private int tutorialUIindex = 1;
    
    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            textBox.SetActive(true);

            if (tutorialUIindex == 1)
            {
                gameText.text = "Use WASD to move around";
            }
            else if (tutorialUIindex == 2)
            {
                gameText.text = "Shoot the aliens using right click. Killing one gives you extra reserves. Press R to reload.";
            }
            else if (tutorialUIindex == 3)
            {
                gameText.text = "Hold down LSHIFT to sprint.";
            }
            else if (tutorialUIindex == 4)
            {
                gameText.text = "Use the scroll wheel or the 1 and 2 keys to swap weapons.";
            }
            else if (tutorialUIindex == 5)
            {
                gameText.text = "Press TAB to open the settings menu.";
            }
            
            //todo more tutorial tips

            StartCoroutine(displayUI());

        }
    }

    IEnumerator displayUI()
    {
        yield return new WaitForSeconds(3.5f);
        if (tutorialUIindex == 1)
        {
            gameManager.transform.position = new Vector3(157, 2, 806);
        }
        else if (tutorialUIindex == 2)
        {
            gameManager.transform.position = new Vector3(164, 2, 748);
            gameManager.transform.rotation = Quaternion.Euler(0, -27, 0);
        }
        else if (tutorialUIindex == 3)
        {
            gameManager.transform.position = new Vector3(196, 2, 669);
        }
        else if (tutorialUIindex == 4)
        {
            gameManager.transform.position = new Vector3(225, 2, 600);
        }
        
        textBox.SetActive(false);
        tutorialUIindex++;
    }
}
