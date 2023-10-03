using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class WordManager : MonoBehaviour {
    public List<string> FiveLetterWords;
    public List<string> SixLetterWords;
    public string WordA;
    public string WordB;

    void Start() {
        InitializeWords();
    }

    private void InitializeWords() {
        FiveLetterWords = new List<string>();
        string filepathFive = Application.dataPath + "/Resources/FiveLetterWords.txt";
        try {
            using (StreamReader reader = new StreamReader(filepathFive)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string[] words = line.Split('\t');
                    FiveLetterWords.AddRange(words);
                }
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
        }

        SixLetterWords = new List<string>();
        string filepathSix = Application.dataPath + "/Resources/SixLetterWords.txt";
        try {
            using (StreamReader reader = new StreamReader(filepathFive)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string[] words = line.Split('\t');
                    SixLetterWords.AddRange(words);
                }
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
    }

    public void ChooseWords(int vowelCount) {
        int a; int b = 0;
        string wordA = ChooseRandomWord(FiveLetterWords);
        string wordB = "";
        a = CountVowels(wordA);
        while (a + b != vowelCount) {
            wordB = ChooseRandomWord(SixLetterWords);
            b = CountVowels(wordB);
        }
        WordA = wordA;
        WordB = wordB;
    }

    private string ChooseRandomWord(List<string> words) {
        int index = UnityEngine.Random.Range(0, words.Count);
        return words[index];
    }

    private int CountVowels(string word) {
        int count = 0;
        foreach (char c in word) {
            if (c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u') {
                count++;
            }
        }
        return count;
    }
}