using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class Campaign
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Levels { get; set; }

    private int currentIndex = 0;

    public Campaign()
    { }

    public Campaign(string filename)
    {
        Load(filename);
    }

    public void Load(string fileName)
    {
        var fileLines = FileManager.SplitOnNewlines(FileManager.LoadFile(fileName));

        Name = fileLines[0];
        Description = fileLines[1];

        List<string> loadingLevels = new List<string>();
        for (int i = 2; i < fileLines.Length; i++)
            loadingLevels.Add(fileLines[i]);

        Levels = loadingLevels.ToArray();
    }

    public bool HasNextLevel()
    {
        return currentIndex + 1 <= Levels.Length;
    }

    public string NextLevel()
    {
        if (currentIndex >= Levels.Length)
            return null;

        string toReturn = Levels[currentIndex];
        currentIndex++;
        return toReturn;
    }

    public int GetCurrentLevel()
    {
        return currentIndex;
    }
}
