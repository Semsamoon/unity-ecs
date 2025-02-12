using UnityEngine;

namespace ECS
{
    public static class Logger
    {
        public static void Warning(string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"<ECS <color=yellow>WARNING</color>> {message}");
#else
            Debug.LogWarning($"<ECS WARNING> {message}");
#endif
        }

        public static void Error(string message)
        {
#if UNITY_EDITOR
            Debug.LogError($"<ECS <color=red>ERROR</color>> {message}");
#else
            Debug.LogError($"<ECS ERROR> {message}");
#endif
        }
    }
}