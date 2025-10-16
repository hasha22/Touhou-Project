using UnityEngine;
namespace KH
{
    public class BossPhase : ScriptableObject
    {
        [Header("Phase Information")]
        public int phaseBossHealth;
        public string phaseName;
        public float duration;
        public bool isSpellCard;
        public MovementSequence phaseMovementSequence;
        public AttackSequence phaseAttackSequence;
        public float delayBeforeNextPhase;

        public virtual void StartPhase(BossManager boss) { }
        public virtual void UpdatePhase(BossManager boss, float deltaTime) { }
        public virtual void EndPhase(BossManager boss) { }
    }
}


