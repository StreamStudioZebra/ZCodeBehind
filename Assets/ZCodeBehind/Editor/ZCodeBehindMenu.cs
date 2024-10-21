using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable InconsistentNaming
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable InvertIf
// ReSharper disable AssignNullToNotNullAttribute

namespace ZCodeBehind.Editor
{
    public static class ZCodeBehindMenu
    {
        [MenuItem("Z/CODE_BEHIND/SYNC")]
        public static async void Sync()
        {
            var scene = SceneManager.GetActiveScene();

            if (scene != null)
            {
                Debug.Log($"SYNC_START scene.path:{scene.path}");
                var rootGoList = scene.GetRootGameObjects();
                Debug.Log($"rootGoList:{rootGoList.Length}");
                await _Run(rootGoList, scene.path, false);
            }

            var prefab = PrefabStageUtility.GetCurrentPrefabStage();

            if (prefab != null)
            {
                Debug.Log($"SYNC_START prefab.assetPath:{prefab.assetPath}");
                var rootGoList = new[] { prefab.prefabContentsRoot };
                Debug.Log($"rootGoList:{rootGoList.Length}");
                await _Run(rootGoList, prefab.assetPath, true);
            }
        }

        private static async Task _Run(GameObject[] rootGoList, string assetPath, bool exportFieldPublic)
        {
            var goCompDict = new Dictionary<string, List<string>>();

            foreach (var rootGo in rootGoList)
            {
                _Sync(rootGo, goCompDict);
            }

            var rootDirPath = Path.Combine(Application.dataPath, "..");
            rootDirPath = Path.GetFullPath(rootDirPath);
            Debug.Log($"rootDirPath:{rootDirPath}");

            var fileName = Path.GetFileNameWithoutExtension(assetPath);
            Debug.Log($"fileName:{fileName}");
            var cbPath = Path.Combine(Path.GetDirectoryName(assetPath), $"ZCodeBehind/{fileName}.zcb.cs");
            cbPath = Path.GetFullPath(cbPath);
            Debug.Log($"cbPath:{cbPath}:{File.Exists(cbPath)}");
            string[] fileLineList;
            var newFileLineList = new List<string>();
            long newFileLength;

            {
                await using var fs = File.OpenRead(assetPath);
                newFileLength = fs.Length;
                Debug.Log($"newFileLength:{newFileLength}");
            }

            if (File.Exists(cbPath))
            {
                fileLineList = await File.ReadAllLinesAsync(cbPath);
                Debug.Log($"fileLineList:{fileLineList.Length}");
            }
            else
            {
                var templatePath = Path.Combine(rootDirPath, "Assets/ZCodeBehind/Editor/ZCodeBehind.cs.template");

                if (File.Exists(templatePath))
                {
                    fileLineList = await File.ReadAllLinesAsync(templatePath);
                }
                else
                {
                    var packCacheDirPath = Path.Combine(Application.dataPath, "..", "Library/PackageCache");
                    packCacheDirPath = Path.GetFullPath(packCacheDirPath);
                    var zcbDirPath = Directory.GetDirectories(packCacheDirPath).FirstOrDefault(dirPath => dirPath.Contains("app.streamstudio.zcodebehind"));
                    templatePath = Path.Join(zcbDirPath, "Editor/ZCodeBehind.cs.template");
                    fileLineList = await File.ReadAllLinesAsync(templatePath);
                }

                var cbDirPath = Path.GetDirectoryName(cbPath);
                Debug.Log($"cbDirPath:{cbDirPath}");

                if (!Directory.Exists(cbDirPath))
                    Directory.CreateDirectory(cbDirPath);
            }

            var canSkip = false;
            var underBarStr = exportFieldPublic ? "" : "_";

            foreach (var fileLine in fileLineList)
            {
                if (fileLine.Contains("ZCODEBEHIND_SCENE_FILE_LENGTH"))
                {
                    newFileLineList.Add($"// ZCODEBEHIND_SCENE_FILE_LENGTH {newFileLength}");
                }
                else if (fileLine.Contains("%CLASS_NAME%"))
                {
                    newFileLineList.Add(fileLine.Replace("%CLASS_NAME%", fileName));
                }
                else if (fileLine.Contains("%GO_LIST_QUERY%"))
                {
                    var newLine = exportFieldPublic ? "var goList = ZcbUtil.GetDescendantList(gameObject);" : "var goList = Resources.FindObjectsOfTypeAll<GameObject>().ToList();";
                    newFileLineList.Add(fileLine.Replace("%GO_LIST_QUERY%", newLine));
                }
                else if (fileLine.Contains("ZCODEBEHIND_FIELD_START"))
                {
                    canSkip = true;
                    newFileLineList.Add($"    // ZCODEBEHIND_FIELD_START");

                    foreach (var kv in goCompDict)
                    {
                        var goName = kv.Key;
                        var compNameList = kv.Value;

                        if (goName == fileName)
                            continue;

                        var goNameClean = Regex.Replace(goName, @"[^a-zA-Z0-9_]", "");
                        var fieldLine = "";
                        var pubPrvStr = exportFieldPublic ? "public" : "private";
                        fieldLine += $"    {pubPrvStr} (GameObject go, ";

                        foreach (var compName in compNameList)
                        {
                            var ccCompName = $"{compName[0].ToString().ToLower()}{compName[1..]}";
                            fieldLine += $"{compName} {ccCompName}, ";
                        }

                        fieldLine = fieldLine.TrimEnd(' ', ',');
                        fieldLine += $") {underBarStr}{goNameClean};";
                        Debug.Log($"fieldLine:{fieldLine}");
                        newFileLineList.Add(fieldLine);
                    }
                }
                else if (fileLine.Contains("ZCODEBEHIND_FIELD_FIN"))
                {
                    canSkip = false;
                    newFileLineList.Add($"    // ZCODEBEHIND_FIELD_FIN");
                }
                else if (fileLine.Contains("ZCODEBEHIND_CASE_START"))
                {
                    canSkip = true;
                    newFileLineList.Add($"                // ZCODEBEHIND_CASE_START");

                    foreach (var kv in goCompDict)
                    {
                        var goName = kv.Key;
                        var compNameList = kv.Value;

                        if (goName == fileName)
                            continue;

                        var goNameClean = Regex.Replace(goName, @"[^a-zA-Z0-9_]", "");
                        newFileLineList.Add($"                case \"{goName}\":");
                        newFileLineList.Add($"                {{");
                        newFileLineList.Add($"                    if ({underBarStr}{goNameClean}.go != null)");
                        newFileLineList.Add($"                        break;");
                        newFileLineList.Add($"                    {underBarStr}{goNameClean}.go = go;");

                        foreach (var compName in compNameList)
                        {
                            var ccCompName = $"{compName[0].ToString().ToLower()}{compName[1..]}";
                            newFileLineList.Add($"                    {underBarStr}{goNameClean}.{ccCompName} = go.GetComponent<{compName}>();");
                        }

                        newFileLineList.Add($"                    break;");
                        newFileLineList.Add($"                }}");
                    }
                }
                else if (fileLine.Contains("ZCODEBEHIND_CASE_FIN"))
                {
                    canSkip = false;
                    newFileLineList.Add($"                // ZCODEBEHIND_CASE_FIN");
                }
                else
                {
                    if (!canSkip)
                        newFileLineList.Add(fileLine);
                }
            }

            Debug.Log($"newFileLineList:{newFileLineList.Count}");
            await File.WriteAllLinesAsync(cbPath, newFileLineList);
        }

        private static void _Sync(GameObject go, Dictionary<string, List<string>> goCompDict)
        {
            if (!goCompDict.ContainsKey(go.name))
            {
                var compArray = go.GetComponents<Component>();
                Debug.Log($"go.name:{go.name} compArray:{compArray.Length}");
                goCompDict[go.name] = new List<string>();

                foreach (var comp in compArray)
                {
                    if (comp == null)
                        continue;
                    Debug.Log($"comp:{comp.GetType().Name}");
                    goCompDict[go.name].Add(comp.GetType().Name);
                }
            }

            foreach (Transform childTr in go.transform)
            {
                _Sync(childTr.gameObject, goCompDict);
            }
        }
    }
}