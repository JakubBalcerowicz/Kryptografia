using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace cezar
{
    class cezar
    {
        public static string AffineEncrypt(string plainText, int a, int b)
        {
            string cipherText = "";
            char[] chars = plainText.ToUpper().ToCharArray();
            foreach (char c in chars)
            {
                if (!char.IsLetter(c))
                {
                    cipherText = cipherText + " ";
                    continue;
                }

                int x = Convert.ToInt32(c - 65);
                cipherText += Convert.ToChar(((a * x + b) % 26) + 65);
            }
            return cipherText;
        }
        public static string AffineDecrypt(string cipherText, int a, int b)
        {
            string plainText = "";

            int aInverse = MultiplicativeInverse(a);

            char[] chars = cipherText.ToUpper().ToCharArray();

            foreach (char c in chars)           
            {
                if (!char.IsLetter(c))
                {
                    plainText = plainText + " ";
                    continue;
                }
                int x = Convert.ToInt32(c - 65);
                if (x - b < 0) x = Convert.ToInt32(x) + 26;
                plainText += Convert.ToChar(((aInverse * (x - b)) % 26) + 65);
            }

            return plainText;
        }

        public static int MultiplicativeInverse(int a)
        {
            for (int x = 1; x < 27; x++)
            {
                if ((a * x) % 26 == 1)
                    return x;
            }

            throw new Exception("No multiplicative inverse found!");
        
        }


        public static int Keyfinder()
        {
            
            string UserString = System.IO.File.ReadAllText(@"crypto.txt");
            char[] crypto = UserString.ToCharArray();
            string plainstring = System.IO.File.ReadAllText(@"extra.txt");
            char[] plain = plainstring.ToCharArray();

            int result;
            int result1;
            int result2;
            string path;
            for (int a = 1; a < 26; a = a + 2)
                {
                    for (int b = 1; b < 26; b++)
                    {
                    
                        result = ((plain[0] - 65) * a + b) % 26;
                        result1 = ((plain[1] - 65) * a + b) % 26;
                        result2 = ((plain[2] - 65) * a + b) % 26;


                        if (result == (crypto[0] - 65) && result1 == (crypto[1] - 65) && result2 == (crypto[2] - 65))
                         {    
                              Console.WriteLine("znaleziono klucz");
                              string decipherText = AffineDecrypt(UserString, a, b);
                              Console.Write("Tekst rozszyfrowany:  ");
                              Console.WriteLine(decipherText);
                              path = @"decrypt.txt";
                              File.WriteAllText(path, decipherText);
                              return 1;
                    }
                    }
                    
                }
            Console.WriteLine("nie mozna znalesc klucza");
            return 0;
        }
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
                
                string[] words;
                words = Keystring.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                UserString = System.IO.File.ReadAllText(@"plain.txt");
                Console.Write("Tekst z pliku:  ");
                Console.WriteLine(UserString);           
                    string Text;
                    try
                    {
                        int number = Int32.Parse(words[0]);
                        Text = Encipher(UserString, number);
                        Console.WriteLine("Zaszyfrowany tekst: {0}",Text);
                        path = @"crypto.txt";
                        File.WriteAllText(path, Text);

                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("{0}: zly format klucza", words[0]);
                        
                    }
            }

            if ((args[0] == "-c" && args[1] == "-d") || args[0] == "-d" && args[1] == "-c")
            {

                UserString = System.IO.File.ReadAllText(@"crypto.txt");
                Console.Write("Tekst z pliku:  ");
                Console.WriteLine(UserString);
                Keystring = System.IO.File.ReadAllText(@"key.txt");
                string[] words;
                words = Keystring.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                string decipherText;
                try
                {
                    int number = Int32.Parse(words[0]);
                    decipherText = Decipher(UserString, number);
                    Console.Write("Tekst rozszyfrowany:  ");
                    Console.WriteLine(decipherText);
                    path = @"decrypt.txt";
                    File.WriteAllText(path, decipherText);
                }
                catch
                {
                    Console.WriteLine("{0}: zly format klucza", words[0]);
                }         
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
                    if (result[i] < 0)
                    {
                        result[i] = result[i] + 26;
                    }

                }
                double avg = 0;
                for (int j = 0; j < PomocString.Length; j++)
                {
                    avg = avg + result[j];
                }
                double wynik = avg / PomocString.Length;
                if ((result[0] == result[1]) && result[0] == result[PomocString.Length-1] && result[0] == wynik )
                {
                    if (result[0] > 0)
                    {
                        Console.WriteLine("Poprawny klucz: {0}", result[0]);
                    }
                    else
                    {
                        Console.WriteLine("Poprawny klucz: {0}", result[0]*-1);
                    }
                    key = result[0];
                    Console.Write("Tekst z pliku:  ");
                    Console.WriteLine(UserString);
                    Console.Write("rozszyfrowany:  ");
                    string decipherText = Decipher(UserString, key);
                    Console.WriteLine(decipherText);
                }
                else
                {
                    Console.WriteLine("nie da sie obliczyc klucza {0}",result[0]);
                }
                    
            }

            if ((args[0] == "-c" && args[1] == "-k") || args[0] == "-k" && args[1] == "-c")
            {
                UserString = System.IO.File.ReadAllText(@"crypto.txt");
                char[] crypto = UserString.ToCharArray();
                string decipherText;
                for (int i = 1; i < 26; i++)
                {
                    decipherText = Decipher(UserString, i);
                    Console.WriteLine(decipherText +"  "+i);
                }
            }
            if ((args[0] == "-a" && args[1] == "-e") || args[0] == "-e" && args[1] == "-a")
            {
                Keystring = System.IO.File.ReadAllText(@"key.txt");

                string[] words;
                words = Keystring.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                UserString = System.IO.File.ReadAllText(@"plain.txt");
                Console.Write("Tekst z pliku:  ");
                Console.WriteLine(UserString);
                string Text;
                try
                {
                    int number = Int32.Parse(words[0]);
                    int number2 = Int32.Parse(words[1]);
                    Text = AffineEncrypt(UserString, number,number2);
                    Console.WriteLine("Zaszyfrowany tekst: {0}", Text);
                    path = @"crypto.txt";
                    File.WriteAllText(path, Text);
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0}: zly format klucza", words[0]);

                }
            }
            if ((args[0] == "-a" && args[1] == "-d") || args[0] == "-d" && args[1] == "-a")
            {
                UserString = System.IO.File.ReadAllText(@"crypto.txt");
                Console.Write("Tekst z pliku:  ");
                Console.WriteLine(UserString);
                Keystring = System.IO.File.ReadAllText(@"key.txt");
                string[] words;
                words = Keystring.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                string decipherText;
                try
                {
                    int number = Int32.Parse(words[0]);
                    int number2 = Int32.Parse(words[1]);
                    decipherText = AffineDecrypt(UserString, number,number2);
                    Console.Write("Tekst rozszyfrowany:  ");
                    Console.WriteLine(decipherText);
                    path = @"decrypt.txt";
                    File.WriteAllText(path, decipherText);
                }
                catch
                {
                    Console.WriteLine("{0}: zly format klucza", words[0]);
                }
                
            }
            if ((args[0] == "-a" && args[1] == "-j") || args[0] == "-j" && args[1] == "-a")
            {
                Keyfinder();
            }

                if ((args[0] == "-a" && args[1] == "-k") || args[0] == "-k" && args[1] == "-a")
            {
                UserString = System.IO.File.ReadAllText(@"crypto.txt");
                char[] crypto = UserString.ToCharArray();
                string decipherText;
                int h = 1;
                for (int i = 1; i < 26; i+=2) {
                    for (int j = 1; j <= 26; j++)
                    {
                        if (i == 13)
                        {
                            i = i + 2;
                        }
                        else
                        {
                            decipherText = AffineDecrypt(UserString, i, j);
                            h++;
                            Console.WriteLine(decipherText+"  "+ h);
                        }


                    }
                }
            }

        }

     }
 }
