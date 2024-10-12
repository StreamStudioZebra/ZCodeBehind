using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable CheckNamespace

public static class ZCodeBehindMenu
{
    [MenuItem("Z/CODE_BEHIND/SYNC")]
    public static async void Sync()
    {
        var scenePath = SceneManager.GetActiveScene().path;
        Debug.Log($"SYNC_START scenePath:{scenePath}");
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        var rootGoList = scene.GetRootGameObjects();
        Debug.Log($"rootGoList:{rootGoList.Length}");
        var goCompDict = new Dictionary<string, List<string>>();

        foreach (var rootGo in rootGoList)
        {
            _Sync(rootGo, goCompDict);
        }

        var rootDirPath = Path.Combine(Application.dataPath, "..");
        rootDirPath = Path.GetFullPath(rootDirPath);
        Debug.Log($"rootDirPath:{rootDirPath}");

        var sceneName = Path.GetFileNameWithoutExtension(scenePath);
        Debug.Log($"sceneName:{sceneName}");
        var cbPath = Path.Combine(Path.GetDirectoryName(scenePath), $"ZCodeBehind/{sceneName}.zcb.cs");
        cbPath = Path.GetFullPath(cbPath);
        Debug.Log($"cbPath:{cbPath}:{File.Exists(cbPath)}");
        string[] fileLineList;
        var newFileLineList = new List<string>();
        long newSceneFileLength;

        {
            await using var fs = File.OpenRead(scenePath);
            newSceneFileLength = fs.Length;
            Debug.Log($"newSceneFileLength:{newSceneFileLength}");
        }

        if (File.Exists(cbPath))
        {
            fileLineList = await File.ReadAllLinesAsync(cbPath);
            Debug.Log($"fileLineList:{fileLineList.Length}");
            var line = fileLineList.First(item => item.Contains("ZCODEBEHIND_SCENE_FILE_LENGTH"));
            var sceneFileLength = long.Parse(line.Replace("//", "").Replace("ZCODEBEHIND_SCENE_FILE_LENGTH", "").Trim());

            if (sceneFileLength == newSceneFileLength)
            {
                Debug.Log($"SCENE_FILE_IS_IDENTICAL_SKIP scenePath:{scenePath}");
                return;
            }
        }
        else
        {
            var templatePath = Path.Combine(rootDirPath, "Assets/ZCodeBehind/Editor/ZCodeBehind.cs.template");
            fileLineList = await File.ReadAllLinesAsync(templatePath);

            var cbDirPath = Path.GetDirectoryName(cbPath);
            Debug.Log($"cbDirPath:{cbDirPath}");

            if (!Directory.Exists(cbDirPath))
                Directory.CreateDirectory(cbDirPath);
        }

        var canSkip = false;

        foreach (var fileLine in fileLineList)
        {
            if (fileLine.Contains("ZCODEBEHIND_SCENE_FILE_LENGTH"))
            {
                newFileLineList.Add($"// ZCODEBEHIND_SCENE_FILE_LENGTH {newSceneFileLength}");
            }
            else if (fileLine.Contains("%CLASS_NAME%"))
            {
                newFileLineList.Add(fileLine.Replace("%CLASS_NAME%", sceneName));
            }
            else if (fileLine.Contains("ZCODEBEHIND_FIELD_START"))
            {
                canSkip = true;
                newFileLineList.Add($"    // ZCODEBEHIND_FIELD_START");

                foreach (var kv in goCompDict)
                {
                    var goName = kv.Key;
                    var compNameList = kv.Value;
                    var goNameClean = Regex.Replace(goName, @"[^a-zA-Z0-9_]", "");
                    var fieldLine = "";
                    fieldLine += "    private (GameObject go, ";

                    foreach (var compName in compNameList)
                    {
                        var ccCompName = $"{compName[0].ToString().ToLower()}{compName[1..]}";
                        fieldLine += $"{compName} {ccCompName}, ";
                    }

                    fieldLine = fieldLine.TrimEnd(' ', ',');
                    fieldLine += $") _{goNameClean};";
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
                    var goNameClean = Regex.Replace(goName, @"[^a-zA-Z0-9_]", "");
                    newFileLineList.Add($"                case \"{goName}\":");
                    newFileLineList.Add($"                {{");
                    newFileLineList.Add($"                    if (_{goNameClean}.go != null)");
                    newFileLineList.Add($"                        break;");
                    newFileLineList.Add($"                    _{goNameClean}.go = go;");

                    foreach (var compName in compNameList)
                    {
                        var ccCompName = $"{compName[0].ToString().ToLower()}{compName[1..]}";
                        newFileLineList.Add($"                    _{goNameClean}.{ccCompName} = go.GetComponent<{compName}>();");
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