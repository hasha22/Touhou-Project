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
        public GameObject dialogueBox;
        public Image playerPortrait;
        public Image bossPortrait;
        public TMP_Text speakerName;
        public TMP_Text dialogueText;

        [Header("Typing Settings")]
        public float typingSpeed = 0.03f;
        [SerializeField] private float autoAdvanceDelay = 2f;
        private Coroutine autoAdvanceRoutine;

        [Header("Highlight Settings")]
        public float fadeSpeed = 6f;
        public Vector2 playerOffset = new Vector2(-60f, 0f);
        public Vector2 bossOffset = new Vector2(60f, 0f);
        public Color dimColor = new Color(0.4f, 0.4f, 0.4f, 1f);
        public Color normalColor = Color.white;
        private Vector3 playerBasePos;
        private Vector3 bossBasePos;

        [Header("References")]
        private int currentLine = 0;
        [SerializeField] private DialogueSequence currentSequence;
        private Coroutine highlightRoutine;
        private PlayerManager playerManager;

        [Header("Flags")]
        private bool isActive = false;
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
            playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
        }
        private void Start()
        {
            playerBasePos = playerPortrait.rectTransform.anchoredPosition;
            bossBasePos = bossPortrait.rectTransform.anchoredPosition;
        }
        private void Update()
        {
            if (!isActive || !dialogueBox.activeInHierarchy) return;

            if (Input.GetKeyDown(KeyCode.Z) && lineComplete)
            {
                currentLine++;
                if (currentLine < currentSequence.lines.Count && isActive)
                    ShowLine();
                else
                    EndDialogue();
            }
        }
        public void StartDialogue(DialogueSequence sequence)
        {
            if (isActive) return;

            dialogueBox.SetActive(true);
            isActive = true;
            currentSequence = sequence;
            currentLine = 0;

            PlayerShooter playerShooter = PlayerInputManager.instance.playerObject.GetComponent<PlayerShooter>();
            playerManager.playerCollider.enabled = false;
            WaveManager.instance.isPaused = true;
            StageManager.instance.isPaused = true;
            FaithManager.instance.isPaused = true;
            playerShooter.isPaused = true;

            ShowLine();
        }
        private void ShowLine()
        {
            if (currentSequence == null) return;

            StopAllCoroutines();
            DialogueLine line = currentSequence.lines[currentLine];

            playerPortrait.sprite = line.playerPortraitSprite;
            bossPortrait.sprite = line.bossPortraitSprite;

            //speakerName.text = line.speakerName;
            dialogueText.text = "";

            if (highlightRoutine != null)
                StopCoroutine(highlightRoutine);

            highlightRoutine = StartCoroutine(HighlightSpeaker(line.speakerType));

            StartCoroutine(TypeText(line.text));
        }
        private IEnumerator HighlightSpeaker(DialogueSpeaker speaker)
        {
            // Capture targets
            bool playerSpeaking = speaker == DialogueSpeaker.Player ? true : false;

            Image active = playerSpeaking ? playerPortrait : bossPortrait;
            Image inactive = playerSpeaking ? bossPortrait : playerPortrait;

            float t = 0f;
            Color startActiveColor = active.color;
            Color startInactiveColor = inactive.color;

            Vector3 playerStartPos = playerPortrait.rectTransform.anchoredPosition;
            Vector3 bossStartPos = bossPortrait.rectTransform.anchoredPosition;

            Vector3 playerTargetPos = playerBasePos + (playerSpeaking ? Vector3.zero : (Vector3)playerOffset);
            Vector3 bossTargetPos = bossBasePos + (playerSpeaking ? (Vector3)bossOffset : Vector3.zero);

            while (t < 1f)
            {
                t += Time.deltaTime * fadeSpeed;

                active.color = Color.Lerp(startActiveColor, normalColor, t);
                inactive.color = Color.Lerp(startInactiveColor, dimColor, t);

                playerPortrait.rectTransform.anchoredPosition = Vector3.Lerp(playerStartPos, playerTargetPos, t);
                bossPortrait.rectTransform.anchoredPosition = Vector3.Lerp(bossStartPos, bossTargetPos, t);


                yield return null;
            }
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

            if (autoAdvanceRoutine != null)
                StopCoroutine(autoAdvanceRoutine);

            autoAdvanceRoutine = StartCoroutine(AutoAdvanceAfterDelay());
        }
        private IEnumerator AutoAdvanceAfterDelay()
        {
            yield return new WaitForSeconds(autoAdvanceDelay);

            if (!isTyping && lineComplete && isActive)
            {
                currentLine++;
                if (currentLine < currentSequence.lines.Count)
                    ShowLine();
                else
                    EndDialogue();
            }
        }
        private void EndDialogue()
        {
            PlayerShooter playerShooter = PlayerInputManager.instance.playerObject.GetComponent<PlayerShooter>();

            UIManager.instance.InitializeBossUI(EnemyDatabase.instance.currentActiveBoss.bossData);
            dialogueBox.SetActive(false);
            playerShooter.isPaused = false;
            playerManager.playerCollider.enabled = true;
            WaveManager.instance.isPaused = false;
            StageManager.instance.isPaused = false;
            FaithManager.instance.isPaused = false;

            BossManager boss = EnemyDatabase.instance.currentActiveBoss;
            boss.isPaused = false;
            boss.isWaitingForDialogue = false;
            boss.StartNextPhase();

            isActive = false;

        }
    }
}

