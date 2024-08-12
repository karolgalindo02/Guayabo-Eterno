using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Parts : MonoBehaviour
{

    [SerializeField] private GameObject dialogueMark;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    [SerializeField] private GameObject playerPrefab;
    private bool isPlayerInRange;

    private float typingTime = 0.05f;
    private bool didDialogueStart;
    private int lineIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == dialogueLines[lineIndex])
            {
                NextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
            }
        }        
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            dialogueMark.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueMark.SetActive(false);
        lineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(ShowLine());
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject == playerPrefab)
    //     {
    //         Debug.Log("hay colision");
    //         isPlayerInRange = true;
    //         dialogueMark.SetActive(true);
    //     }
    // }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerPrefab)
        {
            
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == playerPrefab)
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);
        }
    }

    // private void OnCollisionExit(Collision collision)
    // {
    //     if (collision.gameObject == playerPrefab)
    //     {
    //         isPlayerInRange = false;
    //         dialogueMark.SetActive(false);
    //     }
    // }

}
