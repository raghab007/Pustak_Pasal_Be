using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AlishPustakGhar.Utils
{
    public class FileHelper
    {
        private readonly string _rootFolderPath;

        public FileHelper()
        {
            // Initialize the root folder path once
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _rootFolderPath = Path.Combine(desktopPath, "PustakGhar");
        }

        public async Task<string> SaveFile(IFormFile file, string savingFolder)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null");
            }

            // Create subfolder if it doesn't exist
            var folderPath = Path.Combine(_rootFolderPath, savingFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Extract original file extension
            var extension = Path.GetExtension(file.FileName);

            // Generate a unique file name using GUID
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            // Combine folder path and file name
            var filePath = Path.Combine(folderPath, uniqueFileName);

            // Save the file
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return uniqueFileName;
        }

        public bool DeleteFile(string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            try
            {
                var filePath = Path.Combine(_rootFolderPath, folderName, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }

                return false;
            }
            catch
            {
                // Log error if needed
                return false;
            }
        }

        // Optional: Method to get full file path
        public string GetFilePath(string fileName, string folderName)
        {
            return Path.Combine(_rootFolderPath, folderName, fileName);
        }
    }
}