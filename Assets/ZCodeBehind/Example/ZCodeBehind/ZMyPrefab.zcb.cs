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

public partial class ZMyPrefab : MonoBehaviour
{
    // ZCODEBEHIND_FIELD_START
    public (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image) imgRed;
    public (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Image image) imgBlue;
    public (GameObject go, RectTransform rectTransform, CanvasRenderer canvasRenderer, Text text, ContentSizeFitter contentSizeFitter) txtName;
    // ZCODEBEHIND_FIELD_FIN
    private (GameObject go, RectTransform rectTransform, Image image) _imgZCodeBehindTest;

    private void _InitializeComponent()
    {
        var goList = ZcbUtil.GetDescendantList(gameObject);

        foreach (var go in goList)
        {
            switch (go.name)
            {
                // ZCODEBEHIND_CASE_START
                case "imgRed":
                {
                    if (imgRed.go != null)
                        break;
                    imgRed.go = go;
                    imgRed.rectTransform = go.GetComponent<RectTransform>();
                    imgRed.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    imgRed.image = go.GetComponent<Image>();
                    break;
                }
                case "imgBlue":
                {
                    if (imgBlue.go != null)
                        break;
                    imgBlue.go = go;
                    imgBlue.rectTransform = go.GetComponent<RectTransform>();
                    imgBlue.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    imgBlue.image = go.GetComponent<Image>();
                    break;
                }
                case "txtName":
                {
                    if (txtName.go != null)
                        break;
                    txtName.go = go;
                    txtName.rectTransform = go.GetComponent<RectTransform>();
                    txtName.canvasRenderer = go.GetComponent<CanvasRenderer>();
                    txtName.text = go.GetComponent<Text>();
                    txtName.contentSizeFitter = go.GetComponent<ContentSizeFitter>();
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
        _InitializeComponent();
    }
}
