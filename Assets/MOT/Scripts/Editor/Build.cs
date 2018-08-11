using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using MOT.Common;

namespace MOT.Editor
{
    /// <summary>
    /// The Mist of Time build system
    /// </summary>
    public static class Build
    {
        /// <summary>
        /// Builds the player for the current platform
        /// </summary>
        public static void BuildPlayer()
        {
            BuildTarget buildPlatform = EditorUserBuildSettings.activeBuildTarget;

            string[] buildScenes = new string[] { };
            string buildPath = Directory.GetCurrentDirectory().Replace('\\', '/') + "/obj/";
            BuildOptions buildSettings = BuildOptions.None;

            switch (buildPlatform)
            {
                case BuildTarget.StandaloneWindows:
                    buildScenes = new string[] { "Assets/MOT/Scenes/Startup/motboot.unity" };
                    buildPath += buildPlatform.ToString() + "/Player/motboot.exe";
                    buildSettings = BuildOptions.None;
                    break;
                case BuildTarget.StandaloneWindows64:
                    buildScenes = new string[] { "Assets/MOT/Scenes/Startup/motboot.unity" };
                    buildPath += buildPlatform.ToString() + "/Player/motboot.exe";
                    buildSettings = BuildOptions.None;
                    break;
                case BuildTarget.Android:
                    buildScenes = new string[] { "Assets/MOT/Scenes/Startup/motboot.unity" };
                    buildPath += buildPlatform.ToString() + "/Player/motboot.apk";
                    buildSettings = BuildOptions.None;
                    break;
                case BuildTarget.WebGL:
                    buildScenes = new string[] { "Assets/MOT/Scenes/Startup/motboot.unity" };
                    buildPath += buildPlatform.ToString() + "/Player/motboot";
                    buildSettings = BuildOptions.None;
                    break;
                default:
                    Debug.LogError("Mist of Time does not support " + buildPlatform.ToString() + " as a build target");
                    return;
            }

            if (Directory.Exists(Path.GetDirectoryName(buildPath)))
            {
                Directory.Delete(Path.GetDirectoryName(buildPath), true);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(buildPath));

            BuildPipeline.BuildPlayer(buildScenes, buildPath, buildPlatform, buildSettings);

            if (buildPlatform == BuildTarget.StandaloneWindows ||
                buildPlatform == BuildTarget.StandaloneWindows64)
            {
                Directory.Move(Path.GetDirectoryName(buildPath) + "/motboot_Data", Path.GetDirectoryName(buildPath) + "/data");
            }
        }

