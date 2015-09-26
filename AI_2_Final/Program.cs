using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_2_Final
{
    class Program
    {
        static void Main(string[] args)
        {
            // read wordfile in
            string wordsFilePath = "C:\\Users\\ResearchBeast\\Documents\\Visual Studio 2015\\Projects\\CECS451_Asgn2_DLeiva_2\\CECS451_Asgn2_DLeiva_2\\bin\\Debug\\dictionary.txt";
            List<string> words = new List<string>();
            using (System.IO.StreamReader reader = new System.IO.StreamReader(wordsFilePath))
                while (!reader.EndOfStream)
                    words.Add(reader.ReadLine());

            // read corpus in
            // caulculate pair frequency of corpus
            string corpusFilePath = "C:\\Users\\ResearchBeast\\Documents\\Visual Studio 2015\\Projects\\CECS451_Asgn2_DLeiva_2\\CECS451_Asgn2_DLeiva_2\\bin\\Debug\\corpus.txt";
            string corpusTxt;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(corpusFilePath))
                corpusTxt = reader.ReadToEnd();
            string[] corpusWords = corpusTxt.Split();

            // add all words to frequency with value of 0
            Dictionary<string, int> corpusWordFreq = new Dictionary<string, int>();

            for (int i = 0; i < words.Count; i++)
                corpusWordFreq.Add(words[i], 0);

            // count the frequency of the words
            for (int i = 0; i < corpusWords.Length - 1; i++)
                corpusWordFreq[corpusWords[i]]++;
            corpusWordFreq = corpusWordFreq.OrderBy(p => -p.Value).ToDictionary(x => x.Key, p => p.Value);
            // probably useful traits
            double averageWordLenght = corpusWords.Average(p => p.Length);
            double minLength = corpusWords.Min(p => p.Length != 0 ? p.Length : 100);
            double maxLength = corpusWords.Max(p => p.Length);

            // find min/max frequency
            var minFrequencyWord = corpusWordFreq.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            var maxFrequencyWord = corpusWordFreq.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            Dictionary<string, int> encryptedWords;

            string rootFileName = "RepetitionFIle_repetition_";

            for (int i = 2; i < 50; i++)
            {
                encryptedWords = GetEncryptedWords(i);
                // order words
                encryptedWords = encryptedWords.OrderBy(p => -p.Value).ToDictionary(k => k.Key, v => v.Value);
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(rootFileName + i + ".txt"))
                {
                    writer.WriteLine("Repetitions Cutoff: " + i);
                    writer.WriteLine("Total Words: " + encryptedWords.Count);
                    writer.WriteLine("\n\nWords:\n");
                    foreach (KeyValuePair<string, int> pair in encryptedWords)
                        writer.WriteLine("{0,-40} : {1, 40}", pair.Key, pair.Value);
                }
            }
        }

        public static Dictionary<string, int> GetEncryptedWords(int cutoffRepetitions)
        {
            Dictionary<string, int> words = new Dictionary<string, int>();

            // TO_DO
            string encryptedFilePath = "C:\\Users\\ResearchBeast\\Documents\\Visual Studio 2015\\Projects\\CECS451_Asgn2_DLeiva_2\\CECS451_Asgn2_DLeiva_2\\bin\\Debug\\encrypted.txt";
            string encryptedTxt;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(encryptedFilePath))
                encryptedTxt = reader.ReadToEnd();

            int repetitions;
            int j = 0;

            for (int i = 0; i + j < encryptedTxt.Length;)
            {
                for (j = 2; i + j < encryptedTxt.Length; j += 2)
                {
                    if (words.ContainsKey(encryptedTxt.Substring(i, j)))
                    {
                        words[encryptedTxt.Substring(i, j)]++;
                        i += j;
                        break;
                    }
                    repetitions = CountRepetitions(encryptedTxt.Substring(i, encryptedTxt.Length - i), encryptedTxt.Substring(i, j));
                    if (repetitions < cutoffRepetitions)
                    {
                        if (j - 2 != 0)
                            words.Add(encryptedTxt.Substring(i, j - 2), 1);
                        i += j - 2;
                        break;
                    }
                }
            }

            return words;
        }

        public static int CountRepetitions(string mainText, string stringToFind)
        {
            return (mainText.Length - mainText.Replace(stringToFind, "").Length) / stringToFind.Length;
        }
    }
}