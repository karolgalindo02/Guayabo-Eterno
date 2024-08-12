using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueParts : MonoBehaviour
{
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] dialogueImages; // Nuevo campo para las imágenes
    [SerializeField] private GameObject polaroidImage; // Nuevo campo para la imagen de la polaroid

    private float typingTime = 0.05f;

    private bool isPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Return))
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

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueMark.SetActive(false);
        polaroidImage.SetActive(false); // Desactivar la imagen de la polaroid
        lineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(ShowLine());
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
            polaroidImage.SetActive(true); // Activar la imagen de la polaroid
            Time.timeScale = 1f;
            DeactivateAllImages(); // Desactivar todas las imágenes cuando el diálogo termine
        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        ActivateImage(lineIndex); // Activar la imagen correspondiente

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    private void ActivateImage(int index)
    {
        DeactivateAllImages(); // Desactivar todas las imágenes primero
        if (index < dialogueImages.Length)
        {
            dialogueImages[index].SetActive(true);
        }
    }

    private void DeactivateAllImages()
    {
        foreach (GameObject image in dialogueImages)
        {
            image.SetActive(false);
        }
    }

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
}