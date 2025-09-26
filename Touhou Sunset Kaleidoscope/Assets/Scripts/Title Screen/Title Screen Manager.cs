using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace KH
{
    public class TitleScreenManager : MonoBehaviour
    {

        [Header("Scene Indexes")]
        [SerializeField] public int gameSceneIndex = 1;
        private void Awake()
        {

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