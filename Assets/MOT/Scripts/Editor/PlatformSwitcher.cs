using UnityEditor;

namespace MOT.Editor
{
    /// <summary>
    /// Switches the current Mist of Time platform
    /// </summary>
    public static class PlatformSwitcher
    {
        /// <summary>
        /// Switches the platform to StandaloneWindows
        /// </summary>
        [MenuItem("Mist of Time/Platform/StandaloneWindows")]
        public static void StandaloneWindows()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        }

        /// <summary>
        /// Switches the platform to StandaloneWindows64
        /// </summary>
        [MenuItem("Mist of Time/Platform/StandaloneWindows64")]
        public static void StandaloneWindows64()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        }

        /// <summary>
        /// Switches the platform to Android
        /// </summary>
        [MenuItem("Mist of Time/Platform/Android")]
        public static void Android()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }

        /// <summary>
        /// Switches the platform to WebGL
        /// </summary>
        [MenuItem("Mist of Time/Platform/WebGL")]
        public static void WebGL()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
        }
    }
}
