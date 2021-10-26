#if UNITY_EDITOR
using UnityEditor;
#else
using UnityEngine;
#endif

public static class QuitHelper
{
    public static void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}