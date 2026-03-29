using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Boss/Spell Card Phase")]
    public class BossSpellCardPhase : BossPhase
    {
        private Coroutine currentPatternRoutine;

        [Header("Spell Card Data")]
        public int spellCardBonus;
        public bool playerHasDied = false;
        public override void StartPhase(BossManager boss)
        {
            playerHasDied = false;
            // for loop to start all pattern coroutines
            foreach (PatternStep step in phaseAttackSequence.patternSteps)
            {
                if (step.pattern is Hailstorm hailStorm)
                {
                    hailStorm.StartPattern(boss.transform.position);
                }
            }
            // same logic as enemy controller sequences
            attackRoutine = boss.StartCoroutine(AttackSequence(boss.transform.position, boss));
            moveRoutine = boss.StartCoroutine(MovementSequence(boss.transform.position, boss, boss.rb));
        }

        public override void EndPhase(BossManager boss)
        {
            boss.StopCoroutine(attackRoutine);
            boss.StopCoroutine(moveRoutine);
        }
    }
}

