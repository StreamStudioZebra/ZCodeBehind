using UnityEngine;

public partial class ZCodeBehindTest : MonoBehaviour
{
    private void Start()
    {
        _imgFg.go.SetActive(true);
        _imgFg.image.color = Color.blue;
        _imgBg.go.SetActive(true);
        _imgBg.image.enabled = true;
        _imgBg.image.color = Color.cyan;
        _myPrefab_0.zMyPrefab.imgBlue.image.color = Color.gray;
    }
}