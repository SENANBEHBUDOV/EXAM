namespace Bootstrap_3.Extension
{
    public static class FileExtension
    {
        public static bool CheckMemory(this IFormFile file, int kb) => kb * 1024 * 1024 > file.Length;
        public static bool CheckType(this IFormFile file, string type) => file.ContentType.Contains(type);
        public static string ChangeFileName(string oldname)
        {
            string extension = oldname.Substring(oldname.LastIndexOf('.'));
            if (oldname.Length < 32)
            {
                oldname = oldname.Substring(0, oldname.LastIndexOf('.'));
            }
            else
            {
                oldname = oldname.Substring(0, 32);
            }
            return Guid.NewGuid() + oldname + extension;
        }
        public static string SaveFile(this IFormFile file, string path)
        {
            string newname = ChangeFileName(file.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, newname), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return newname;
        }
        public static void DeleteFile(this string filename, string folder, string root)
        {
            string path = Path.Combine(root, folder, filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static string CheckValidate(this IFormFile file, string type, int kb)
        {
            string result = "";
            if (!file.CheckMemory(kb))
            {
                result = $"Faylin olcusu {kb}-dan artiq ola bilmez";
            }
            if (!file.CheckType(type))
            {
                result = $"faylin tipi {type} olmalidir";
            }
            return result;
        }
    }
}