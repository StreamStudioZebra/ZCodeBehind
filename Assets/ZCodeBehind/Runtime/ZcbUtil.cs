using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZCodeBehind.Runtime
{
    public static class ZcbUtil
    {
        public static List<GameObject> GetAllObjListInCurScene()
        {
            var allObjList = new List<GameObject>();
            var scene = SceneManager.GetActiveScene();
            var rootObjList = scene.GetRootGameObjects();

            foreach (var rootGo in rootObjList)
            {
                allObjList.Add(rootGo);
                _GetAllChildObjList(rootGo.transform, allObjList);
            }

            return allObjList;
        }

        private static void _GetAllChildObjList(Transform parentTr, List<GameObject> allObjList)
        {
            foreach (Transform childTr in parentTr)
            {
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
    }
}
