using System.IO.Compression;

namespace Nerosoft.Starfish.Common;
public static class GzipHelper
{
	public static string CompressToBase64(string source)
	{
		var buffer = Compress(source);

		return Convert.ToBase64String(buffer);
	}

	public static byte[] Compress(string source)
	{
		var data = Encoding.UTF8.GetBytes(source);

		var stream = new MemoryStream();
		var zip = new GZipStream(stream, CompressionMode.Compress, true);
		zip.Write(data, 0, data.Length);
		zip.Close();
		var buffer = new byte[stream.Length];
		stream.Position = 0;
		_ = stream.Read(buffer, 0, buffer.Length);
		stream.Close();

		return buffer;
	}

	public static string DecompressFromBase64(string base64Data)
	{
		var data = Convert.FromBase64String(base64Data);
		return Decompress(data);
	}

	public static string Decompress(byte[] data)
	{
		return Decompress(data, data.Length);
	}

	public static string Decompress(byte[] data, int count)
	{
		var stream = new MemoryStream(data, 0, count);
		var zip = new GZipStream(stream, CompressionMode.Decompress, true);
		var destStream = new MemoryStream();
		var buffer = new byte[0x1000];
		while (true)
		{
			var reader = zip.Read(buffer, 0, buffer.Length);
			if (reader <= 0)
			{
				break;
			}

			destStream.Write(buffer, 0, reader);
		}

		zip.Close();
		stream.Close();
		destStream.Position = 0;
		buffer = destStream.ToArray();
		destStream.Close();
		return Encoding.UTF8.GetString(buffer);
	}
}
