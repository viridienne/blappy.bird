using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    [Serializable]
    public struct SceneData
    {
        public int LoadIndex;
        public string SceneName;
    }
    public class SceneLoader : BaseSingletonMono<SceneLoader>
    {
        [SerializeField] private SceneData[] _scenes;
       
        private async void Start()
        {
            for (int i = 0; i < _scenes.Length; i++)
            {
                await LoadSceneAsync(i);
            }
        }
        
        private async Task LoadSceneAsync(int index)
        {
            if (index < 0 || index >= _scenes.Length)
            {
                Debug.LogError("Invalid scene index");
                return;
            }
            
            var sceneData = _scenes[index];
            if (string.IsNullOrEmpty(sceneData.SceneName))
            {
                Debug.LogError("Invalid scene name");
                return;
            }
            if(sceneData.SceneName == SceneManager.GetActiveScene().name)
            {
                Debug.LogWarning("Scene is already loaded");
                return;
            }
            
            var asyncOperation = SceneManager.LoadSceneAsync(sceneData.SceneName, LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                await Task.Yield();
            }
        }
    }
}

