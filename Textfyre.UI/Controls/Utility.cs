using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Resources;

namespace Textfyre.UI.Controls
{
    public class Utility
    {
        public static FileStream GetFileStream(string filename)
        {
            string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            string settingsFilePath = System.IO.Path.Combine(exeFolder, "GameFiles", filename);

            return File.OpenRead(settingsFilePath);
        }

    }
}
