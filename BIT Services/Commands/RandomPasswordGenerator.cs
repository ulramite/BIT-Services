using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Commands
{
	class RandomPasswordGenerator
	{
		// Obtained from https://stackoverflow.com/questions/18110243/random-word-generator-2
		// Used in password generation, modified slightly
		Random rnd = new Random();
		private string WordFinder2(int requestedLength)
		{
			
			string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
			string[] vowels = { "a", "e", "i", "o", "u" };

			string word = "";

			if (requestedLength == 1)
			{
				word = GetRandomLetter(rnd, vowels);
			}
			else
			{
				for (int i = 0; i < requestedLength; i += 2)
				{
					word += GetRandomLetter(rnd, consonants) + GetRandomLetter(rnd, vowels);
				}

				word = word.Replace("q", "qu").Substring(0, requestedLength); // We may generate a string longer than requested length, but it doesn't matter if cut off the excess.
			}

			return word;
		}

		private static string GetRandomLetter(Random rnd, string[] letters)
		{
			return letters[rnd.Next(0, letters.Length - 1)];
		}



		/// <summary>
		/// Generates a password string. If used multiple times in a row will only generate the same string
		/// </summary>
		/// <returns></returns>
		public static string GeneratePassword()
		{
			string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
			string[] special = { "-", "+", "[", "]", "\\", "/", ";", "'", ":", "\"", ",", ".", "?", "<", ">" };
			Random rnd = new Random();
			StringBuilder password = new StringBuilder();
			RandomPasswordGenerator passwordGenerator = new RandomPasswordGenerator();
			password.Append(passwordGenerator.WordFinder2(5));
			password.Append(numbers[rnd.Next(0, 9)]);
			password.Append(passwordGenerator.WordFinder2(7));
			password.Append(special[rnd.Next(0,14)]);
			password.Append(passwordGenerator.WordFinder2(5));

			return password.ToString();
		}
	}
}
