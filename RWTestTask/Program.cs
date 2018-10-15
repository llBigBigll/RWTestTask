using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace RWTestTask
{

    class Program
    {
        static int NumOfLines(string filename)
        {
            using (StreamReader strm = new StreamReader(filename))
            {
                int i = 0;
                while (!strm.EndOfStream)
                {
                    string a = strm.ReadLine();
                    i++;
                }
                return i;
            }
        }
        static char[] InvalidLetters()
        { List<char> chars = new List<char>();
            for (char i = char.MinValue; i < char.MaxValue; i++)
            {
                if (!((i >= '0' && i <= '9') || (i >= 'A' && i <= 'Z') || (i >= 'a' && i < 'z')))
                {
                    chars.Add(i);
                }
            }

            return chars.ToArray();
        }
        static Dictionary<string, uint> TaskWork(string str, char[] chr)
        {
            Dictionary<string, uint> dic = new Dictionary<string, uint>();
            if (str != null)
            {
                string[] wordsarr = str.Split(chr, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in wordsarr)
                {

                    if (dic.ContainsKey(word.ToLower()))
                    {
                        dic[word.ToLower()] += 1;

                    }
                    else
                    {
                        dic.Add(word.ToLower(), 1);
                    }
                }
            }
            return dic;
        }
    

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            SortedDictionary<string, uint> WordsList = new SortedDictionary<string, uint>();
            string FileName = "C:/Users/nikit/source/repos/llBigBigll/RWTestTask/RWTestTask/WarAndPeace.txt";
            string FileOfAnswer = "C:/Users/nikit/source/repos/llBigBigll/RWTestTask/RWTestTask/Answer.txt";
            if (NumOfLines(FileName) > 1000)
            {
                int linesForOneTask = NumOfLines(FileName) / 12;


                string[] parts = new string[10];

                using (StreamReader filestream = new StreamReader(FileName))
                {
                    int i = 0;
                    int counter = 0;

                    while (!filestream.EndOfStream)
                    {

                        string line = filestream.ReadLine();
                        parts[i] += line;
                        counter++;

                        if (counter >= linesForOneTask && i <= 8)
                        {
                            counter = 0;
                            i++;
                        }
                    }
                }


                char[] invchar = InvalidLetters();
                Task<Dictionary<string, uint>>[] tasks = { Task<Dictionary<string, uint>>.Factory.StartNew(() => TaskWork((parts[0]), invchar)),
                Task<Dictionary<string, uint>>.Factory.StartNew(() => TaskWork((parts[1]), invchar)),
                Task<Dictionary<string, uint>>.Factory.StartNew(() => TaskWork((parts[2]), invchar)),
                Task<Dictionary<string, uint>>.Factory.StartNew(() => TaskWork((parts[3]), invchar)),
                Task<Dictionary<string, uint>>.Factory.StartNew(() => TaskWork((parts[4]), invchar)),
                Task<Dictionary<string, uint>>.Factory.StartNew(() => TaskWork((parts[5]), invchar)),
                Task<Dictionary<string, uint>>.Factory.StartNew(() => TaskWork((parts[6]), invchar)),
                Task<Dictionary<string, uint>>.Factory.StartNew(() => TaskWork((parts[7]), invchar)),
                Task<Dictionary<string, uint>>.Factory.StartNew(() => TaskWork((parts[8]), invchar)),
                Task<Dictionary<string, uint>>.Factory.StartNew(() => TaskWork((parts[9]), invchar))};
                Task.WaitAll(tasks);



                for (int i = 0; i < tasks.Length; i++)
                {

                    foreach (KeyValuePair<string, uint> pair in tasks[i].Result)
                    {

                        if (WordsList.ContainsKey(pair.Key))
                        {
                            WordsList[pair.Key] += pair.Value;
                        }
                        else
                        {
                            WordsList.Add(pair.Key, pair.Value);
                        }
                    }
                }
            }
            else
            {
                using (StreamReader filestream = new StreamReader(FileName))
                {
                    while (!filestream.EndOfStream)
                    {

                        string line = filestream.ReadLine();
                        if (line != null)
                        {

                            var wordsarr = line.Split(InvalidLetters(), StringSplitOptions.RemoveEmptyEntries);
                            foreach (string word in wordsarr)
                            {
                                if (WordsList.ContainsKey(word.ToLower()))
                                {
                                    WordsList[word.ToLower()] += 1;

                                }
                                else
                                {
                                    WordsList.Add(word.ToLower(), 1);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
            }
            Console.WriteLine("Файл успешно прочтен");
            using (StreamWriter filestream = new StreamWriter(FileOfAnswer))
            {
                foreach (KeyValuePair<string, uint> word in WordsList)
                {
                    filestream.WriteLine(word.Key + ": "+ word.Value.ToString());
                }
            }
            Console.WriteLine("Ответ записан");


            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
            Console.ReadKey();
        }
    }
}
