using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueCollider : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueBox;
 
    private bool _conversationOpened;
    

    private void Awake()
    {
        _conversationOpened = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement>() && !_conversationOpened)
        {
            
            _dialogueBox.gameObject.SetActive(true);
            _conversationOpened = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _dialogueBox.gameObject.SetActive(false);
        _conversationOpened = false;
    }
}
