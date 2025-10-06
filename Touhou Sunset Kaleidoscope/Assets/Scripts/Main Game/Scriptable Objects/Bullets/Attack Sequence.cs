using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Attack/Attack Sequence")]
    public class AttackSequence : ScriptableObject
    {
        public List<PatternStep> patternSteps;
        public bool loopPattern = true;
    }

    // System.Serializable = struct
    [System.Serializable]
    public class PatternStep
    {
        public EnemyShotPattern pattern;
        public float delayBeforeNextPattern = 1f;
    }
}

