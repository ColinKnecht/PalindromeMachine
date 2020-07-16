using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ColinKnecht_PalindromeMachine
{
    class Program
    {
        //Using VS Code, write a C# solution to take in an input of a paragraph and:
        //Give the number of palindrome words
        //Give the number of palindrome sentences
        //List the unique words of a paragraph with the count of the word instance
        //Let the user also input a letter at some point and list all words containing that letter
        //Write up a short documentation explaining what your program does.
        static void Main(string[] args)
        {
            //TODO's - get rid of 1 letter palindromes
            //check for copy paste multiple lines
            // get ride of somewhere? in last example when entering 'o'
            Console.WriteLine("Welcome to the Palindrome Machine!  Please enter your paragraph to check....");
            string paragraph = Console.ReadLine();
            Console.WriteLine("Number of palindrome words in paragraph is: " + CountPalindromeWords(paragraph).ToString());
            Console.WriteLine("Number of palindrome sentences in paragraph is: " + CountPalindromeSentences(paragraph).ToString());
            ListUniqueWordsWithCount(paragraph);
            Console.WriteLine("=================================================");
            Console.WriteLine("Please enter a letter to check which words have that letter.....");
            string letter = Console.ReadLine();
            
            while(letter.Length != 1)
            {
                Console.WriteLine("Please enter a 1 letter character!! Enter letter below...");
                letter = Console.ReadLine();
            }
            CheckUserInputLetter(letter, paragraph);

            Console.WriteLine("Thanks for playing!! Enter any key to exit!!");
            Console.ReadLine();
        }

        public static int CountPalindromeWords(string paragraph)
        {
            int palindromeCount = 0;
            //remove periods and special characters
            paragraph = Regex.Replace(paragraph, "[';,().-]+", "");
            //turn paragraph into array of words
            string[] words = paragraph.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            foreach(string word in words)
            {
                if (word.SequenceEqual(word.Reverse()))
                {
                    palindromeCount++;
                }
            }

            return palindromeCount;
        }

        public static int CountPalindromeSentences(string paragraph)
        {
            int palindromeCount = 0;
            //split paragraph into array of sentences
            char[] separators = new char[] { '!', '.', '?' };
            string[] sentences = paragraph.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            
            foreach(string sentence in sentences)
            { 
                string sent = Regex.Replace(sentence, "[';,().-]+", "");
                //squash sentences into just text.. no special chars
                sent = Regex.Replace(sent, @"\s", "");
                sent = sent.ToLower(); //ignore case
                if (sent.SequenceEqual(sent.Reverse()))
                {
                    palindromeCount++;
                }
            }

            return palindromeCount;
        }

        public static void ListUniqueWordsWithCount(string paragraph)
        {
            string[] words = paragraph.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string paragraphToUnique = "";

            for (int i = 0; i < words.Length; i++)
            {
                //remove special characters from word
                words[i] = Regex.Replace(words[i], "[';,().-]+", "");
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
            
        }

        public static void CheckUserInputLetter(string letter, string paragraph)
        {
            //Let the user also input a letter at some point and list all words containing that letter
            string letterIgnoreCase = letter.ToString().ToLower();
            ArrayList wordsWithLetter = new ArrayList();
            string[] words = paragraph.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                //remove special characters from word
                words[i] = Regex.Replace(words[i], "[';,().-]+", "");
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

            Console.ForegroundColor = ConsoleColor.Yellow;
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
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
