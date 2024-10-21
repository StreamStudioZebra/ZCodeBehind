// ReSharper disable CheckNamespace
// ReSharper disable ConvertSwitchStatementToSwitchExpression
// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedField.Local
// ReSharper disable RedundantUsingDirective

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;
using UnityEngine.IO;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine;
using ZCodeBehind.Runtime;

public partial class ZCodeBehindTest : MonoBehaviour
{
    // ZCODEBEHIND_FIELD_START
    private (GameObject go, Transform transform) _COMMON;
    private (GameObject go, Transform transform, Camera camera, AudioListener audioListener, UniversalAdditionalCameraData universalAdditionalCameraData) _mainCam;
    private (GameObject go, Transform transform, EventSystem eventSystem, StandaloneInputModule standaloneInputModule) _eventSystem;
    private (GameObject go, Transform transform) _mainScript;
    private (GameObject go, RectTransform rectTransform, Canvas canvas, CanvasScaler canvasScaler, GraphicRaycaster graphicRaycaster) _cvMain;
    private (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image) _imgBg;
    private (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image) _imgFg;
    private (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image) _imgFg123abc456;
    private (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image) _divLine;
    private (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image, ZMyPrefab zMyPrefab) _myPrefab_0;
    private (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image) _imgRed;
    private (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image) _imgBlue;
    private (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Text text, ContentSizeFitter contentSizeFitter) _txtName;
    private (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image, ZMyPrefab zMyPrefab) _myPrefab_1;
    private (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image, ZMyPrefab zMyPrefab) _myPrefab_2;
    // ZCODEBEHIND_FIELD_FIN
    private (GameObject go, RectTransform rectTransform, Image image) _imgZCodeBehindTest;

    public void InitializeComponent()
    {
        var goList = Resources.FindObjectsOfTypeAll<GameObject>().ToList();

        foreach (var go in goList)
        {
            // ZCODEBEHIND_IF_ROOT_START
            // ZCODEBEHIND_IF_ROOT_FIN
                
            switch (go.name)
            {
                // ZCODEBEHIND_CASE_START
                case "COMMON":
                {
                    if (_COMMON.go != null)
                        break;
                    _COMMON.go = go;
                    _COMMON.transform = go.GetComponent<Transform>();
                    break;
                }
                case "mainCam":
                {
                    if (_mainCam.go != null)
                        break;
                    _mainCam.go = go;
                    _mainCam.transform = go.GetComponent<Transform>();
                    _mainCam.camera = go.GetComponent<Camera>();
                    _mainCam.audioListener = go.GetComponent<AudioListener>();
                    _mainCam.universalAdditionalCameraData = go.GetComponent<UniversalAdditionalCameraData>();
                    break;
                }
                case "eventSystem":
                {
                    if (_eventSystem.go != null)
                        break;
                    _eventSystem.go = go;
                    _eventSystem.transform = go.GetComponent<Transform>();
                    _eventSystem.eventSystem = go.GetComponent<EventSystem>();
                    _eventSystem.standaloneInputModule = go.GetComponent<StandaloneInputModule>();
                    break;
                }
                case "mainScript":
                {
                    if (_mainScript.go != null)
                        break;
                    _mainScript.go = go;
                    _mainScript.transform = go.GetComponent<Transform>();
                    break;
                }
                case "cvMain":
                {
                    if (_cvMain.go != null)
                        break;
                    _cvMain.go = go;
                    _cvMain.rectTransform = go.GetComponent<RectTransform>();
                    _cvMain.canvas = go.GetComponent<Canvas>();
                    _cvMain.canvasScaler = go.GetComponent<CanvasScaler>();
                    _cvMain.graphicRaycaster = go.GetComponent<GraphicRaycaster>();
                    break;
                }
                case "imgBg":
                {
                    if (_imgBg.go != null)
                        break;
                    _imgBg.go = go;
                    _imgBg.rectTransform = go.GetComponent<RectTransform>();
                    _imgBg.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    _imgBg.image = go.GetComponent<Image>();
                    break;
                }
                case "imgFg":
                {
                    if (_imgFg.go != null)
                        break;
                    _imgFg.go = go;
                    _imgFg.rectTransform = go.GetComponent<RectTransform>();
                    _imgFg.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    _imgFg.image = go.GetComponent<Image>();
                    break;
                }
                case "imgFg 123 abc 한글 안돼 456":
                {
                    if (_imgFg123abc456.go != null)
                        break;
                    _imgFg123abc456.go = go;
                    _imgFg123abc456.rectTransform = go.GetComponent<RectTransform>();
                    _imgFg123abc456.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    _imgFg123abc456.image = go.GetComponent<Image>();
                    break;
                }
                case "divLine":
                {
                    if (_divLine.go != null)
                        break;
                    _divLine.go = go;
                    _divLine.rectTransform = go.GetComponent<RectTransform>();
                    _divLine.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    _divLine.image = go.GetComponent<Image>();
                    break;
                }
                case "myPrefab_0":
                {
                    if (_myPrefab_0.go != null)
                        break;
                    _myPrefab_0.go = go;
                    _myPrefab_0.rectTransform = go.GetComponent<RectTransform>();
                    _myPrefab_0.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    _myPrefab_0.image = go.GetComponent<Image>();
                    _myPrefab_0.zMyPrefab = go.GetComponent<ZMyPrefab>();
                    break;
                }
                case "imgRed":
                {
                    if (_imgRed.go != null)
                        break;
                    _imgRed.go = go;
                    _imgRed.rectTransform = go.GetComponent<RectTransform>();
                    _imgRed.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    _imgRed.image = go.GetComponent<Image>();
                    break;
                }
                case "imgBlue":
                {
                    if (_imgBlue.go != null)
                        break;
                    _imgBlue.go = go;
                    _imgBlue.rectTransform = go.GetComponent<RectTransform>();
                    _imgBlue.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    _imgBlue.image = go.GetComponent<Image>();
                    break;
                }
                case "txtName":
                {
                    if (_txtName.go != null)
                        break;
                    _txtName.go = go;
                    _txtName.rectTransform = go.GetComponent<RectTransform>();
                    _txtName.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    _txtName.text = go.GetComponent<Text>();
                    _txtName.contentSizeFitter = go.GetComponent<ContentSizeFitter>();
                    break;
                }
                case "myPrefab_1":
                {
                    if (_myPrefab_1.go != null)
                        break;
                    _myPrefab_1.go = go;
                    _myPrefab_1.rectTransform = go.GetComponent<RectTransform>();
                    _myPrefab_1.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    _myPrefab_1.image = go.GetComponent<Image>();
                    _myPrefab_1.zMyPrefab = go.GetComponent<ZMyPrefab>();
                    break;
                }
                case "myPrefab_2":
                {
                    if (_myPrefab_2.go != null)
                        break;
                    _myPrefab_2.go = go;
                    _myPrefab_2.rectTransform = go.GetComponent<RectTransform>();
                    _myPrefab_2.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    _myPrefab_2.image = go.GetComponent<Image>();
                    _myPrefab_2.zMyPrefab = go.GetComponent<ZMyPrefab>();
                    break;
                }
                // ZCODEBEHIND_CASE_FIN
                case "imgZCodeBehindTest":
                {
                    if (_imgZCodeBehindTest.go != null)
                        break;
                    _imgZCodeBehindTest.go = go;
                    _imgZCodeBehindTest.rectTransform = go.GetComponent<RectTransform>();
                    _imgZCodeBehindTest.image = go.GetComponent<Image>();
                    break;
                }
            }
        }
    }
        
    private void Awake()
    {
        InitializeComponent();
    }
}
