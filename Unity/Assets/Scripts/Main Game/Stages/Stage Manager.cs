using UnityEngine;
namespace KH
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager instance { get; private set; }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }

        }

    }
}
