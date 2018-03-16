using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentOrganizerUI.Extensions
{
    public static class FileInfoExtensions
    {
        public static string NameWithoutExtension(this FileInfo fi)
        {
            return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
        }
    }
}
