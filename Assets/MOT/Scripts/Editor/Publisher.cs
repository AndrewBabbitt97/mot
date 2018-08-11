using UnityEditor;
using System.IO;
using System.Diagnostics;

namespace MOT.Editor
{
    /// <summary>
    /// A wraper for the publisher utility
    /// </summary>
    public static class Publisher
    {
        /// <summary>
        /// Runs the publisher
        /// </summary>
        /// <param name="arguments">The arguments to run the publisher with</param>
        /// <returns>The exit code</returns>
        private static int RunPublisher(string arguments)
        {
            if (!File.Exists(Directory.GetCurrentDirectory().Replace("\\", "/") + "/../utilities/publisher/Publisher/bin/Release/Publisher.exe"))
            {
                Process publisherBuild = new Process();
                publisherBuild.StartInfo.FileName = Directory.GetCurrentDirectory().Replace("\\", "/") + "/../utilities/publisher/build.bat";
                publisherBuild.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory().Replace("\\", "/") + "/../utilities/publisher";
                publisherBuild.StartInfo.UseShellExecute = false;
                publisherBuild.StartInfo.CreateNoWindow = true;
                publisherBuild.Start();
                EditorUtility.DisplayProgressBar("Building Publisher", "Building Publisher.exe", 0.25f);
                publisherBuild.WaitForExit();
                EditorUtility.ClearProgressBar();
            }

            if (!File.Exists(Directory.GetCurrentDirectory().Replace("\\", "/") + "/../utilities/publisher/Publisher/bin/Release/Publisher.exe"))
            {
                UnityEngine.Debug.LogError("Publisher failed to build!");
                return 1;
            }

            Process publisherEXE = new Process();
            publisherEXE.StartInfo.FileName = Directory.GetCurrentDirectory().Replace("\\", "/") + "/../utilities/publisher/Publisher/bin/Release/Publisher.exe";
            publisherEXE.StartInfo.Arguments = arguments;
            publisherEXE.Start();
            publisherEXE.WaitForExit();
            return publisherEXE.ExitCode;
        }

        /// <summary>
        /// Configures FTP
        /// </summary>
        /// <returns>The exit code</returns>
        public static int FTPConfig()
        {
            return RunPublisher("ftp-config");
        }

        /// <summary>
        /// Adds a file to FTP
        /// </summary>
        /// <param name="localPath">The local path to the file</param>
        /// <param name="serverPath">The server path to save the file to</param>
        /// <returns>The exit code</returns>
        public static int FTPAddFile(string localPath, string serverPath)
        {
            return RunPublisher("ftp-addfile -l \"" + localPath + "\" -s \"" + serverPath + "\"");
        }

        /// <summary>
        /// Deletes a file from FTP
        /// </summary>
        /// <param name="serverPath">The server path to the file to delete</param>
        /// <returns>The exit code</returns>
        public static int FTPDeleteFile(string serverPath)
        {
            return RunPublisher("ftp-deletefile -s \"" + serverPath + "\"");
        }

        /// <summary>
        /// Gets a file from FTP
        /// </summary>
        /// <param name="serverPath">The server path to the file</param>
        /// <param name="localPath">The local path to save the file to</param>
        /// <returns>The exit code</returns>
        public static int FTPGetFile(string serverPath, string localPath)
        {
            return RunPublisher("ftp-getfile -s \"" + serverPath + "\" -l \"" + localPath + "\"");
        }

        /// <summary>
        /// Adds a directory to FTP
        /// </summary>
        /// <param name="serverPath">The server path to the directory to create</param>
        /// <returns>The exit code</returns>
        public static int FTPAddDirectory(string serverPath)
        {
            return RunPublisher("ftp-adddirectory -s \"" + serverPath + "\"");
        }

        /// <summary>
        /// Deletes a directory from FTP
        /// </summary>
        /// <param name="serverPath">The server path to the directory to delete</param>
        /// <returns>The exit code</returns>
        public static int FTPDeleteDirectory(string serverPath)
        {
            return RunPublisher("ftp-deletedirectory -s \"" + serverPath + "\"");
        }
    }
}
