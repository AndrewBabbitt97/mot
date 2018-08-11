using System.IO;
using System.Collections.Generic;

namespace MOT.Common
{
    /// <summary>
    /// Mist of Time patch info
    /// </summary>
    public class PatchInfo
    {
        /// <summary>
        /// The directories in the patch info
        /// </summary>
        public List<PatchInfoDirectory> PatchDirectories { get; set; }

        /// <summary>
        /// The files in the patch info
        /// </summary>
        public List<PatchInfoFile> PatchFiles { get; set; }

        /// <summary>
        /// Creates patch info
        /// </summary>
        public PatchInfo()
        {
            PatchDirectories = new List<PatchInfoDirectory>();
            PatchFiles = new List<PatchInfoFile>();
        }

        /// <summary>
        /// Serializes the patch info
        /// </summary>
        /// <param name="path">The path to the file</param>
        public void Serialize(string path)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                foreach (PatchInfoDirectory directory in PatchDirectories)
                {
                    writer.WriteLine("directory:" + directory.Name);
                }

                foreach (PatchInfoFile file in PatchFiles)
                {
                    writer.WriteLine("file:" + file.Hash + ":" + file.Size + ":" + file.Name);
                }
            }
        }

        /// <summary>
        /// Deserializes the patch info
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>The deserialized object</returns>
        public static PatchInfo Deserialize(string path)
        {
            PatchInfo newPatchInfo = new PatchInfo();

            string line;
            string[] lineParts;

            using (TextReader reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lineParts = line.Split(':');

                    if (lineParts[0] == "directory")
                    {
                        newPatchInfo.PatchDirectories.Add(new PatchInfoDirectory(lineParts[1]));
                    }
                    else if (lineParts[0] == "file")
                    {
                        newPatchInfo.PatchFiles.Add(new PatchInfoFile(lineParts[3], long.Parse(lineParts[2]), lineParts[1]));
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return newPatchInfo;
        }
    }

    /// <summary>
    /// A patch info directory
    /// </summary>
    public class PatchInfoDirectory
    {
        /// <summary>
        /// Creates a patch info directory
        /// </summary>
        public PatchInfoDirectory()
        {
            Name = "";
        }

        /// <summary>
        /// Creates a patch info directory
        /// </summary>
        /// <param name="name">The patch info directory name</param>
        public PatchInfoDirectory(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The patch info directory name
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// A patch info file
    /// </summary>
    public class PatchInfoFile
    {
        /// <summary>
        /// Creates a patch info file
        /// </summary>
        public PatchInfoFile()
        {
            Name = "";
            Size = 0;
            Hash = "";
        }

        /// <summary>
        /// Creates a patch info file
        /// </summary>
        /// <param name="name">The patch info file name</param>
        /// <param name="size">The patch info file size</param>
        /// <param name="hash">The patch info file hash</param>
        public PatchInfoFile(string name, long size, string hash)
        {
            Name = name;
            Size = size;
            Hash = hash;
        }

        /// <summary>
        /// The patch info file name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The patch info file size
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// The patch info file hash
        /// </summary>
        public string Hash { get; set; }
    }
}
