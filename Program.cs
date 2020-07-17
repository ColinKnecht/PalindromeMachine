using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ColinKnecht_PalindromeMachine
{

    class Program
    {
        //Colin Knecht
        //cknecht87@gmail.com
        //740-817-1006

        //****Write up a short documentation explaining what your program does.
        //My program takes in a user inputted paragraph; which in turn.....
        //counts the number of palindrome words
        //counts the number of palindrome sentences
        //Lists all unique words with the total number of times a count is used
        //accepts a user inputted letter to check how many words have letter user entered
        private const string CHARACTERS_TO_REPLACE = "[*'\",:;().-?!_&#^@]+";
        static void Main(string[] args)
        {
            TurnConsoleColorBlue();
            Console.WriteLine("Welcome to the Palindrome Machine!  Please enter your paragraph to check....");
            string paragraph = Console.ReadLine();

            TurnConsoleColorRed();
            Console.WriteLine("Number of palindrome words in paragraph is: " + CountPalindromeWords(paragraph).ToString());
            Console.WriteLine("Number of palindrome sentences in paragraph is: " + CountPalindromeSentences(paragraph).ToString());

            TurnConsoleColorGreen();
            PrintUniqueWordsToConsole(ListUniqueWordsWithCount(paragraph));
            
            TurnConsoleColorYellow();
            Console.WriteLine("=================================================");
            Console.WriteLine("Please enter a letter to check which words have that letter.....");
            string letter = Console.ReadLine();

            while(letter.Length != 1)
            {
                Console.WriteLine("Please enter a 1 letter character!! Enter letter below...");
                letter = Console.ReadLine();
            }
            CheckUserInputLetter(letter, paragraph);

            TurnConsoleColorWhite();
            Console.WriteLine("Thanks for playing!! Enter any key to exit!!");
            Console.ReadLine();
        }

        private static int CountPalindromeWords(string paragraph) //Give the number of palindrome words
        {
            int palindromeCount = 0;
            //remove periods and special characters
            paragraph = Regex.Replace(paragraph, CHARACTERS_TO_REPLACE, "");
            //turn paragraph into array of words
            string[] words = paragraph.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            foreach(string wrd in words)
            {
                string word = wrd.ToLower();
                if (word.SequenceEqual(word.Reverse()) && word.Length > 1)
                {
                    palindromeCount++;
                }
            }

            return palindromeCount;
        }

        private static int CountPalindromeSentences(string paragraph) //Give the number of palindrome sentences
        {
            int palindromeCount = 0;
            //split paragraph into array of sentences
            char[] separators = new char[] { '!', '.', '?' };
            string[] sentences = paragraph.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach(string sentence in sentences)
            {
                string sent = Regex.Replace(sentence, CHARACTERS_TO_REPLACE, "");
                //squash sentences into just text.. no special chars
                sent = Regex.Replace(sent, @"\s", "");
                sent = sent.ToLower(); //ignore case
                if (sent.SequenceEqual(sent.Reverse()) && sent.Length > 1)
                {
                    palindromeCount++;
                }
            }

            return palindromeCount;
        }

        private static Dictionary<string, int> ListUniqueWordsWithCount(string paragraph) //List the unique words of a paragraph with the count of the word instance
        {
            string[] words = paragraph.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string paragraphToUnique = "";

            for (int i = 0; i < words.Length; i++)
            {
                //remove special characters from word
                words[i] = Regex.Replace(words[i], CHARACTERS_TO_REPLACE, "");
                //strip out the apostrophes and special characters in words to add to uniqueWords hashset
                paragraphToUnique += words[i] + " ";
            }

            Char[] separators = new Char[] { ' ', '\r', '\n', '\t', ',', '.', ';', '!', '?', '(', ')', '-', '+' };

            HashSet<String> uniqueWords = new HashSet<String>(paragraphToUnique.Split(separators, StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase);

            HashSet<string>.Enumerator uniqueWordsEnumerator = uniqueWords.GetEnumerator();
            Dictionary<string, int> wordCountDictionary = new Dictionary<string, int>();
            while (uniqueWordsEnumerator.MoveNext())
            {
                string currentWord = uniqueWordsEnumerator.Current;
                currentWord = currentWord.ToLower();
                wordCountDictionary.Add(currentWord, 0);
                foreach(string wrd in words)
                {
                    string word = wrd.ToLower();
                    if (currentWord.Equals(word))
                    {
                        //C# won't let you access Dictionary value with wordCountDictionary[currentWord]
                        //had to iterate through list with loop below to get value out
                        for (int i = wordCountDictionary.Count - 1; i >= 0; i--)
                        {
                            if (wordCountDictionary.ElementAt(i).Key.Equals(currentWord))
                            {
                                int instancesOfWord = wordCountDictionary.ElementAt(i).Value;
                                wordCountDictionary[currentWord] = instancesOfWord + 1;
                            }
                        }
                    }
                }
            }
            return wordCountDictionary;
        }

        private static void PrintUniqueWordsToConsole(Dictionary<string, int> wordCountDictionary) //print unique words
        {
            var sortedDict = from entry in wordCountDictionary orderby entry.Value descending select entry;

            foreach(KeyValuePair<string, int> entry in sortedDict)
            {
                Console.WriteLine(entry.Key + " --------- occurs " + entry.Value + " times.");
            }
        }

        private static void CheckUserInputLetter(string letter, string paragraph) //Let the user also input a letter at some point and list all words containing that letter
        {
            string letterIgnoreCase = letter.ToString().ToLower();
            ArrayList wordsWithLetter = new ArrayList();
            string[] words = paragraph.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                //remove special characters from word
                words[i] = Regex.Replace(words[i], CHARACTERS_TO_REPLACE, "");
            }

            //check for char and add it to list to return
            foreach (string wrd in words)
            {
                string word = wrd.ToLower();
                if (word.Contains(letterIgnoreCase) && !wordsWithLetter.Contains(word))
                {
                    wordsWithLetter.Add(word);
                }
            }
            
            if (wordsWithLetter.Count == 0)
            {
                Console.WriteLine("There are no words containing the letter: " + letterIgnoreCase);
            }
            else
            {
                Console.WriteLine("Words Containing the Letter: " + letterIgnoreCase);
                foreach (string word in wordsWithLetter)
                {
                    Console.WriteLine(word);
                }
            }
        }

        private static void ResetConsoleColors()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void TurnConsoleColorRed()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        private static void TurnConsoleColorBlue()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void TurnConsoleColorGreen()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void TurnConsoleColorYellow()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        private static void TurnConsoleColorWhite()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}
