using UnityEngine;
using UnityEngine.SceneManagement;

namespace Models
{
    public class SettingScript : MonoBehaviour
    {
        public void loadScene()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}