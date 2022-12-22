using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private GameObject _loadScreenGO;
        [SerializeField] private Image _progressBar;

        public void Load()
        {
            StartCoroutine(LoadCoroutine());
        }

        private IEnumerator LoadCoroutine()
        {
            var scene = SceneManager.LoadSceneAsync(_sceneName);
            _loadScreenGO.SetActive(true);
            while (!scene.isDone)
            {
                _progressBar.fillAmount = Mathf.Clamp01(scene.progress/0.9f);
                yield return null;
            }
 
        }
    }
}
