﻿using System;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace Utility
{
    public static class FileHelper
    {
        public static string ReadAll(string path)
        {
            var fullPath = Path.Combine(Application.dataPath, path);
            if (!File.Exists(fullPath))
                return string.Empty;

            var fileContents = File.ReadAllText(path);
            if (string.IsNullOrEmpty(fileContents))
            {
                Debug.LogWarning($"File at {fullPath} could not be read.");
                return String.Empty;
            }

            return fileContents;
        }

        public static void WriteToFile(string path,[CanBeNull] string content, bool append = false)
        {
            var fullPath = Path.Combine(Application.dataPath, path);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning($"file does not exist at {path}");
                return;
            }

            using (var writer = new StreamWriter(fullPath, append))
            {
                writer.Write(content);
                writer.Flush();
            }
        }
    }
}