        /// <summary>
        /// Builds the asset bundles for the current platform
        /// </summary>
        public static void BuildAssetBundles()
        {
            BuildTarget buildPlatform = EditorUserBuildSettings.activeBuildTarget;

            string buildPath = Directory.GetCurrentDirectory().Replace('\\', '/') + "/obj/";
            BuildAssetBundleOptions buildSettings = BuildAssetBundleOptions.None;

            switch (buildPlatform)
            {
                case BuildTarget.StandaloneWindows:
                    buildPath += buildPlatform.ToString() + "/AssetBundles/";
                    buildSettings = BuildAssetBundleOptions.None;
                    break;
                case BuildTarget.StandaloneWindows64:
                    buildPath += buildPlatform.ToString() + "/AssetBundles/";
                    buildSettings = BuildAssetBundleOptions.None;
                    break;
                case BuildTarget.Android:
                    buildPath += buildPlatform.ToString() + "/AssetBundles/";
                    buildSettings = BuildAssetBundleOptions.None;
                    break;
                case BuildTarget.WebGL:
                    buildPath += buildPlatform.ToString() + "/AssetBundles/";
                    buildSettings = BuildAssetBundleOptions.UncompressedAssetBundle;
                    break;
                default:
                    Debug.LogError("Mist of Time does not support " + buildPlatform.ToString() + " as a build target");
                    return;
            }

            if (!Directory.Exists(Path.GetDirectoryName(buildPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(buildPath));
            }

            BuildPipeline.BuildAssetBundles(buildPath, buildSettings, buildPlatform);
        }

        /// <summary>
        /// Builds the release for the current platform
        /// </summary>
        public static void BuildRelease()
        {
            BuildTarget buildPlatform = EditorUserBuildSettings.activeBuildTarget;

            string releasePath = Directory.GetCurrentDirectory().Replace('\\', '/') + "/obj/";
            string buildPath = Directory.GetCurrentDirectory().Replace('\\', '/') + "/bin/";

            switch (buildPlatform)
            {
                case BuildTarget.StandaloneWindows:
                    releasePath += buildPlatform.ToString() + "/Installer/Installer.exe";
                    buildPath += buildPlatform.ToString() + "/Release/mistoftime_" + buildPlatform.ToString().ToLower() + ".exe";
                    break;
                case BuildTarget.StandaloneWindows64:
                    releasePath += buildPlatform.ToString() + "/Installer/Installer.exe";
                    buildPath += buildPlatform.ToString() + "/Release/mistoftime_" + buildPlatform.ToString().ToLower() + ".exe";
                    break;
                case BuildTarget.Android:
                    releasePath += buildPlatform.ToString() + "/Player/motboot.apk";
                    buildPath += buildPlatform.ToString() + "/Release/mistoftime_" + buildPlatform.ToString().ToLower() + ".apk";
                    break;
                case BuildTarget.WebGL:
                    releasePath += buildPlatform.ToString() + "/Launcher/Launcher.html";
                    buildPath += buildPlatform.ToString() + "/Release/mistoftime_" + buildPlatform.ToString().ToLower() + ".html";
                    break;
                default:
                    Debug.LogError("Mist of Time does not support " + buildPlatform.ToString() + " as a build target");
                    return;
            }

            if (Directory.Exists(Path.GetDirectoryName(buildPath)))
            {
                Directory.Delete(Path.GetDirectoryName(buildPath), true);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(buildPath));

            if (buildPlatform == BuildTarget.StandaloneWindows ||
                buildPlatform == BuildTarget.StandaloneWindows64 ||
                buildPlatform == BuildTarget.Android)
            {
                BuildPlayer();
            }

            if (buildPlatform == BuildTarget.StandaloneWindows ||
                buildPlatform == BuildTarget.StandaloneWindows64)
            {
                System.Diagnostics.Process installerCMD = new System.Diagnostics.Process();
                installerCMD.StartInfo.FileName = Directory.GetCurrentDirectory().Replace("\\", "/") + "/Assets/PlayerAssets/Standalone/" + buildPlatform.ToString() + "/Installer/build.bat";
                installerCMD.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory().Replace("\\", "/") + "/Assets/PlayerAssets/Standalone/" + buildPlatform.ToString() + "/Installer";
                installerCMD.StartInfo.UseShellExecute = false;
                installerCMD.StartInfo.CreateNoWindow = true;
                installerCMD.Start();
                EditorUtility.DisplayProgressBar("Building Installer", "Building Installer.exe", 0.25f);
                installerCMD.WaitForExit();
                EditorUtility.ClearProgressBar();

                if (!File.Exists(releasePath))
                {
                    Debug.LogError("Installer failed to build!");
                    return;
                }
            }

            if (buildPlatform == BuildTarget.WebGL)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(releasePath));
                File.WriteAllText(releasePath, "<meta http-equiv=\"refresh\" content=\"0; url=../Patch/motboot/index.html\" />");
            }

            File.Copy(releasePath, buildPath);
        }

        /// <summary>
        /// Builds a patch for the current platform
        /// </summary>
        public static void BuildPatch()
        {
            BuildTarget buildPlatform = EditorUserBuildSettings.activeBuildTarget;

            string playerPath = Directory.GetCurrentDirectory().Replace('\\', '/') + "/obj/";
            string assetBundlesPath = Directory.GetCurrentDirectory().Replace('\\', '/') + "/obj/";
            string buildPath = Directory.GetCurrentDirectory().Replace('\\', '/') + "/bin/";

            switch (buildPlatform)
            {
                case BuildTarget.StandaloneWindows:
                    playerPath += buildPlatform.ToString() + "/Player/";
                    assetBundlesPath += buildPlatform.ToString() + "/AssetBundles/";
                    buildPath += buildPlatform.ToString() + "/Patch/";
                    break;
                case BuildTarget.StandaloneWindows64:
                    playerPath += buildPlatform.ToString() + "/Player/";
                    assetBundlesPath += buildPlatform.ToString() + "/AssetBundles/";
                    buildPath += buildPlatform.ToString() + "/Patch/";
                    break;
                case BuildTarget.Android:
                    playerPath += buildPlatform.ToString() + "/Player/";
                    assetBundlesPath += buildPlatform.ToString() + "/AssetBundles/";
                    buildPath += buildPlatform.ToString() + "/Patch/";
                    break;
                case BuildTarget.WebGL:
                    playerPath += buildPlatform.ToString() + "/Player/";
                    assetBundlesPath += buildPlatform.ToString() + "/AssetBundles/";
                    buildPath += buildPlatform.ToString() + "/Patch/";
                    break;
                default:
                    Debug.LogError("Mist of Time does not support " + buildPlatform.ToString() + " as a build target");
                    return;
            }

            if (Directory.Exists(Path.GetDirectoryName(buildPath)))
            {
                Directory.Delete(Path.GetDirectoryName(buildPath), true);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(buildPath));

            BuildPlayer();
            BuildAssetBundles();

            CopyDirectory(playerPath, buildPath, new string[] { });
            CopyDirectory(assetBundlesPath, buildPath, new string[] { ".manifest" });

            File.Move(buildPath + "AssetBundles", buildPath + "FTABLE.DAT");

            PatchInfo newPatchInfo = new PatchInfo();

            string[] buildDirectories = Directory.GetDirectories(buildPath, "*", SearchOption.AllDirectories);

            for (int i = 0; i < buildDirectories.Length; i++)
            {
                buildDirectories[i] = buildDirectories[i].Replace('\\', '/').Replace(buildPath, "");
                EditorUtility.DisplayProgressBar("Generating Patch Info", "Processing " + buildDirectories[i], ((float)i / buildDirectories.Length));
                newPatchInfo.PatchDirectories.Add(new PatchInfoDirectory(buildDirectories[i]));
            }

            EditorUtility.ClearProgressBar();

            string[] buildFiles = Directory.GetFiles(buildPath, "*", SearchOption.AllDirectories);

            for (int i = 0; i < buildFiles.Length; i++)
            {
                buildFiles[i] = buildFiles[i].Replace('\\', '/').Replace(buildPath, "");
                EditorUtility.DisplayProgressBar("Generating Patch Info", "Processing " + buildFiles[i], ((float)i / buildFiles.Length));

                using (FileStream stream = File.OpenRead(buildPath + buildFiles[i]))
                {
                    SHA256Managed sha = new SHA256Managed();
                    byte[] checksum = sha.ComputeHash(stream);
                    newPatchInfo.PatchFiles.Add(new PatchInfoFile(buildFiles[i], stream.Length, BitConverter.ToString(checksum).Replace("-", string.Empty)));
                }
            }

            EditorUtility.ClearProgressBar();

            newPatchInfo.Serialize(buildPath + "patch.txt");

            VersionInfo currentVersionInfo = (VersionInfo)AssetDatabase.LoadAssetAtPath("Assets/MOT/VersionInfo.asset", typeof(VersionInfo));

            File.WriteAllText(buildPath + "patch.ver", currentVersionInfo.PatchVersion);
        }

