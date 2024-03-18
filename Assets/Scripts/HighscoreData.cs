using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HighscoreData : IJSONData<HighscoreData>
{
    public List<Entry> Highscores;
    public string PreviousName;

    public HighscoreData CreateNewFile() {
        HighscoreData data = new HighscoreData();
        data.Highscores = new List<Entry>();
        data.PreviousName = "Name";
        return data;
    }

    [System.Serializable]
    public struct Entry {
        public string Name;
        public int Score;
        public int Day;
        public int Month;
        public int Year;

        public Entry(string name, int score, DateTime date) {
            Name = name;
            Score = score;
            Day = date.Day;
            Month = date.Month;
            Year = date.Year;
        }

        public int CompareTo(Entry other) {
            return Score.CompareTo(other.Score);
        }

        public override string ToString() {
            return $"{Name} - {Score}";
        }
    }
}