using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AlishPustakGhar.Utils
{
    public class FileHelper
    {
        public  async Task<string> SaveFile(IFormFile file, string savingFolder)
        {
            // Get the path of the desktop folder
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderName = "PustakGhar";

            // Create main folder on the desktop if it doesn't exist
            var rootFolderPath = Path.Combine(desktopPath, folderName);
            if (!Directory.Exists(rootFolderPath))
            {
                Directory.CreateDirectory(rootFolderPath);
            }

            // Create subfolder if it doesn't exist
            var folderPath = Path.Combine(rootFolderPath, savingFolder);
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
    }
}