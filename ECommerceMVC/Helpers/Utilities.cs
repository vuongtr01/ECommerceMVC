using System.Text;
namespace ECommerceMVC.Helpers
{
	public class Utilities
	{
		public static string GenerateRandomKey(int length = 5)
		{
			var pattern = @"adfasfklsfjalsqeksfASDFWFHGVCZBN!#*#*#^#%$&";
			var sb = new StringBuilder();
			var random = new Random();
			for (int i = 0; i < length; i++)
			{
				sb.Append(pattern[random.Next(0, pattern.Length)]);
			}

			return sb.ToString();
		}

		public static string UploadImage(IFormFile image, string folder)
		{
			try
			{
				var pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, image.FileName);
				using (var myFile = new FileStream(pathFile, FileMode.CreateNew))
				{
					image.CopyTo(myFile);
				}
				return image.FileName;
			} catch(Exception ex)
			{
				return String.Empty;
			}
		}
	}
}

