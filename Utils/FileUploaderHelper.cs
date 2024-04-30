using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace GroupCoursework.Utils
{
    public class FileUploaderHelper
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileUploaderHelper(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null");

            string contentRootPath = _hostingEnvironment.ContentRootPath;

            // Get the web root path

            // Ensure the destination folder exists, if not, create it
            string uploadFolder = Path.Combine(contentRootPath, "Assets", "Images");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            // Generate a unique file name to avoid overwriting existing files
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Combine the upload folder path with the file name
            string filePath = Path.Combine(uploadFolder, fileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Return the file name (or full path if needed)
            return fileName;
        }
    }
}
