using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PasswordCracker_Client;
using PasswordProject;

namespace PasswordClient
{

    class Program
    {
        public static StreamReader sr;
        public static StreamWriter sw;
        public static List<UserInfo> UserInfo = new List<UserInfo>();
        public static List<string> WordListPart = new List<string>();

        static void Main(string[] args)
        {
            TcpClient socket = new TcpClient("localhost", 6789);
            Stream ns = socket.GetStream();

            Console.WriteLine("Server connected");

            sr = new StreamReader(ns);
            sw = new StreamWriter(ns);

            while (true)
            {
                var msg = Console.ReadLine();

                if (msg == "-exit")
                {
                    sw.WriteLine("exit");
                    Console.WriteLine("Shutting down...");
                    break;
                }
                else if (msg == "-crack")
                {
                    sw.WriteLine(msg);
                    GetPassword();
                    Console.WriteLine("Getting passwords");
                    GetWordListPart();
                    Cracking cracking = new Cracking(UserInfo, WordListPart);
                    var passwords = cracking.RunCracking();
                    sw.WriteLine("result");
                    sw.WriteLine(passwords);
                }
            }
        }

        public static void GetPassword()
        {
            if (UserInfo.Count == 0)
            {
                Console.WriteLine();
                var passwordfile = sr.ReadLine();
                var passfile = passwordfile.Split('_');

                foreach (var v in passfile)
                {
                    if (!string.IsNullOrEmpty(v))
                    {
                        var splitted = v.Split(':');
                        UserInfo.Add(new UserInfo(splitted[0], splitted[1]));
                        Console.WriteLine(v);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Passwords recieved...");
            }
            else
            {
                sr.ReadLine();
                Console.WriteLine("You already have the password file...");
            }
        }

        public static void GetWordListPart()
        {
            if (WordListPart.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Recieving wordlist part. This may take a while...");
                var wordlist = sr.ReadLine();
                var wl = wordlist.Split('_');

                foreach (var v in wl)
                {
                    if (!string.IsNullOrEmpty(v))
                    {
                        WordListPart.Add(v);
                    }
                }

                Console.WriteLine("Wordlist recieved...");
                Console.WriteLine("Wordlist count: " + WordListPart.Count());
            }
            else
            {
                Console.WriteLine("You already have the wordlist part...");
            }

        }

    }
}
