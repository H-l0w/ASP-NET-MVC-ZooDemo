using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ZooDemo.Data;
using ZooDemo.Objects;

namespace ZooDemo.Repos
{
    public class ImageRepo
    {
        private readonly string _dirPath;
        private readonly string _galleryPath;
        private ApplicationDbContext _context;

        public ImageRepo(ApplicationDbContext context)
        {
            _context = context;
            string path = Assembly.GetEntryAssembly().Location;

            int bin = path.IndexOf("bin");
            int charCountToDelete = path.Length - bin;
            path = path.Remove(path.IndexOf("bin"), charCountToDelete);

            //path += "\\wwwroot\\Images\\";
            _dirPath = path + "\\wwwroot\\Images\\";
            _galleryPath = path + "\\wwwroot\\Gallery\\";
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

        public void AddImageGallery(Stream stream, Image image)
        {
            try {
                using (FileStream output = new FileStream(_galleryPath + image.Name + ".png", FileMode.Create)) {
                    stream.CopyTo(output);
                }
            }
            catch (Exception) {
            }
            _context.Images.Add(image);
            _context.SaveChanges();
        }

        public void RemoveImageGallery(string name)
        {
            name = name.Replace("\\Gallery\\", "");
            name = name.Replace(".png", "");
            Image temp = _context.Images.FirstOrDefault(x => x.Name == name);

            try {
                File.Delete(_galleryPath + temp.Name + ".png");
            }
            catch (Exception) {
            }
            _context.Images.Remove(temp);
            _context.SaveChanges();
        }

        public List<Image> GetGalleryImages()
        {
            var list = _context.Images.ToList();
            list.ForEach(delegate (Image image) 
            {
                image.Name = "\\Gallery\\" + image.Name + ".png";
            });

            return list;
        }
    }
}
