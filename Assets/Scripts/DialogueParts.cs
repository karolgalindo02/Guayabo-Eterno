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
    [SerializeField] private GameObject[] dialogueImages; 
    [SerializeField] private GameObject polaroidImage; 
    [SerializeField] private AudioClip[] dialogueAudioClips; 

    [SerializeField] private GameObject[] partsList;

    private float typingTime = 0.05f;

    private bool isPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;
    private AudioSource audioSource; 
    private bool barkPlayed; 

    private ControllerParts controllerParts;

    public GameObject MinipiezasGameObject; 

    private List<GameObject> gameObjectsList;

    void Start()
    {
        // Encuentra el script ControllerParts en la escena
        controllerParts = FindObjectOfType<ControllerParts>();

        ControllerParts MinipiezasScript = MinipiezasGameObject.GetComponent<ControllerParts>();

        // Acceder a la lista
        gameObjectsList = MinipiezasScript.gameObjects;

        audioSource = gameObject.AddComponent<AudioSource>();
        barkPlayed = false; // Inicializar el flag
    }

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
        polaroidImage.SetActive(false);
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
            polaroidImage.SetActive(true); 
            Time.timeScale = 1f;
            DeactivateAllImages(); 
            barkPlayed = false; 
            gameObject.SetActive(false);
            controllerParts.collectedPartsCount++;
            controllerParts.UpdateCollectedPartsText();
            
            gameObjectsList.Remove(gameObject);

        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        ActivateImage(lineIndex); 

        if (lineIndex < dialogueAudioClips.Length)
        {
            audioSource.clip = dialogueAudioClips[lineIndex];
            if (audioSource.clip.name == "Bark" && !barkPlayed)
            {
                audioSource.Play();
                barkPlayed = true;
            }
            else if (audioSource.clip.name != "Bark")
            {
                audioSource.Play();
            }
        }

        int charCount = 0;
        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            charCount++;
            if (charCount % 3 == 0 && audioSource.clip != null && audioSource.clip.name != "Bark")
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    private void ActivateImage(int index)
    {
        DeactivateAllImages();
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