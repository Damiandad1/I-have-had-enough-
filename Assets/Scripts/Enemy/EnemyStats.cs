using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public static int enemyHP = 3;
	[SerializeField] private GameObject hitEffect;

	public void TakeMeleeDamage(int damage)
	{
		// play a hurt sound 
		Instantiate(hitEffect, transform.position, Quaternion.identity);
		enemyHP -= damage;
		Debug.Log("damage TAKEN !" + damage);


		//dazedTime = startDazedTime;
	}


	private void Update()
	{
		if (enemyHP <= 0)
		{
			Destroy(gameObject);
		}
	}

}
