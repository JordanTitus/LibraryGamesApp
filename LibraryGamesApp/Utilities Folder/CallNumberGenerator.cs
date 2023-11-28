using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryGamesApp.Utilities_Folder
{
    public class CallNumberGenerator
    {
        Random rand = new Random();

        // Call number description dictionary
        // Instead of using the ranges (0 - 99, 100 - 199, etc.) 
        // I intend to substring the call number to get the first number
        // and compare it to the keys in the dictionary
        // Easier solution than taking the call number and seeing in which range it falls 
        private static Dictionary<int, string> callCategoryDictionary = new Dictionary<int, string>()
        {
            { 0, "General Works" },
            { 1, "Philosophy & Physcology" },
            { 2, "Religion" },
            { 3, "Social Sciences" },
            { 4, "Language" },
            { 5, "Natural Sciences & maths" },
            { 6, "Technology" },
            { 7, "Arts" },
            { 8, "Literature" },
            { 9, "History & Geography" },
        };

        private static Dictionary<string, int> inverseCallCategoryDictionary = new Dictionary<string, int>()
        {
            { "General Works", 0 },
            { "Philosophy & Physcology" , 1 },
            { "Religion" , 2 },
            { "Social Sciences" , 3 },
            { "Language" , 4 },
            { "Natural Sciences & maths" , 5 },
            { "Technology" , 6 },
            { "Arts" , 7 },
            { "Literature" , 8 },
            { "History & Geography" , 9 },
        };
        ///-----------------------------------------------------------------------------------------

        // Stack solution
        public Dictionary<int, string> GetDictionary()
        {
            return callCategoryDictionary;
        }

        public Dictionary<string, int> GetInverseDictionary()
        {
            return inverseCallCategoryDictionary;
        }

        // Creates the call numbers
        public string GenCallNumbers()
        {
            // Creates the 3 numbers preceeding the decimal point
            int numbersPart = rand.Next(1000);

            // Creates the two numbers following the decimal point
            double decimalPart = (rand.Next(100));

            // Creates the 3 letters that make up the authors initials
            string lettersPart = GenerateRandomLetters(3);
            // Console.WriteLine($"{numbersPart:D3}.{decimalPart} {lettersPart.ToUpper()}"); -------------------------> for testing

            return $"{numbersPart:D3}.{decimalPart} {lettersPart.ToUpper()}";
        }
        ///-----------------------------------------------------------------------------------------

        // Gets 3 letters from the alphabet to use for the call numbers
        private string GenerateRandomLetters(int length)
        {
            // String containing all letters
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder result = new StringBuilder(length);

            // Loop that will pick a random letter and append it to the string
            for (int i = 0; i < length; i++)
            {
                result.Append(alphabet[rand.Next(alphabet.Length)]);
            }

            return result.ToString();
        }
        ///-----------------------------------------------------------------------------------------

        // Method that splits call numbers into two strings, dependent on where theres a space
        // Does comparison on the numbers
        // If the numbers are the same, compares the letters alphabetically
        public int CompareCallNumbers(string number1, string number2)
        {
            string[] cn1 = number1.Split(' ');
            string[] cn2 = number2.Split(' ');

            double cn1Number = double.Parse(cn1[0]);
            double cn2Number = double.Parse(cn2[0]);

            int numericComparison = cn1Number.CompareTo(cn2Number);

            if (numericComparison != 0)
            {
                // If the numeric parts are different return the comparison
                return numericComparison;
            }
            else
            {
                // If same, compare letters
                return string.Compare(cn1[1], cn2[1]);
            }
        }
        ///-----------------------------------------------------------------------------------------

        public Boolean CompareDescriptions(string leftSide, string rightSide, int roundCounter)
        {
            int callNumberNumber;

            if (roundCounter % 2 == 0)
            {
                string sub = rightSide.Substring(0, 1);

                callNumberNumber = int.Parse(sub);

                if (callNumberNumber == inverseCallCategoryDictionary[leftSide])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                string sub = leftSide.Substring(0, 1);

                callNumberNumber = int.Parse(sub);

                if (callNumberNumber == inverseCallCategoryDictionary[rightSide])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        // Gets the description matched to the callNumber provided
        // Using substring to get the string at the first index
        // Which would help determine which category it falls into
        public string getmatchingDescription(string callNum, int looper)
        {
            string firstNum = callNum.Substring(0, 1);
            string matching;

            if (looper <= 4)
            {
                // Searches for value(string) by key(int)
                matching = callCategoryDictionary[int.Parse(firstNum)];
            }
            else
            {
                int randomNumber = rand.Next(1, 10);
                // Searches for value(string) by key(int)
                matching = callCategoryDictionary[randomNumber];
            }

            // Console.WriteLine(matching); ---------------------------------------------> for testing
            // Returns the matching description
            return matching;
        }
    }
}
///-----------------------------------------------END-----------------------------------------------
