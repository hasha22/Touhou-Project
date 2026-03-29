using KH;
using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Dialogue/Dialogue Sequence")]
    public class DialogueSequence : ScriptableObject
    {
        public List<DialogueLine> lines;
        [Tooltip("If it's supposed to trigger after wave 2, set it's value to 2. Currently unused")]
        public float triggerAfterWaveIndex; // currently unused
        public float delayBeforeDialogueBegins = 2f;
    }
}
[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public DialogueSpeaker speakerType;
    [TextArea(2, 5)] public string text;
    public Sprite playerPortraitSprite;
    public Sprite bossPortraitSprite;
}
