using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntouchableSkill : MonoBehaviour
{
    public Rigidbody2D _myRb;

    public float skillTime;
    private float startSkillTime;

    public float regenerationTime;
    private float startRegenerationTime;

    private bool isSkillActive = false;
    private bool isRegenerationActive = false;

    private int _secondsPassed;
    private bool _isCorRunning;
    private bool _isReadyToUse;

    public float knockback;

    public Rigidbody2D _enemyRb;
    private bool _isKnockActive;
    public Animator animator;

    private void Awake()
    {
        _isCorRunning = false;
        _isReadyToUse = true;
        _myRb = GetComponent<Rigidbody2D>();

      
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.L) && _isReadyToUse)
        {
            Skill();
        }
            
        
    }

    private void Skill()
    {
        
            if (!_isCorRunning && _isReadyToUse)
            {
                _myRb.gameObject.layer = LayerMask.NameToLayer("Untouchable");
                StartCoroutine(RegenHP());
            }
    
    }

    private IEnumerator RegenHP()
    {
        animator.SetBool("IsRegenSpellActive", true);
        _myRb.gameObject.GetComponent<Movement>().enabled = false;
        _isCorRunning = true;
        yield return new WaitForSeconds(1f);
        Health.myHealth++;
        _secondsPassed++;
        yield return new WaitForSeconds(1f);
        Health.myHealth++;
        _secondsPassed++;
        yield return new WaitForSeconds(1f);
        Health.myHealth++;
        _secondsPassed++;
        yield return new WaitForSeconds(1f);
        Health.myHealth++;
        _secondsPassed++;
        yield return new WaitForSeconds(1f);
        Health.myHealth++;
        _secondsPassed++;
        yield return new WaitForSeconds(1f);
        _isCorRunning = false;
        animator.SetBool("IsRegenSpellActive", false);

        _myRb.gameObject.GetComponent<Movement>().enabled = true;
        //if (!_isKnockActive)
        //{
        //    KnockBack();
        //}
       

        _myRb.gameObject.layer = LayerMask.NameToLayer("Player");
        _isReadyToUse = false;
      
        
        yield return new WaitForSeconds(5f);
        _isReadyToUse = true;
 
    }
  
    private void KnockBack()
    {
       
        Vector2 direction = _enemyRb.transform.position - transform.position;


        if (Vector2.Distance(transform.position, _enemyRb.transform.position) < 3f)
        {
            Debug.Log("Knockback");
            _enemyRb.AddForce(direction * knockback, ForceMode2D.Impulse);
        }
        
      
    }

}
