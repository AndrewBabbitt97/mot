using System.Collections.Generic;

namespace MOT.Common
{
    /// <summary>
    /// A Mist of Time patch comparison
    /// </summary>
    public class PatchComparison
    {
        /// <summary>
        /// The deleted files list
        /// </summary>
        public List<PatchInfoFile> DeletedFilesList { get; set; }

        /// <summary>
        /// The deleted directories list
        /// </summary>
        public List<PatchInfoDirectory> DeletedDirectoriesList { get; set; }

        /// <summary>
        /// The added directories list
        /// </summary>
        public List<PatchInfoDirectory> AddedDirectoriesList { get; set; }

        /// <summary>
        /// The added files list
        /// </summary>
        public List<PatchInfoFile> AddedFilesList { get; set; }

        /// <summary>
        /// The updated files list
        /// </summary>
        public List<PatchInfoFile> UpdatedFilesList { get; set; }

        /// <summary>
        /// Compares two patches to find the differences
        /// </summary>
        /// <param name="oldPatch">The old patch</param>
        /// <param name="newPatch">The new patch</param>
        public PatchComparison(PatchInfo oldPatch, PatchInfo newPatch)
        {
            DeletedFilesList = new List<PatchInfoFile>();

            foreach (PatchInfoFile oldFile in oldPatch.PatchFiles)
            {
                int index = newPatch.PatchFiles.FindIndex(item => item.Name == oldFile.Name);
                if (index < 0)
                {
                    DeletedFilesList.Add(oldFile);
                }
            }

            DeletedDirectoriesList = new List<PatchInfoDirectory>();

            foreach (PatchInfoDirectory oldDirectory in oldPatch.PatchDirectories)
            {
                int index = newPatch.PatchDirectories.FindIndex(item => item.Name == oldDirectory.Name);
                if (index < 0)
                {
                    DeletedDirectoriesList.Add(oldDirectory);
                }
            }

            AddedDirectoriesList = new List<PatchInfoDirectory>();

            foreach (PatchInfoDirectory addedDirectory in newPatch.PatchDirectories)
            {
                int index = oldPatch.PatchDirectories.FindIndex(item => item.Name == addedDirectory.Name);
                if (index < 0)
                {
                    AddedDirectoriesList.Add(addedDirectory);
                }
            }

            AddedFilesList = new List<PatchInfoFile>();

            foreach (PatchInfoFile addedFile in newPatch.PatchFiles)
            {
                int index = oldPatch.PatchFiles.FindIndex(item => item.Name == addedFile.Name);
                if (index < 0)
                {
                    AddedFilesList.Add(addedFile);
                }
            }

            UpdatedFilesList = new List<PatchInfoFile>();

            foreach (PatchInfoFile updatedFile in newPatch.PatchFiles)
            {
                int index = oldPatch.PatchFiles.FindIndex(item => item.Name == updatedFile.Name);
                if (index >= 0)
                {
                    if (updatedFile.Hash != oldPatch.PatchFiles[index].Hash)
                    {
                        UpdatedFilesList.Add(newPatch.PatchFiles[index]);
                    }
                }
            }
        }
    }
}
