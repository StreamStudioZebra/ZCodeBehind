# ZCodeBehind

## Introduction
- You can use the code behind feature of WPF, ASP.NET in Unity UGUI.
- You can free yourself from countless mouse clicks and repetitive coding work to connect fields of Unity scenes and scripts.
- Think of countless blocking dialogs of Unity Editor during this connection work.
- Now you can easily change the names of GameObjects in the scene, because all names are updated every time you press the Sync button in the menu.

## Installation

- in `manifest.json`

```
"dependencies": {
"app.streamstudio.zcodebehind": "https://github.com/StreamStudioZebra/ZCodeBehind.git?path=Assets/ZCodeBehind",
```

## Usage

1. Open a scene in Unity Editor: `MyScene.unity`
2. Create a script with the same name as the scene: `MyScene.cs`
3. Click Z/CODE_BEHIND/SYNC in the menu
4. Now, in MyScene.cs, all GameObjects and their subcomponents can be accessed with stable types and correct field names.

```
public partial class ZCodeBehindTest : MonoBehaviour
{
    private void Start()
    {
        _InitializeComponent();
        _imgFg.go.SetActive(true);
        _imgFg.image.color = Color.blue;
        _imgBg.go.SetActive(true);
        _imgBg.image.enabled = true;
        _imgBg.image.color = Color.cyan;
    }
}
```

## Demo

[![Video Title](https://img.youtube.com/vi/AGVBH-8uX2o/0.jpg)](https://www.youtube.com/watch?v=AGVBH-8uX2o)