using UnityEngine;
using System.Collections;
using System;

public static class FileManager
{
    public static string LoadFile(string fileName)
    {
        var filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        var obj = Resources.Load("");
        
        string toReturn = string.Empty;
        if (filePath.Contains("://"))
        {
            var urlReader = new WWW(filePath);
            while (!urlReader.isDone) { }
            
            toReturn = urlReader.text;
            
        }
        else
            toReturn = System.IO.File.ReadAllText(filePath);

        return toReturn;
    }

    public static string[] SplitOnNewlines(string toSplit)
    {
        var fileLines = toSplit.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        return fileLines;
    }
}
