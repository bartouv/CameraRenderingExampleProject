using Script.SceneService;
using UnityEngine;

namespace Script
{
    public class ExampleLoader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SceneLoaderService.Instance.LoadScene(SceneType.CanvasScene);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
