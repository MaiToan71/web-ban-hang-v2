using Microsoft.AspNetCore.Hosting;

namespace Project.Services.Storages
{
    public class FileStorageService : IStorageService
    {

        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "UploadFiles";

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(Environment.CurrentDirectory, USER_CONTENT_FOLDER_NAME); ;
        }

        public string GetFileUrl(string path)
        {

            return $"{path}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }


        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
