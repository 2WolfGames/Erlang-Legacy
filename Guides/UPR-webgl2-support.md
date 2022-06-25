
## The problematic

We needed to add lighting to our project. We made use of Unity Universal Pipeline Renderer to acomplish this.

The problem is that the pipeline is not compatible with WebGL2 by default and when we build the project it generates a blue scene.

We solves this problems thanks to this post [here](https://www.reddit.com/r/Unity2D/comments/iqaky0/need_help_blue_screen_on_webgl_build/).

We switched default color space to linear in player settings and that solved the problem.

The thing here is that our spine projects imported in scene are not fully compatible with linear color.

```
    Problematic material setup at Skeleton: 
    Warning: Premultiply-alpha atlas textures not supported in Linear color space!You can use a straight alpha texture with 'PMA Vertex Color' by choosing blend mode 'PMA Vertex, Straight Texture'.

    If you have a PMA Texture, please
    a) re-export atlas as straight alpha texture with 'premultiply alpha' unchecked
    (if you have already done this, please set the 'Straight Alpha Texture' Material parameter to 'true') or
    b) switch to Gamma color space via
    Project Settings - Player - Other Settings - Color Space.

    UnityEngine.Debug:LogWarningFormat (UnityEngine.Object,string,object[])
    Spine.Unity.SkeletonRenderer:Initialize (bool,bool) (at Assets/3rdParty/Spine/Runtime/spine-unity/Components/SkeletonRenderer.cs:422)
    Spine.Unity.SkeletonMecanim:Initialize (bool,bool) (at Assets/3rdParty/Spine/Runtime/spine-unity/Components/SkeletonMecanim.cs:76)
    Spine.Unity.SkeletonRenderer:Awake () (at Assets/3rdParty/Spine/Runtime/spine-unity/Components/SkeletonRenderer.cs:321)
    UnityEngine.GUIUtility:ProcessEvent (int,intptr)
```

It seems that we need to re-import spine project reimporting atlas as straight alpha texture with 'premultiply alpha' unchecked.

---

To solve warning we:
1. Export Ajax atlas without checking Premultiply-alpha
2. Changed Project Setings >> Player >> Color to Gamma

Now, but, webGL still does not work.

