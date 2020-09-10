using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static int myHealth = 15;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Update()
    {
        CheckingHealths();

        DecreaseHealth();
        IncreaseHealth();
      
    }
    public void TookDamageFromEnemy(int damage)
    {
        // play a hurt sound 
        
        myHealth -= damage;
        Debug.Log("I HAVE GOT DAMAGE !" + damage);
        //dazedTime = startDazedTime;
    }
    private void DecreaseHealth()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            myHealth--;
            Debug.Log(myHealth);
        }
    }

    private void IncreaseHealth()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            myHealth++;
            Debug.Log(myHealth);
        }
    }

    private void CheckingHealths()
    {
        if (myHealth > numOfHearts)
        {
            myHealth = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < myHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
