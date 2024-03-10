namespace Ecommerce.Utility
{
    public class FileHelper
    {
        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
