using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Caesar_Cipher
{
    class Program
    {
        public static char cipher(char ch, int key)
        {
            if (!char.IsLetter(ch))
            {

                return ch;
            }

            char d = char.IsUpper(ch) ? 'A' : 'a';
            return (char)((((ch + key) - d) % 26) + d);
        }
        public static string Encipher(string input, int key)
        {
            string output = string.Empty;

            foreach (char ch in input)
                output += cipher(ch, key);

            return output;
        }

        public static string Decipher(string input, int key)
        {
            return Encipher(input, 26 - key);
        }


        static void Main(string[] args)
        {
            string UserString;    
            string Keystring;
            int key;
            string path;
            if ((args[0] == "-c" && args[1] == "-e" )|| args[0] == "-e" && args[1] == "-c")
            {   

                Keystring = System.IO.File.ReadAllText(@"key.txt");
                char[] buffer = Keystring.ToCharArray();
                char s = buffer[0];
                key = (int)Char.GetNumericValue(s);
                UserString = System.IO.File.ReadAllText(@"plain.txt");
                Console.Write("Tekst z pliku:  ");
                Console.WriteLine(UserString);
                Console.Write("Zaszyfrowany tekst: ");
                string cipherText = Encipher(UserString, key);
                Console.WriteLine(cipherText);
                Console.WriteLine("Klucz: {0}",key);
                path = @"crypto.txt";
                File.WriteAllText(path, cipherText);
            }

            if ((args[0] == "-c" && args[1] == "-d") || args[0] == "-d" && args[1] == "-c")
            {

                UserString = System.IO.File.ReadAllText(@"crypto.txt");
                Console.Write("Tekst z pliku:  ");
                Console.WriteLine(UserString);
                Keystring = System.IO.File.ReadAllText(@"key.txt");
                char[] buffer = Keystring.ToCharArray();
                char s = buffer[0];
                key = (int)Char.GetNumericValue(s);
                string decipherText = Decipher(UserString, key);
                Console.Write("Tekst rozszyfrowany:  ");
                Console.WriteLine(decipherText);
                path = @"decrypt.txt";
                File.WriteAllText(path, decipherText);
                
            }

            if ((args[0] == "-c" && args[1] == "-j") || args[0] == "-j" && args[1] == "-c")
            {
       
                UserString = System.IO.File.ReadAllText(@"crypto.txt");
                char[] crypto = UserString.ToCharArray();
                string PomocString = System.IO.File.ReadAllText(@"extra.txt");
                char[] pomoc = PomocString.ToCharArray();
                var result = new int[PomocString.Length];

                for (int i = 0; i <PomocString.Length; i ++)
                {
                    result[i] = crypto[i] - pomoc[i];
                }
                double avg = 0;
                for (int j = 0; j < PomocString.Length; j++)
                {
                    avg = avg + result[j];
                }
                double wynik = avg / PomocString.Length;
                if ((result[0] == result[1]) && result[0] == result[PomocString.Length-1] && result[0] == wynik )
                {
                    Console.WriteLine("Poprawny klucz: {0}",result[0]);
                }
                key = result[0];
                Console.Write("Tekst z pliku:  ");
                Console.WriteLine(UserString);
                Console.Write("rozszyfrowany:  ");
                string decipherText = Decipher(UserString, key);
                Console.WriteLine(decipherText);

            }

            if ((args[0] == "-c" && args[1] == "-k") || args[0] == "-k" && args[1] == "-c")
            {
                UserString = System.IO.File.ReadAllText(@"crypto.txt");
                char[] crypto = UserString.ToCharArray();
                string decipherText;
                for (int i = 1; i <= 26; i++)
                {
                    decipherText = Decipher(UserString, i);
                    Console.WriteLine(decipherText);
                }
            }
        }

     }
 }
