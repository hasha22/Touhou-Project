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

