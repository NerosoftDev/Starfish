﻿using System.Security.Cryptography;

namespace Nerosoft.Starfish.Common;

/// <summary>
/// A utility class for generating random numbers or strings.
/// </summary>
internal class RandomUtility : Random
{
	private static readonly RandomNumberGenerator _generator = RandomNumberGenerator.Create();
	private readonly byte[] _uint32Buffer = new byte[4];

	/// <summary>
	/// Generate a random string with a given size.
	/// </summary>
	/// <param name="length"></param>
	/// <returns></returns>
	public static string GenerateUniqueId(int length = 32)
	{
		var bytes = new byte[Convert.ToInt32(length / 2)];
		_generator.GetBytes(bytes);
		var hex = new StringBuilder(bytes.Length * 2);
		foreach (var b in bytes)
		{
			hex.Append($"{b:x2}");
		}

		return hex.ToString();
	}

	/// <summary>
	/// Returns a nonnegative random number.
	/// </summary>
	/// <returns>
	/// A 32-bit signed integer greater than or equal to zero and less than <see cref="F:System.Int32.MaxValue"/>.
	/// </returns>
	public override int Next()
	{
		_generator.GetBytes(_uint32Buffer);
		return BitConverter.ToInt32(_uint32Buffer, 0) & 0x7FFFFFFF;
	}

	/// <summary>
	/// Returns a nonnegative random number less than the specified maximum.
	/// </summary>
	/// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to zero.</param>
	/// <returns>
	/// A 32-bit signed integer greater than or equal to zero, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily includes zero but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals zero, <paramref name="maxValue"/> is returned.
	/// </returns>
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	/// 	<paramref name="maxValue"/> is less than zero.
	/// </exception>
	public override int Next(int maxValue)
	{
		if (maxValue < 0)
			throw new ArgumentOutOfRangeException(nameof(maxValue));
		return Next(0, maxValue);
	}

	/// <summary>
	/// Returns a random number within a specified range.
	/// </summary>
	/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
	/// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
	/// <returns>
	/// A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>. If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
	/// </returns>
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	/// 	<paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
	/// </exception>
	public override int Next(int minValue, int maxValue)
	{
		if (minValue > maxValue)
			throw new ArgumentOutOfRangeException(nameof(minValue));
		if (minValue == maxValue)
			return minValue;
		long diff = maxValue - minValue;

		while (true)
		{
			_generator.GetBytes(_uint32Buffer);
			var rand = BitConverter.ToUInt32(_uint32Buffer, 0);

			var max = (1 + (long)uint.MaxValue);
			var remainder = max % diff;
			if (rand < max - remainder)
			{
				return (int)(minValue + (rand % diff));
			}
		}
	}

	/// <summary>
	/// Returns a random number between 0.0 and 1.0.
	/// </summary>
	/// <returns>
	/// A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
	/// </returns>
	public override double NextDouble()
	{
		_generator.GetBytes(_uint32Buffer);
		var rand = BitConverter.ToUInt32(_uint32Buffer, 0);
		return rand / (1.0 + uint.MaxValue);
	}

	/// <summary>
	/// Fills the elements of a specified array of bytes with random numbers.
	/// </summary>
	/// <param name="buffer">An array of bytes to contain random numbers.</param>
	/// <exception cref="T:System.ArgumentNullException">
	/// 	<paramref name="buffer"/> is null.
	/// </exception>
	public override void NextBytes(byte[] buffer)
	{
		if (buffer == null)
			throw new ArgumentNullException(nameof(buffer));
		_generator.GetBytes(buffer);
	}
}