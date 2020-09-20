using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public static int myHealth = 15;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    Animator animator;

    private void Update()
    {
        if (myHealth <= 0)
        {
            myHealth = 15;
            SceneManager.LoadScene(1);
            //StartCoroutine(DeathAnimation());
        }

        CheckingHealths();

        DecreaseHealth();
        IncreaseHealth();


      
    }
    private IEnumerator DeathAnimation()
    {
        animator.SetBool("IsDead", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("IsDead", false);
      

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
