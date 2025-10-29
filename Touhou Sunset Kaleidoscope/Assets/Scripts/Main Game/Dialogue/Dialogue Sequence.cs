using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class DialogueSequence : MonoBehaviour
    {
        public List<DialogueLine> lines;
    }
}
[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)] public string text;
    public Sprite portraitSprite;
    public bool isPlayer = true;
}
