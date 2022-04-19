using System;
using System.IO;
using System.Reflection;

namespace ZooDemo.Repos
{
    public class ImageRepo
    {
        private readonly string _dirPath;

        public ImageRepo()
        {
            string path = Assembly.GetEntryAssembly().Location;

            int bin = path.IndexOf("bin");
            int charCountToDelete = path.Length - bin;
            path = path.Remove(path.IndexOf("bin"), charCountToDelete);
            path += "\\wwwroot\\Images\\";
            _dirPath = path;
        }

        public void AddImage(Stream stream, string name)
        {
            try {
                using (FileStream outputFileStream = new FileStream(_dirPath + name, FileMode.Create)) {
                    stream.CopyTo(outputFileStream);
                }
            }
            catch (Exception) {
            }
        }

        public void DeleteImage(string name)
        {
            try {
                File.Delete(_dirPath + name);
            }
            catch (Exception) {
            }
        }

        public void UpdateImage(string oldFile, string newFile, Stream stream)
        {
            try {
                AddImage(stream, newFile);
                DeleteImage(oldFile);
            }
            catch (Exception) {
            }    
        }
    }
}