        /// <summary>
        /// Copies a directory
        /// </summary>
        /// <param name="source">The source directory</param>
        /// <param name="destination">The destination directory</param>
        /// <param name="fileExcludeList">The list of files to exclude from copying</param>
        public static void CopyDirectory(string source, string destination, string[] fileExcludeList)
        {
            string[] directories = Directory.GetDirectories(source, "*", SearchOption.AllDirectories);

            for (int i = 0; i < directories.Length; i++)
            {
                directories[i] = directories[i].Replace('\\', '/').Replace(source, "");
                EditorUtility.DisplayProgressBar("Creating Directories", "Copying " + directories[i], ((float)i / directories.Length));
                Directory.CreateDirectory(destination + directories[i]);
            }

            EditorUtility.ClearProgressBar();

            string[] files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace('\\', '/').Replace(source, "");
                EditorUtility.DisplayProgressBar("Copying Files", "Copying " + files[i], ((float)i / files.Length));

                if (fileExcludeList.Any(files[i].EndsWith))
                {
                    continue;
                }

                File.Copy(source + files[i], destination + files[i]);
            }

            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// Builds everything for the current platform
        /// </summary>
        [MenuItem("Mist of Time/Build/All [Current Platform]")]
        public static void BuildAllCurrentPlatform()
        {
            BuildRelease();
            BuildPatch();
        }

        /// <summary>
        /// Builds everything for all supported platforms
        /// </summary>
        [MenuItem("Mist of Time/Build/All [All Platforms]")]
        public static void BuildAllAllPlatforms()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
            BuildRelease();
            BuildPatch();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
            BuildRelease();
            BuildPatch();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            BuildRelease();
            BuildPatch();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            BuildRelease();
            BuildPatch();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        }

        /// <summary>
        /// Builds a release for the current platform
        /// </summary>
        [MenuItem("Mist of Time/Build/Release [Current Platform]")]
        public static void BuildReleaseCurrentPlatform()
        {
            BuildRelease();
        }

        /// <summary>
        /// Builds a release for all supported platforms
        /// </summary>
        [MenuItem("Mist of Time/Build/Release [All Platforms]")]
        public static void BuildReleaseAllPlatforms()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
            BuildRelease();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
            BuildRelease();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            BuildRelease();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            BuildRelease();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        }

        /// <summary>
        /// Builds a patch for the current platform
        /// </summary>
        [MenuItem("Mist of Time/Build/Patch [Current Platform]")]
        public static void BuildPatchCurrentPlatform()
        {
            BuildPatch();
        }

        /// <summary>
        /// Builds a patch for all supported platforms
        /// </summary>
        [MenuItem("Mist of Time/Build/Patch [All Platforms]")]
        public static void BuildPatchAllPlatforms()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
            BuildPatch();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
            BuildPatch();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            BuildPatch();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            BuildPatch();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        }
    }
}
