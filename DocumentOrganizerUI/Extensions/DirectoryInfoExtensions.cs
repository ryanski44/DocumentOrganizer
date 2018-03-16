using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentOrganizerUI.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static IEnumerable<FileInfo> GetAllFiles(this DirectoryInfo di)
        {
            DirectoryInfo[] subDirectories = null;
            try
            {
                subDirectories = di.GetDirectories();
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.WriteLine(e.Message);
            }
            if (subDirectories != null)
            {
                foreach (DirectoryInfo subDir in subDirectories)
                {
                    foreach (var file in subDir.GetAllFiles())
                    {
                        yield return file;
                    }
                }
            }

            FileInfo[] files = null;
            try
            {
                files = di.GetFiles();
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.WriteLine(e.Message);
            }
            if (files != null)
            {
                foreach (var file in files)
                {
                    yield return file;
                }
            }
        }
    }
}
