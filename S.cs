using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public static class Steganography
{
    public static void Encode(string inputPath, string outputPath, string message)
    {
        using var image = Image.Load<Rgba32>(inputPath);
        string binaryMessage = ToBinary(message) + "0000000000000000"; // End marker
        int bitIndex = 0;

        for (int y = 0; y < image.Height && bitIndex < binaryMessage.Length; y++)
        {
            for (int x = 0; x < image.Width && bitIndex < binaryMessage.Length; x++)
            {
                Rgba32 pixel = image[x, y];

                pixel.R = SetLsb(pixel.R, binaryMessage, ref bitIndex);
                pixel.G = SetLsb(pixel.G, binaryMessage, ref bitIndex);
                pixel.B = SetLsb(pixel.B, binaryMessage, ref bitIndex);

                image[x, y] = pixel; // 書き戻し
            }
        }

        image.Save(outputPath);
    }

    public static string Decode(string inputPath)
    {
        using var image = Image.Load<Rgba32>(inputPath);
        StringBuilder binary = new();

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var pixel = image[x, y];
                binary.Append(pixel.R & 1);
                binary.Append(pixel.G & 1);
                binary.Append(pixel.B & 1);
            }
        }

        StringBuilder result = new();

        for (int i = 0; i + 8 <= binary.Length; i += 8)
        {
            string byteStr = binary.ToString(i, 8);
            if (byteStr == "00000000") break;
            result.Append((char)Convert.ToByte(byteStr, 2));
        }

        return result.ToString();
    }

    private static byte SetLsb(byte value, string bits, ref int index)
    {
        if (index >= bits.Length) return value;
        byte bit = (byte)(bits[index++] - '0');
        return (byte)((value & ~1) | bit);
    }

    private static string ToBinary(string text)
    {
        return string.Concat(Encoding.UTF8.GetBytes(text).Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
    }
}
