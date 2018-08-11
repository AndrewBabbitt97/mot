using UnityEditor;
using UnityEngine;
using System.IO;
using MOT.Common;

namespace MOT.Editor
{
    /// <summary>
    /// The Mist of Time publish system
    /// </summary>
    public static class Publish
    {
        /// <summary>
        /// Publishes a release to FTP
        /// </summary>
        public static void PublishReleaseFTP()
        {
            BuildTarget publishPlatform = EditorUserBuildSettings.activeBuildTarget;

            string releasePath = "";
            string uploadPath = "";

            switch (publishPlatform)
            {
                case BuildTarget.StandaloneWindows:
                    releasePath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/bin/" + publishPlatform.ToString() + "/Release/mistoftime_" + publishPlatform.ToString().ToLower() + ".exe";
                    uploadPath = publishPlatform.ToString() + "/Release/mistoftime_" + publishPlatform.ToString().ToLower() + ".exe";
                    break;
                case BuildTarget.StandaloneWindows64:
                    releasePath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/bin/" + publishPlatform.ToString() + "/Release/mistoftime_" + publishPlatform.ToString().ToLower() + ".exe";
                    uploadPath = publishPlatform.ToString() + "/Release/mistoftime_" + publishPlatform.ToString().ToLower() + ".exe";
                    break;
                case BuildTarget.Android:
                    releasePath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/bin/" + publishPlatform.ToString() + "/Release/mistoftime_" + publishPlatform.ToString().ToLower() + ".apk";
                    uploadPath = publishPlatform.ToString() + "/Release/mistoftime_" + publishPlatform.ToString().ToLower() + ".apk";
                    break;
                case BuildTarget.WebGL:
                    releasePath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/bin/" + publishPlatform.ToString() + "/Release/mistoftime_" + publishPlatform.ToString().ToLower() + ".html";
                    uploadPath = publishPlatform.ToString() + "/Release/mistoftime_" + publishPlatform.ToString().ToLower() + ".html";
                    break;
                default:
                    Debug.LogError("Mist of Time does not dupport " + publishPlatform.ToString() + " as a publish target");
                    return;
            }

            if (!File.Exists(releasePath))
            {
                Debug.LogError("The release could not be found, please build a release");
                return;
            }

            EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Publishing " + Path.GetFileName(releasePath) + "...", 0.5f);

            int exitCode;

            exitCode = Publisher.FTPAddDirectory(publishPlatform.ToString() + "/Release");

            if (exitCode == 1)
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            exitCode = Publisher.FTPAddFile(releasePath, uploadPath);

            if (exitCode == 1)
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// Publishes a patch to FTP
        /// </summary>
        public static void PublishPatchFTP()
        {
            BuildTarget publishPlatform = EditorUserBuildSettings.activeBuildTarget;

            string publisherPath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/obj/";
            string patchPath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/bin/";
            string uploadPath = "";

            switch (publishPlatform)
            {
                case BuildTarget.StandaloneWindows:
                    publisherPath = publisherPath + publishPlatform.ToString() + "/Publisher/Patch/";
                    patchPath = patchPath + publishPlatform.ToString() + "/Patch/";
                    uploadPath = publishPlatform.ToString() + "/Patch/";
                    break;
                case BuildTarget.StandaloneWindows64:
                    publisherPath = publisherPath + publishPlatform.ToString() + "/Publisher/Patch/";
                    patchPath = patchPath + publishPlatform.ToString() + "/Patch/";
                    uploadPath = publishPlatform.ToString() + "/Patch/";
                    break;
                case BuildTarget.Android:
                    publisherPath = publisherPath + publishPlatform.ToString() + "/Publisher/Patch/";
                    patchPath = patchPath + publishPlatform.ToString() + "/Patch/";
                    uploadPath = publishPlatform.ToString() + "/Patch/";
                    
                    break;
                case BuildTarget.WebGL:
                    publisherPath = publisherPath + publishPlatform.ToString() + "/Publisher/Patch/";
                    patchPath = patchPath + publishPlatform.ToString() + "/Patch/";
                    uploadPath = publishPlatform.ToString() + "/Patch/";
                    break;
                default:
                    Debug.LogError("Mist of Time does not support " + publishPlatform.ToString() + " as a publish target");
                    return;
            }

            if (!Directory.Exists(publisherPath))
            {
                Directory.CreateDirectory(publisherPath);
            }

            if (!File.Exists(patchPath + "patch.txt"))
            {
                Debug.LogError("The patch could not be found, please build a patch");
                return;
            }

            int exitCode;

            exitCode = Publisher.FTPAddDirectory(publishPlatform.ToString() + "/Patch");

            if (exitCode == 1)
            {
                return;
            }

            EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Downloading old patch info", 0.5f);

            if (File.Exists(publisherPath + "patch.txt"))
            {
                File.Delete(publisherPath + "patch.txt");
            }

            exitCode = Publisher.FTPGetFile(uploadPath + "patch.txt", publisherPath + "patch.txt");

            if (exitCode == 1)
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            EditorUtility.ClearProgressBar();

            EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Processing old patch info", 0.5f);

            PatchInfo oldPatchInfo = new PatchInfo();

            if (File.Exists(publisherPath + "patch.txt"))
            {
                oldPatchInfo = PatchInfo.Deserialize(publisherPath + "patch.txt");
            }

            EditorUtility.ClearProgressBar();

            EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Processing new patch info", 0.5f);

            PatchInfo newPatchInfo = PatchInfo.Deserialize(patchPath + "patch.txt");

            EditorUtility.ClearProgressBar();

            EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Comparing patch info", 0.5f);

            PatchComparison patchChanges = new PatchComparison(oldPatchInfo, newPatchInfo);

            EditorUtility.ClearProgressBar();

            int progressBarProgress = 0;

            foreach (PatchInfoFile deletedFile in patchChanges.DeletedFilesList)
            {
                EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Deleting file " + deletedFile.Name, ((float)progressBarProgress / patchChanges.DeletedFilesList.Count));
                progressBarProgress++;
                exitCode = Publisher.FTPDeleteFile(uploadPath + deletedFile.Name);

                if (exitCode == 1)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
            }

            progressBarProgress = 0;
            EditorUtility.ClearProgressBar();

            foreach (PatchInfoDirectory deletedDirectory in patchChanges.DeletedDirectoriesList)
            {
                EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Deleting directory " + deletedDirectory.Name, ((float)progressBarProgress / patchChanges.DeletedDirectoriesList.Count));
                progressBarProgress++;
                exitCode = Publisher.FTPDeleteDirectory(uploadPath + deletedDirectory.Name);

                if (exitCode == 1)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
            }

            progressBarProgress = 0;
            EditorUtility.ClearProgressBar();

            foreach (PatchInfoDirectory addedDirectory in patchChanges.AddedDirectoriesList)
            {
                EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Adding directory " + addedDirectory.Name, ((float)progressBarProgress / patchChanges.AddedDirectoriesList.Count));
                progressBarProgress++;
                exitCode = Publisher.FTPAddDirectory(uploadPath + addedDirectory.Name);

                if (exitCode == 1)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
            }

            progressBarProgress = 0;
            EditorUtility.ClearProgressBar();

            foreach (PatchInfoFile addedFile in patchChanges.AddedFilesList)
            {
                EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Adding file " + addedFile.Name, ((float)progressBarProgress / patchChanges.AddedFilesList.Count));
                progressBarProgress++;
                exitCode = Publisher.FTPAddFile(patchPath + addedFile.Name, uploadPath + addedFile.Name);

                if (exitCode == 1)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }
            }

            progressBarProgress = 0;
            EditorUtility.ClearProgressBar();

            foreach (PatchInfoFile updatedFile in patchChanges.UpdatedFilesList)
            {
                EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Updating file " + updatedFile.Name, ((float)progressBarProgress / patchChanges.UpdatedFilesList.Count));
                progressBarProgress++;
                exitCode = Publisher.FTPDeleteFile(uploadPath + updatedFile.Name);

                if (exitCode == 1)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }

                exitCode = Publisher.FTPAddFile(patchPath + updatedFile.Name, uploadPath + updatedFile.Name);

                if (exitCode == 1)
                { 
                    EditorUtility.ClearProgressBar();
                    return;
                }
            }

