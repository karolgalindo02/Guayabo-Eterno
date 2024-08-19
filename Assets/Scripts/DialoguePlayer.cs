using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] dialogueImages; // New field for images
    [SerializeField] private GameObject polaroidImage; // New field for polaroid image
    [SerializeField] private FollowPlayer followPlayerScript; // Referencia al script FollowPlayer
    [SerializeField] private AudioClip[] dialogueAudioClips; // New field for audio clips

    public GameObject parts;
    private float typingTime = 0.05f;

    private bool isPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;
    private bool isFollowingPlayer; // New flag to check if following player
    private AudioSource audioSource; // AudioSource component

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Return) && !isFollowingPlayer)
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
        polaroidImage.SetActive(false); // Deactivate polaroid image
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
            polaroidImage.SetActive(true); // Activate polaroid image
            Time.timeScale = 1f;
            DeactivateAllImages(); // Deactivate all images when dialogue ends
            
            followPlayerScript.StartFollowing();
            isFollowingPlayer = true; // Set the flag to true
            dialogueMark.SetActive(false); // Deactivate dialogue mark

            parts.SetActive(true);
        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        ActivateImage(lineIndex); // Activate the corresponding image

        if (lineIndex < dialogueAudioClips.Length)
        {
            audioSource.clip = dialogueAudioClips[lineIndex];
            audioSource.Play();
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
        DeactivateAllImages(); // Deactivate all images first
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerPrefab && !isFollowingPlayer)
        {
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
        }
    }

    private void OntriggerExit(Collider other)
    {
        if (other.gameObject == playerPrefab)
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);
        }
    }
}