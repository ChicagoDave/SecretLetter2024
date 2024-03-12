using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.IO.IsolatedStorage;

namespace Textfyre.UI.Entities
{
    public class SaveFile
    {
        #region :: Title ::
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        #endregion

        #region :: Description ::
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        #endregion

        #region :: SaveTime ::
        public DateTime _saveTime;
        public DateTime SaveTime
        {
            get { return _saveTime; }
            set { _saveTime = value; }
        }
        #endregion

        #region :: Filename ::
        private string _filename = string.Empty;
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }
        #endregion

        #region :: GameFileVersion ::
        private string _gameFileVersion = string.Empty;
        public string GameFileVersion
        {
            get { return _gameFileVersion; }
            set { _gameFileVersion = value; }
        }
        #endregion

        #region :: FyreXml ::
        private string _fyreXml = string.Empty;
        public string FyreXml
        {
            get { return _fyreXml; }
            set { _fyreXml = value; }
        }
        #endregion

        #region :: Transcript ::
        private string _transcript = string.Empty;
        public string Transcript
        {
            get { return _transcript; }
            set { _transcript = value; }
        }
        #endregion

        #region :: Chapter ::
        private string _chapter = string.Empty;
        public string Chapter
        {
            get { return _chapter; }
            set { _chapter = value; }
        }
        #endregion

        #region :: Theme ::
        private string _theme = string.Empty;
        public string Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }
        #endregion

        #region :: StoryTitle ::
        private string _storyTitle = string.Empty;
        public string StoryTitle
        {
            get { return _storyTitle; }
            set { _storyTitle = value; }
        }
        #endregion

        #region :: Hints ::
        private string _hints = string.Empty;
        public string Hints
        {
            get { return _hints; }
            set { _hints = value; }
        }
        #endregion

        public void Delete()
        {
            if (string.IsNullOrEmpty(_filename))
                return;

            IsolatedStorageSettings isoSettings = IsolatedStorageSettings.ApplicationSettings;

            if (isoSettings.Contains(_filename + ".fvt"))
                isoSettings.Remove(_filename + ".fvt");

            if (isoSettings.Contains(_filename + ".fvq"))
                isoSettings.Remove(_filename + ".fvq");

            //isoSettings.Save();
        }

        public string Save()
        {
            if (string.IsNullOrEmpty(_filename))
            {
                _filename = FindFilename();
            }

            string filepath = _filename;

            IsolatedStorageSettings isoSettings = IsolatedStorageSettings.ApplicationSettings;
            isoSettings[filepath + ".fvt"] = Serialize(this);
            //isoSettings.Save();

            return filepath + ".fvq";
        }

        private string FindFilename()
        {
            List<SaveFile> sfs = SaveFiles;

            foreach (SaveFile sf in sfs)
            {
                if (sf.Title == _title && !string.IsNullOrEmpty(sf.Filename))
                    return sf.Filename;
            }

            return Guid.NewGuid().ToString() + Current.Game.GameFileName;
        }

        public string BinaryStoryFilePath
        {
            get { return _filename + ".fvq"; }
        }

        public static int SaveFilesCount
        {
            get { return SaveFiles.Count; }
        }

        public static void DeleteOldSaveFiles()
        {
            string currentGameFileName = Current.Game.GameFileName;
            List<SaveFile> files = SaveFiles;
            foreach (SaveFile file in files)
            {
                if (file.GameFileVersion != currentGameFileName)
                {
                    file.Delete();
                }
            }
        }

        public static List<SaveFile> SaveFiles
        {
            get
            {
                List<SaveFile> saveFiles = new List<SaveFile>();
                IsolatedStorageSettings isoSettings = IsolatedStorageSettings.ApplicationSettings;

                foreach (var key in isoSettings.Keys)
                {
                    if (key.ToString().EndsWith(".fvt"))
                    {
                        string json = isoSettings[key.ToString()].ToString();
                        SaveFile sf = SaveFile.Deserialize(json);
                        saveFiles.Add(sf);
                    }
                }

                return saveFiles;
            }
        }

        public static string Serialize(SaveFile saveFile)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append(ToParam(saveFile.Filename) + "|");
            sb.Append(ToParam(saveFile.Title) + "|");
            sb.Append(ToParam(saveFile.Description) + "|");
            sb.Append(ToParam(saveFile.SaveTime.ToString()) + "|");
            sb.Append(ToParam(saveFile.GameFileVersion.ToString()) + "|");
            sb.Append(ToParam(saveFile.FyreXml) + "|");
            sb.Append(ToParam(saveFile.Transcript) + "|");
            sb.Append(ToParam(saveFile.StoryTitle) + "|");
            sb.Append(ToParam(saveFile.Chapter) + "|");
            sb.Append(ToParam(saveFile.Theme) + "|");
            sb.Append(ToParam(saveFile.Hints) + "");

            return sb.ToString();
        }

        public static SaveFile Deserialize(string json)
        {
            SaveFile saveFile = new SaveFile();
            string[] parts = json.Split('|');
            if (parts.Length >= 4)
            {
                saveFile.Filename = FromParam(parts[0]);
                saveFile.Title = FromParam(parts[1]);
                saveFile.Description = FromParam(parts[2]);
                try
                {
                    saveFile.SaveTime = DateTime.Parse(FromParam(parts[3]));
                }
                catch
                {
                    saveFile.SaveTime = DateTime.Now;
                }
            }

            if (parts.Length >= 5)
            {
                saveFile.GameFileVersion = FromParam(parts[4]);
            }

            if (parts.Length >= 6)
            {
                saveFile.FyreXml = FromParam(parts[5]);
            }

            if (parts.Length >= 7)
            {
                saveFile.Transcript = FromParam(parts[6]);
            }

            if (parts.Length >= 8)
            {
                saveFile.StoryTitle = FromParam(parts[7]);
            }

            if (parts.Length >= 9)
            {
                saveFile.Chapter = FromParam(parts[8]);
            }

            if (parts.Length >= 10)
            {
                saveFile.Theme = FromParam(parts[9]);
            }

            if (parts.Length >= 11)
            {
                saveFile.Hints = FromParam(parts[10]);
            }

            return saveFile;
        }

        private static string ToParam(string value)
        {
            return value.Replace("|", "&#124;");
        }

        private static string FromParam(string value)
        {
            return value.Replace("&#124;", "|");
        }

        public static long FreeSpace
        {
            get
            {
                // Not applicable in OpenSilver
                return 0;
            }
        }

        public static long TotalSpace
        {
            get
            {
                // Not applicable in OpenSilver
                return 0;
            }
        }

        public static void IncreaseStorageSpace()
        {
            // Not applicable in OpenSilver
            // OpenSilver uses browser storage, which has its own limits
        }
    }
}
