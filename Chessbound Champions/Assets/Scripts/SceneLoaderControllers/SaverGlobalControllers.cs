using UnityEngine;

namespace SceneLoaderControllers
{
    public class SaverGlobalControllers : MonoBehaviour
    {
        private void Start()
        {
            var objs = GameObject.FindGameObjectsWithTag("GlobalControllers");

            if (objs.Length > 1)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
    }
}