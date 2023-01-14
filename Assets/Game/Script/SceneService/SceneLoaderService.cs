using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.SceneService
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly HashSet<SceneType> _loadedScenes = new();
        private readonly HashSet<SceneType> _loadingScenes = new();

        
        private static readonly Lazy<SceneLoaderService> LazyInstance = new(() => new SceneLoaderService());    
        
        public static ISceneLoaderService Instance => LazyInstance.Value;
        
        private SceneLoaderService()
        {
            var countLoaded = SceneManager.sceneCount;
            for (var i = 0; i < countLoaded; i++)
            {
                var sceneName = SceneManager.GetSceneAt(i).name;
                var sceneType = Enum.Parse<SceneType>(sceneName);
                var isSceneNotInHashSet = !_loadedScenes.Contains(sceneType);
                if (isSceneNotInHashSet)
                {
                    _loadedScenes.Add(sceneType);
                }
            }
        }
        
        public bool IsSceneLoaded(SceneType sceneType)
        {
            return _loadedScenes.Contains(sceneType);
        }

        public async Task<bool> LoadScene(SceneType sceneType)
        {
            var canLoadScene = !_loadedScenes.Contains(sceneType) && !_loadingScenes.Contains(sceneType);
            if (canLoadScene)
            {
                _loadingScenes.Add(sceneType);
                await SceneManager.LoadSceneAsync(sceneType.ToString(),  LoadSceneMode.Additive);
                _loadingScenes.Remove(sceneType);
                _loadedScenes.Add(sceneType);
                return true;
            }
            else
            {
                Debug.LogError($"sceneType:{sceneType} is either Loading or already Loaded");
                return false;
            }
        }

        public async Task<bool> UnloadScene(SceneType sceneType)
        {
            var canUnloadScene = _loadedScenes.Contains(sceneType) && !_loadingScenes.Contains(sceneType);
            if (canUnloadScene)
            {
                await SceneManager.UnloadSceneAsync(sceneType.ToString());
                _loadedScenes.Remove(sceneType);
                return true;
            }
            else
            {
                Debug.LogError($"sceneType:{sceneType} cant be unloaded as it is not Loaded");
                return false;
            }
        }
        
    }
}