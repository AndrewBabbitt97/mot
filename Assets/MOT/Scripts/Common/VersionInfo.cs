using UnityEngine;
using System;

namespace MOT.Common
{
    /// <summary>
    /// The Mist of Time version info
    /// </summary>
    [Serializable]
    public class VersionInfo : ScriptableObject
    {
        /// <summary>
        /// The patch version - year
        /// </summary>
        [Header("Patch Version")]
        [SerializeField]
        int _patchVersionYear;

        /// <summary>
        /// The patch version - month
        /// </summary>
        [SerializeField]
        int _patchVersionMonth;

        /// <summary>
        /// The patch version - day
        /// </summary>
        [SerializeField]
        int _patchVersionDay;

        /// <summary>
        /// The patch version - revision
        /// </summary>
        [SerializeField]
        int _patchVersionRevision;

        /// <summary>
        /// The game version - major
        /// </summary>
        [Header("Game Version")]
        [SerializeField]
        int _gameVersionMajor;

        /// <summary>
        /// The game version - minor
        /// </summary>
        [SerializeField]
        int _gameVersionMinor;

        /// <summary>
        /// The game version - build
        /// </summary>
        [SerializeField]
        int _gameVersionBuild;

        /// <summary>
        /// The game version - revision
        /// </summary>
        [SerializeField]
        int _gameVersionRevision;

        /// <summary>
        /// The game version - extension
        /// </summary>
        [SerializeField]
        string _gameVersionExtension;

        /// <summary>
        /// Creates version info
        /// </summary>
        public VersionInfo()
        {
            _patchVersionYear = 0;
            _patchVersionMonth = 0;
            _patchVersionDay = 0;
            _patchVersionRevision = 0;
            _gameVersionMajor = 0;
            _gameVersionMinor = 0;
            _gameVersionBuild = 0;
            _gameVersionRevision = 0;
            _gameVersionExtension = "";
        }

        /// <summary>
        /// Gets the patch version
        /// </summary>
        public string PatchVersion
        {
            get
            {
                return string.Format("{0}{1}{2}_{3}", _patchVersionYear.ToString(), _patchVersionMonth.ToString("D2"), _patchVersionDay.ToString("D2"), _patchVersionRevision.ToString("D4"));
            }
        }

        /// <summary>
        /// Gets the game version
        /// </summary>
        public string GameVersion
        {
            get
            {
                if (_gameVersionRevision == 0)
                {
                    if (_gameVersionBuild == 0)
                    {
                        return string.Format("{0}.{1}{2}", _gameVersionMajor, _gameVersionMinor, _gameVersionExtension);
                    }
                    else
                    {
                        return string.Format("{0}.{1}.{2}{3}", _gameVersionMajor, _gameVersionMinor, _gameVersionBuild, _gameVersionExtension);
                    }
                }
                else
                {
                    return string.Format("{0}.{1}.{2}.{3}{4}", _gameVersionMajor, _gameVersionMinor, _gameVersionBuild, _gameVersionRevision, _gameVersionExtension);
                }
            }
        }
    }
}
