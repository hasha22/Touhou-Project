using UnityEngine;
namespace KH
{
    public class StageManager : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Transform playableArea;
        public static StageManager instance { get; private set; }
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

    }
}
