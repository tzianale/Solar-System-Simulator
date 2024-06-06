using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using System.Collections;
using Models;

namespace Tests.PlayMode
{
    public class SettingScriptTests : MonoBehaviour
    {
        private GameObject testGameObject;

        [SetUp]
        public void Setup()
        {
            testGameObject = new GameObject();
        }

        [UnityTest]
        public IEnumerator TestLoadScene()
        {
            var settingScript = testGameObject.AddComponent<SettingScript>();

            settingScript.loadScene();

            yield return null;

            Assert.IsTrue(SceneManager.GetSceneAt(0).isLoaded, "Die Szene 0 wird nicht geladen.");
        }

        [TearDown]
        public void TearDown()
        {
            if (testGameObject != null)
            {
                GameObject.Destroy(testGameObject);
            }
        }
    }
}
