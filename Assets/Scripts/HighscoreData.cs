using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HighscoreData : IJSONData<HighscoreData>
{
    public List<Entry> Highscores;
    public string PreviousName;

    public HighscoreData CreateNewFile()
    {
        HighscoreData data = new HighscoreData();
        data.Highscores = new List<Entry>();
        data.PreviousName = "Name";
        return data;
    }

    [System.Serializable]
    public struct Entry
    {
        public string Name;
        public int CleanWins;
        public int RetryWins;
        public int CleanLosses;
        public int RetryLosses;
        public float AverageScore;
        public float TotalScore;
        public int TotalGames;

        public Entry(string name, int cleanWins, int retryWins, int cleanLosses, int retryLosses)
        {
            Name = name;
            CleanWins = cleanWins;
            RetryWins = retryWins;
            CleanLosses = cleanLosses;
            RetryLosses = retryLosses;
            TotalGames = CleanWins + RetryWins + CleanLosses + RetryLosses;
            TotalScore = CleanWins * 10f + RetryWins * 4f + CleanLosses * 1.5f + RetryLosses * 0f;
            AverageScore = (float)Math.Round(TotalScore / TotalGames, 3);
        }

        public override string ToString()
        {
            return $"{Name} - {CleanWins} - {RetryWins} - {CleanLosses} - {RetryLosses} - {TotalScore} - {TotalGames} - {AverageScore}";
        }

        public int CompareTo(Entry other)
        {
            return AverageScore.CompareTo(other.AverageScore);
        }
    }
}
