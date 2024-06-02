using System.IO.Compression;

// ReSharper disable MemberCanBePrivate.Global

namespace Nerosoft.Starfish.Common;

public static class GzipHelper
{
	/// <summary>
	/// 使用Gzip压缩字符串并转换为Base64字符串
	/// </summary>
	/// <param name="source"></param>
	/// <returns></returns>
	public static string CompressToBase64(string source)
	{
		var buffer = Compress(source);

		return Convert.ToBase64String(buffer);
	}

	/// <summary>
	/// 使用Gzip压缩字符串
	/// </summary>
	/// <param name="source"></param>
	/// <returns></returns>
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

	/// <summary>
	/// 解压Base64字符串
	/// </summary>
	/// <param name="base64Data"></param>
	/// <returns></returns>
	/// <remarks>
	/// 此操作将Base64字符串解码为字节数组，然后解压缩
	/// </remarks>
	public static string DecompressFromBase64(string base64Data)
	{
		var data = Convert.FromBase64String(base64Data);
		return Decompress(data);
	}

	/// <summary>
	/// 解压Gzip压缩的字节数组
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	public static string Decompress(byte[] data)
	{
		return Decompress(data, data.Length);
	}

	/// <summary>
	/// 解压Gzip压缩的字节数组
	/// </summary>
	/// <param name="data"></param>
	/// <param name="count"></param>
	/// <returns></returns>
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