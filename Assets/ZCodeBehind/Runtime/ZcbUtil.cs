using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable CheckNamespace
// ReSharper disable InvertIf

namespace ZCodeBehind.Runtime
{
    public static class ZcbUtil
    {
        public static List<GameObject> GetAllObjListInCurScene()
        {
            var scene = SceneManager.loadedSceneCount == 0 ? SceneManager.GetActiveScene() : SceneManager.GetSceneAt(SceneManager.loadedSceneCount - 1);
            var allObjList = GetAllObjListInScene(scene);
            return allObjList;
        }

        private static void _GetAllChildObjList(Transform parentTr, List<GameObject> allObjList)
        {
            for (var i = 0; i < parentTr.childCount; i++)
            {
                var childTr = parentTr.GetChild(i);
                allObjList.Add(childTr.gameObject);
                _GetAllChildObjList(childTr, allObjList);
            }
        }

        public static List<GameObject> GetAllObjList(GameObject goParent)
        {
            var goList = new List<GameObject>();
            goList.Add(goParent);
            _GetAllChildObjList(goParent.transform, goList);
            return goList;
        }

        public static List<GameObject> GetAllObjListInScene(Scene scene)
        {
            var allObjList = new List<GameObject>();
            var rootObjList = scene.GetRootGameObjects();

            foreach (var rootGo in rootObjList)
            {
                allObjList.Add(rootGo);
                _GetAllChildObjList(rootGo.transform, allObjList);
            }

            return allObjList;
        }

        public static List<GameObject> GetAllObjListInScene(string sceneName)
        {
            Scene? targetScene = null;

            for (var i = SceneManager.loadedSceneCount - 1; i >= 0; i--)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (scene.name == sceneName)
                {
                    targetScene = scene;
                    break;
                }
            }

            if (targetScene == null)
                return new List<GameObject>();

            var allObjList = GetAllObjListInScene(targetScene.Value);
            return allObjList;
        }
    }
}
