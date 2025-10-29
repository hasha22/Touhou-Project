using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace KH
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance { get; private set; }

        [Header("UI References")]
        public Image playerPortrait;
        public Image bossPortrait;
        public TMP_Text speakerName;
        public TMP_Text dialogueText;

        [Header("Typing Settings")]
        public float typingSpeed = 0.03f;

        [Header("References")]
        private int currentLine = 0;
        private DialogueSequence currentSequence;

        [Header("Flags")]
        private bool isTyping = false;
        private bool lineComplete = false;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z) && lineComplete)
            {
                currentLine++;
                if (currentLine < currentSequence.lines.Count)
                    ShowLine();
                else
                    EndDialogue();
            }
        }
        public void StartDialogue(DialogueSequence sequence)
        {
            WaveManager.instance.isPaused = true;
            StageManager.instance.isPaused = true;

            currentSequence = sequence;
            currentLine = 0;
            ShowLine();
        }
        private void ShowLine()
        {
            StopAllCoroutines();
            DialogueLine line = currentSequence.lines[currentLine];

            playerPortrait.sprite = line.isPlayer ? line.portraitSprite : null;
            bossPortrait.sprite = line.isPlayer ? null : line.portraitSprite;

            speakerName.text = line.speakerName;
            dialogueText.text = "";

            StartCoroutine(TypeText(line.text));
        }
        private IEnumerator TypeText(string text)
        {
            isTyping = true;
            lineComplete = false;

            foreach (char c in text)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            isTyping = false;
            lineComplete = true;
        }
        private void EndDialogue()
        {
            PlayerShooter playerShooter = PlayerInputManager.instance.playerObject.GetComponent<PlayerShooter>();

            playerShooter.isPaused = false;
            WaveManager.instance.isPaused = false;
            StageManager.instance.isPaused = false;

            Debug.Log("Dialogue Ended!");
        }

    }
}

