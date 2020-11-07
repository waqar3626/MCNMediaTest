using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace MCNMedia_Dev._Helper
{
    public static class FileUploadUtility
    {

        /// <summary>
        /// File Upload Utility
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <returns></returns>
        public static string UploadFile(IFormFile fileToUpload, UploadingAreas uploadingArea)
        {
            string rootPath = Directory.GetCurrentDirectory();
            string contentDirectory = ContentDirectory(uploadingArea, churchId: "");
            string path = Path.Combine(rootPath , "wwwroot", contentDirectory);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(fileToUpload.FileName);

            using (var stream = System.IO.File.Create(Path.Combine(path, fileName)))
            {
                fileToUpload.CopyTo(stream);
            }
            return Path.Combine(contentDirectory, fileName).Replace("\\","/");
        }

        public static string UploadFile(IFormFile fileToUpload, UploadingAreas uploadingArea, int churchId)
        {
            string rootPath = Directory.GetCurrentDirectory();
            string contentDirectory = ContentDirectory(uploadingArea, churchId.ToString());
            string path = Path.Combine(rootPath, "wwwroot", contentDirectory);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(fileToUpload.FileName);

            using (var stream = System.IO.File.Create(Path.Combine(path, fileName)))
            {
                fileToUpload.CopyTo(stream);
            }
            return Path.Combine(contentDirectory, fileName).Replace("\\", "/");
        }

        public static string UploadFile(IFormFile fileToUpload, UploadingAreas uploadingArea, string fileName)
        {
            string rootPath = Directory.GetCurrentDirectory();
            string contentDirectory = ContentDirectory(uploadingArea, churchId: "");
            string path = Path.Combine(rootPath, "wwwroot", contentDirectory);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var stream = System.IO.File.Create(Path.Combine(path, fileName)))
            {
                fileToUpload.CopyTo(stream);
            }
            return Path.Combine(contentDirectory, fileName).Replace("\\", "/");
        }

        public static string UploadFile(IFormFile fileToUpload, UploadingAreas uploadingArea, int churchId ,string fileName)
        {
            string rootPath = Directory.GetCurrentDirectory();
            string contentDirectory = ContentDirectory(uploadingArea,churchId.ToString());
            string path = Path.Combine(rootPath, "wwwroot", contentDirectory);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var stream = System.IO.File.Create(Path.Combine(path, fileName)))
            {
                fileToUpload.CopyTo(stream);
            }
            return Path.Combine(contentDirectory, fileName).Replace("\\", "/");
        }


        private static string ContentDirectory(UploadingAreas uploadingAreas, string churchId)
        {
            string rootDirectory = "Uploads";
            switch (uploadingAreas)
            {
                case UploadingAreas.ChurchProfileImage:
                    return Path.Combine(rootDirectory, churchId, "ProfileImages");
                case UploadingAreas.Picture:
                    return Path.Combine(rootDirectory, churchId, "Pictures");
                case UploadingAreas.Video:
                    return Path.Combine(rootDirectory, churchId, "Videos");
                case UploadingAreas.SlideShow:
                    return Path.Combine(rootDirectory, churchId, "SlideShows");
                case UploadingAreas.UserProfileImage:
                    return Path.Combine(rootDirectory, churchId, "UserProfileImages");
                default:
                    return rootDirectory;
            }

        }
    }

}
