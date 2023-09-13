using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class WordManager : MonoBehaviour
{
    public List<string> FiveLetterWords;
    public List<string> SixLetterWords;

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

        // SixLetterWords = new List<string>();
        // string filepathSix = Application.dataPath + "/Resources/SixLetterWords.txt";
        // try {
        //     using (StreamReader reader = new StreamReader(filepathFive)) {
        //         while (!reader.EndOfStream) {
        //             string line = reader.ReadLine();
        //             string[] words = line.Split('\t');
        //             SixLetterWords.AddRange(words);
        //         }
        //     }
        // } catch (Exception e) {
        //     Debug.Log(e.Message);
        // }
    }
}