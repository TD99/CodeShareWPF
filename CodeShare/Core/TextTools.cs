using System;
using System.IO;
using System.Linq;

namespace CodeShare.Core
{
    public static class TextTools
    {
        public static bool IsBinaryFile(string filePath)
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return fileBytes.Any(b => b > 127);
        }

        public static bool IsBinary(string? text = "")
        {
            if (text == null) return false;

            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            foreach (byte b in textBytes)
            {
                if (b > 127)
                    return true;
            }
            return false;
        }

        public static long GetSizeInMbFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            long fileSizeInBytes = fileInfo.Length;
            long fileSizeInMb = fileSizeInBytes / (1024 * 1024);
            return fileSizeInMb;
        }

        public static long GetSizeInMb(string? text)
        {
            if (text == null) return 0;

            var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            long textSizeInBytes = textBytes.Length;
            long textSizeInMb = textSizeInBytes / (1024 * 1024);
            return textSizeInMb;
        }

        public static string? GetFirstLines(string? text, int numberOfLines)
        {
            if (text == null) return null;

            string[] lines = text.Split('\n');
            string newText = string.Join("\n", lines.Take(numberOfLines));
            if (newText.EndsWith("\n"))
            {
                newText = newText.Substring(0, newText.Length - 1);
            }
            if (newText.EndsWith("\r"))
            {
                newText = newText.Substring(0, newText.Length - 1);
            }
            return newText;
        }

        public static string? RemoveBlankLinesBeforeContent(string? text)
        {
            if (text == null) return null;

            string[] lines = text.Split('\n');
            int firstNonBlankLineIndex = Array.FindIndex(lines, line => !string.IsNullOrWhiteSpace(line));
            return string.Join("\n", lines.Skip(firstNonBlankLineIndex));
        }

    }
}