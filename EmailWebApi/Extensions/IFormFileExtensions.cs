namespace EmailWebApi.Extensions
{
    public static class IFormFileExtensions
    {
        public static bool IsDocxExtension(this IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);

            if (extension == ".docx")
            {
                return true;
            }

            return false;
        }
    }
}
