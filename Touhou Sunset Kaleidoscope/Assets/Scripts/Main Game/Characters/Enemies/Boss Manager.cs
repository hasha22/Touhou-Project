using System.Collections;
using UnityEngine;
namespace KH
{
    public class BossManager : MonoBehaviour
    {
        //here goes nothin...
        [Header("Boss Data")]
        [SerializeField] private Boss bossData;
        [SerializeField] private int currentBossHealth;

        [Header("Phases")]
        public BossPhase[] phases;
        private int currentPhaseIndex = 0;
        private BossPhase currentPhase;

        [Header("Movement")]
        [SerializeField] private MovementSequence currentMovementSequence;

        [Header("Boss Attacks")]
        [SerializeField] private AttackSequence currentAttackSequence;

        private Coroutine phaseRoutine;
        private void Start()
        {
            StartNextPhase();
        }
        public void StartNextPhase()
        {
            if (currentPhaseIndex >= phases.Length)
            {
                // defeat boss logic
                return;
            }

            currentPhase = phases[currentPhaseIndex];
            currentPhaseIndex++;

            phaseRoutine = StartCoroutine(PhaseRoutine(currentPhase));
        }
        private IEnumerator PhaseRoutine(BossPhase phase)
        {
            //set boss phase hp
            phase.StartPhase(this);

            float timer = 0f;
            while (timer < phase.duration) // && boss has health
            {
                //phase.UpdatePhase(this, Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }

            phase.EndPhase(this);
            yield return new WaitForSeconds(1f); // small delay between phases

            StartNextPhase(); // starts next phase
        }
        private void OnBossDefeated()
        {
            // trigger items drops, spell card bonus, etc.
        }
    }
}

