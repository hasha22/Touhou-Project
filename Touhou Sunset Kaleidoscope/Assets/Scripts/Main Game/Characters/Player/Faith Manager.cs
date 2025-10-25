using UnityEngine;
namespace KH
{
    public class FaithManager : MonoBehaviour
    {
        [Header("Faith Settings")]
        public float faith = 0f;
        public float faithThreshold = 6000f;
        public float faithDecayRate = 50f;
        public GameObject faithAuraPrefab;

        private bool auraActive = false;
        private GameObject activeAura;

        private void Update()
        {
            // will change to have the aura lerp to a certain alpha / intensity as faith increases
            // after the threshold is passed the effect becomes active
            faith = Mathf.Max(0, faith - faithDecayRate * Time.deltaTime);

            if (faith >= faithThreshold && !auraActive)
            { ActivateFaithAura(); }
            else if (faith < faithThreshold && auraActive)
            { DeactivateFaithAura(); }
        }
        public void AddFaith(float amount)
        {
            faith += amount;
        }
        private void ActivateFaithAura()
        {
            auraActive = true;
            activeAura = Instantiate(faithAuraPrefab, transform);
            //activeAura.GetComponent<FaithAura>().AttachToPlayer(this);
        }
        private void DeactivateFaithAura()
        {
            auraActive = false;
            if (activeAura)
                Destroy(activeAura);
        }
    }
}

