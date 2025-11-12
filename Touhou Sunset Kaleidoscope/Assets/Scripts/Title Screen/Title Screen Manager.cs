using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace KH
{
    public class TitleScreenManager : MonoBehaviour
    {

        [Header("Scene Indexes")]
        public int gameSceneIndex = 1;

        [Header("Menus")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject controlsMenu;
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
        public void EnableControlsMenu()
        {
            mainMenu.SetActive(false);
            controlsMenu.SetActive(true);
        }
        public void DisableControlsMenu()
        {
            mainMenu.SetActive(true);
            controlsMenu.SetActive(false);
        }
    }
}