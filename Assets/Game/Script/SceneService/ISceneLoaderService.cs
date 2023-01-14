using System.Threading.Tasks;

namespace Script.SceneService
{
    public interface ISceneLoaderService
    {
        bool IsSceneLoaded(SceneType sceneType);
        Task<bool> LoadScene(SceneType sceneType);
        Task<bool> UnloadScene(SceneType sceneType);
    }
}