using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerForMenu : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManagerForMenu>().StartDialogue(dialogue);
    }
}
