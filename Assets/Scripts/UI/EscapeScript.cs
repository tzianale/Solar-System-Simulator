using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class EscapeScript : MonoBehaviour
    {

        public GameObject panel;

        public GameObject Panel
        {
            get { return panel; }
            set { panel = value; }
        }
        public void openPanel()
        {
            if (Panel != null)
            {
                Panel.SetActive(true);
            }
        }

        public void closePanel()
        {
            if (Panel != null)
            {
                Panel.SetActive(false);
            }
        }

        private void escapeGame()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}