            progressBarProgress = 0;
            EditorUtility.ClearProgressBar();

            EditorUtility.DisplayProgressBar("Mist of Time Publisher", "Publishing patch info", 0.5f);

            exitCode = Publisher.FTPDeleteFile(uploadPath + "patch.txt");

            if (exitCode == 1)
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            exitCode = Publisher.FTPDeleteFile(uploadPath + "patch.ver");

            if (exitCode == 1)
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            exitCode = Publisher.FTPAddFile(patchPath + "patch.txt", uploadPath + "patch.txt");

            if (exitCode == 1)
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            exitCode = Publisher.FTPAddFile(patchPath + "patch.ver", uploadPath + "patch.ver");

            if (exitCode == 1)
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// Configures FTP
        /// </summary>
        [MenuItem("Mist of Time/Publish/Configure - FTP")]
        public static void PublishConfigureFTP()
        {
            int exitCode = Publisher.FTPConfig();

            if (exitCode == 1)
            {
                return;
            }
        }

        /// <summary>
        /// Publishes everything for the current platform
        /// </summary>
        [MenuItem("Mist of Time/Publish/All [Current Platform]")]
        public static void PublishAllCurrentPlatform()
        {
            PublishReleaseFTP();
            PublishPatchFTP();
        }

        /// <summary>
        /// Publishes everything for all supported platforms
        /// </summary>
        [MenuItem("Mist of Time/Publish/All [All Platforms]")]
        public static void PublishAllAllPlatforms()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
            PublishReleaseFTP();
            PublishPatchFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
            PublishReleaseFTP();
            PublishPatchFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PublishReleaseFTP();
            PublishPatchFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            PublishReleaseFTP();
            PublishPatchFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        }

        /// <summary>
        /// Publishes a release to FTP for the current platform
        /// </summary>
        [MenuItem("Mist of Time/Publish/Release - FTP [Current Platform]")]
        public static void PublishReleaseFTPCurrentPlatform()
        {
            PublishReleaseFTP();
        }

        /// <summary>
        /// Publishes a release to FTP for all supported platforms
        /// </summary>
        [MenuItem("Mist of Time/Publish/Release - FTP [All Platforms]")]
        public static void PublishReleaseFTPAllPlatforms()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
            PublishReleaseFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
            PublishReleaseFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PublishReleaseFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            PublishReleaseFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        }

        /// <summary>
        /// Publishes a patch to FTP for the current platform
        /// </summary>
        [MenuItem("Mist of Time/Publish/Patch - FTP [Current Platform]")]
        public static void PublishPatchFTPCurrentPlatform()
        {
            PublishPatchFTP();
        }

        /// <summary>
        /// Publishes a patch to FTP for all supported platforms
        /// </summary>
        [MenuItem("Mist of Time/Publish/Patch - FTP [All Platforms]")]
        public static void PublishPatchFTPAllPlatforms()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
            PublishPatchFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
            PublishPatchFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PublishPatchFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            PublishPatchFTP();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        }
    }
}
