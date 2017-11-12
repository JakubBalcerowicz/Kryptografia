using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace XOR
{
    class Program
    {
        public static int[] KeyBreake(int[] crypto)
        {
            int[] key = new int[64];
            int result;
            for (int i =0; i < 64; i++)
            {
                if(crypto[i] >=64 && crypto[i] <= 191)
                {
                    result = 32 ^ crypto[i];
                    key[i] = result;
                }
                else
                {
                    key[i] = 0;
                }
            }
        return key;
        }
        public static int LineCount()
        {
            var lineCount = 0;
            using (var reader = File.OpenText(@"plain.txt"))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }
            return lineCount;
        }
        public static int[] Encrypt(string text, string key, int size)
        {
            int encrypt;
            int[] result = new int[size];
            for (int i = 0; i < size; i++)
            {
                int a = text[i];
                int b = key[i];
                encrypt = a ^ b;
                result[i] = encrypt;
            }
            return result;
        }
        public static string Decrypt(int[] crypto, string key, int size)
        {
            int encrypt;
            string result = "";
            for (int i = 0; i < size; i++)
            {
                int a = crypto[i];
                int b = key[i];
                encrypt = a ^ b;
                result += Convert.ToChar(encrypt);
            }
            return result;
        }
        public static string[] SpliceText(string text, int lineLength)
        {
            return Regex.Matches(text, ".{1," + lineLength + "}").Cast<Match>().Select(m => m.Value).ToArray();
        }
        static void Main(string[] args)
        {
            string OrigString;
            string key;
            string path = @"plain.txt";
            OrigString = System.IO.File.ReadAllText(@"orig.txt").ToLower();
            key = System.IO.File.ReadAllText(@"key.txt").ToLower();
            string[] PlainString = SpliceText(OrigString, 64);
            if (args[0] == "-p")
            {
                File.WriteAllLines(path, PlainString);
                Console.WriteLine("Sformatowano tekst i zapisano do pliku plain.txt");
                Console.WriteLine("");
                string result = System.IO.File.ReadAllText(@"plain.txt").ToLower();
                Console.WriteLine(result);
            }
            string test = PlainString[0];
            int size = test.Length;
            int linecount = (LineCount());
            if (args[0] == "-e")
            {
                string sum = "";
                string wydruk = "";
                Console.WriteLine("Tekst zaszyfrowany:");
                Console.WriteLine("");
                for (int i = 0; i < linecount; i++)
                {
                    if (i == 0)
                    {
                        int[] result = Encrypt(PlainString[i], key, size);
                        sum = string.Join(",", result);
                        wydruk = string.Join("", result);
                        File.WriteAllText(@"crypto.txt", sum.ToString());
                        Console.WriteLine(wydruk);
                    }
                    else
                    {
                        int[] result = Encrypt(PlainString[i], key, size);
                        sum = string.Join(",", result);
                        wydruk = string.Join("", result);
                        string appendText = Environment.NewLine + string.Join(",", result);
                        File.AppendAllText(@"crypto.txt", appendText);
                        Console.WriteLine(wydruk);
                    }
                }
            }
            if (args[0] == "-k")
            {
                string crypto = System.IO.File.ReadAllText(@"crypto.txt");
                string[] cryptobox = new string[linecount];
                //////////////////////////////////////////////////////////////////////zczytwanie tekstu zaszyfrowanego
                using (StringReader reader = new StringReader(crypto))
                {
                    int i = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        cryptobox[i] = line;    
                        i++;
                    }
                }
                //////////////////////////////////////////////////////////////////krypto analiza
                int[] klucz = new int[64];
                string separator = cryptobox[0];
                int[] ia = separator.Split(',').Select(n => Convert.ToInt32(n)).ToArray(); // rodziela stringa z liczbami na tablice intow !!!!
                int[] keycode = KeyBreake(ia);
                for (int i = 0; i < 64; i++)
                {
                    for (int j = 0; j < 21; j++)
                    {

                        string separator1 = cryptobox[j + 1];
                        int[] ib = separator1.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                        int[] keycode1 = KeyBreake(ib);
                        if (keycode[i] == 0)
                        {
                            keycode[i] = keycode1[i];

                        }
                        else
                        {

                        }
                    }
                }
                string WYNIK = "";
                Random rnd = new Random();
                for (int i = 0; i < 64; i++)
                    
                {
                    if (keycode[i] == 0)
                    {
                        int value = rnd.Next(97, 125);
                        keycode[i] = value;
                    }
                }
                for (int i = 0; i < 64; i++)
                {
                    WYNIK += Convert.ToChar(keycode[i]);
                }
                string res = string.Join(" ", keycode);
                /////////////////////////////////////drukowanie 
                Console.WriteLine("obliczony klucz kryptoanaliza:   {0}", WYNIK);
                Console.WriteLine("orgyignalny klucz:               {0}", key);
                Console.WriteLine("");
                ///////////////////////////////////////////////////////////////// dekodwanie calego tekstu 
                for (int i = 0; i < linecount; i++)
                {
                    if (i == 0)
                    {
                        string decrypt = cryptobox[i];
                        int[] ic = decrypt.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                        File.WriteAllText(@"decrypt.txt", Decrypt(ic, WYNIK, size));
                        Console.WriteLine(Decrypt(ic, WYNIK, size));
                    }
                    else
                    {
                        string decrypt = cryptobox[i];
                        int[] ic = decrypt.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                        string appendText = Environment.NewLine + Decrypt(ic, WYNIK, size);
                        File.AppendAllText(@"decrypt.txt", appendText);
                        Console.WriteLine(Decrypt(ic, WYNIK, size));
                    }
                }
            }
        }
    }
}

