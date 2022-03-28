using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentOrganizerUI
{
    public class Document
    {
        public DirectoryInfo ProcessedDocumentDirectory { get; set; }
        public FileInfo ProcessedFile { get; set; }
        public FileInfo OriginalFile { get; set; }
        public FileInfo Preview50File { get { return new FileInfo(Path.Combine(previewsDirectory.FullName, "50.jpg")); } }
        public FileInfo Preview300File { get { return new FileInfo(Path.Combine(previewsDirectory.FullName, "300.jpg")); } }
        public FileInfo PreviewTextFile { get { return new FileInfo(Path.Combine(previewsDirectory.FullName, "text.txt")); } }

        private DirectoryInfo originalDirectory;
        private DirectoryInfo previewsDirectory;
        public static Document FromDirectory(DirectoryInfo processedDocumentDirectory)
        {
            Document doc = new Document(processedDocumentDirectory);
            var files = processedDocumentDirectory.GetFiles();
            if(files.Any())
            {
                doc.ProcessedFile = files.First();
                DirectoryInfo originalDirectory = new DirectoryInfo(Path.Combine(processedDocumentDirectory.FullName, "original"));
                if(originalDirectory.Exists)
                {
                    var origFiles = originalDirectory.GetFiles();
                    if(origFiles.Any())
                    {
                        doc.OriginalFile = origFiles.First();
                        return doc;
                    }
                    else
                    {
                        throw new Exception($"Could not find any original files in {originalDirectory.FullName}");
                    }
                }
                else
                {
                    throw new Exception($"Could not find original file directory {originalDirectory.FullName}");
                }
            }
            else
            {
                throw new Exception($"Could not find any files in {processedDocumentDirectory.FullName}");
            }
        }

        public static Document CreateNewFromRootDirectory(DirectoryInfo rootProcessingDirectory, string originalFileName)
        {
            var doc = new Document(new DirectoryInfo(Path.Combine(rootProcessingDirectory.FullName, Guid.NewGuid().ToString())));
            doc.ProcessedDocumentDirectory.Create();
            doc.ProcessedFile = new FileInfo(Path.Combine(doc.ProcessedDocumentDirectory.FullName, Path.GetFileNameWithoutExtension(originalFileName) + ".pdf"));
            doc.OriginalFile = new FileInfo(Path.Combine(doc.originalDirectory.FullName, originalFileName));
            return doc;
        }

        private Document(DirectoryInfo processedDocumentDirectory)
        {
            ProcessedDocumentDirectory = processedDocumentDirectory;
            if(!ProcessedDocumentDirectory.Exists)
            {
                ProcessedDocumentDirectory.Create();
            }
            originalDirectory = new DirectoryInfo(Path.Combine(ProcessedDocumentDirectory.FullName, "original"));
            if (!originalDirectory.Exists)
            {
                originalDirectory.Create();
            }
            previewsDirectory = new DirectoryInfo(Path.Combine(ProcessedDocumentDirectory.FullName, "previews"));
            if (!previewsDirectory.Exists)
            {
                previewsDirectory.Create();
            }
        }

        public void Delete()
        {
            Directory.Delete(ProcessedDocumentDirectory.FullName, true);
        }
    }
}
