using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueHandler : MonoBehaviour {
    [SerializeField] private GameObject[] DialoguePoints;//array of objects representing dialogue points
    [SerializeField] private TextAsset[] textFiles;//array of text files containing dialoge
    [SerializeField] private TMP_Text textDisplay;//text display UI element
    [SerializeField] private float displayInterval = 2f;//interval for displaying each line of dialogue
    [SerializeField] private GameObject DialogueBox;//UI element for displaying dialogue
    private bool isCoroutineRunning = false;//flag to track if a coroutine is currently running
    private int currentDialogueIndex = -1; // Initialize with an invalid index
    private bool[] dialogueDisplayed;//array to track which dialogues have been displayed

    private void Start() {
        dialogueDisplayed = new bool[DialoguePoints.Length];//initlaize array to track dialogue display status
    }

    void Update() {
        CheckItemContact();//check if player is in contact with a dialogue point
        if (currentDialogueIndex != -1 && !isCoroutineRunning) {
            StartCoroutine(DisplayTextRoutine());//start displaying text when a new dialogue is triggered
        }
    }

    void CheckItemContact() {
        foreach (GameObject dialoguePoint in DialoguePoints) {//iterate through each dialogue point
            if (dialoguePoint != null) {
                float contactDistance = 2.0f; // Adjust the contact distance as needed
                bool inContact = Physics.CheckSphere(dialoguePoint.transform.position, contactDistance, LayerMask.GetMask("Player"));//check if player is within contact distance

                if (inContact) {//player in contact with the dialogue point
                    int dialogueIndex = System.Array.IndexOf(DialoguePoints, dialoguePoint);//get index of the current dialogue point
                    if (currentDialogueIndex != dialogueIndex && !dialogueDisplayed[dialogueIndex]) {
                        currentDialogueIndex = dialogueIndex;//set current dialogue index 
                        LoadTextFile();//load text file for the current dialogue
                        dialogueDisplayed[dialogueIndex] = true; // Mark dialogue as displayed
                        StartTextDisplay();//start displaying text
                    }
                    return; // Exit the loop after finding the first contact
                }
            }
        }
        currentDialogueIndex = -1;//Player not in contact with any dialogue point
    }

    void StartTextDisplay() {
        DialogueBox.SetActive(true);//activate dialogue box UI element
        if (!isCoroutineRunning) {
            StartCoroutine(DisplayTextRoutine());//start displaying text
        }
    }

    IEnumerator DisplayTextRoutine() {
        isCoroutineRunning = true;//set coroutine flag to true

        if (textFiles.Length > 0 && currentDialogueIndex >= 0 && currentDialogueIndex < textFiles.Length) {
            TextAsset selectedFile = textFiles[currentDialogueIndex];//get the text file for the current dialogue
            string[] lines = selectedFile.text.Split('\n');//split text into lines

            foreach (string line in lines) {
                // Check for bold and italic formatting
                string formattedLine = FormatText(line);//format text with bold and italics

                textDisplay.text = formattedLine;//set text to be displayed
                yield return new WaitForSeconds(displayInterval);//wait for specified interval before displaying next line
                textDisplay.text = ""; // Clear the text before moving to the next line
            }

            currentDialogueIndex = -1; // Reset dialogue index after displaying
            DialogueBox.SetActive(false);//deactivate dialogue box UI element
        }

        isCoroutineRunning = false;//reset coroutine flag
    }


    void LoadTextFile() {
        if (currentDialogueIndex >= 0 && currentDialogueIndex < textFiles.Length) {
            TextAsset selectedFile = textFiles[currentDialogueIndex];//get the text file for the current dialogue
            string[] lines = selectedFile.text.Split('\n');//split text into lines
        }
    }

    string FormatText(string inputText) {//function to format text with bald and italic tags
        inputText = inputText.Replace("||", "<b>").Replace("*", "<i>");   // Replace * with <i> for italic and " with <b> for bold
        return inputText;
    }
}