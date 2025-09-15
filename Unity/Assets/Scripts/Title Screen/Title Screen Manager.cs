using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace KH
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance { get; private set; }

        [Header("Scene Indexes")]
        [SerializeField] public int gameSceneIndex = 1;
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
        public void QuitGame()
        {
            Application.Quit();
        }
        public void GameStart()
        {
            StartCoroutine(LoadGameScene());
        }
        public IEnumerator LoadGameScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(gameSceneIndex);
            yield return null;
        }
    }
}