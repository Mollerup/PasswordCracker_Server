using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;
using DistributedPasswordCracker_Server.Handlers;

namespace DistributedPasswordCracker_Server
{
    class ServerProgram
    {
        public static List<Task> taskList;
        public static List<UserInfo> UserInfo;
        public static List<string> Dictionary { get; set; }
        public static IEnumerable<IEnumerable<string>> DictionaryParts;
        public static int ClientsCount;
        static TcpListener connectionSocket;
        public static int wordlistIndexer = 0;
        public static List<UserInfoClearText> resultUserInfo;


        static void Main(string[] args)
        {
            connectionSocket = new TcpListener(6789);
            connectionSocket.Start();
            Console.WriteLine("Server started...");

            GetPasswordAndWordlist();

            Console.WriteLine();
            Console.WriteLine("Write numbers of clients: ");
            ClientsCount = int.Parse(Console.ReadLine());

            resultUserInfo = new List<UserInfoClearText>();
            taskList = new List<Task>();
            DictionaryParts = new List<List<string>>();

            Console.WriteLine("Dividing wordlist into " + ClientsCount + " chunks...");
            DictionaryParts = DivideWordList(Dictionary, ClientsCount);
            Console.WriteLine("Chunks count: ");

            foreach (var v in DictionaryParts)
            {
                Console.WriteLine(v.Count());
            }

            Console.WriteLine();

            while (true)
            {
                ConnectionService cs = new ConnectionService(connectionSocket);
                var t = Task.Factory.StartNew(cs.Service);
                taskList.Add(t);
                CheckAlive();
                Console.WriteLine(taskList.Count + " clients are connected...");
            }
        }

        public static void CheckAlive()
        {
            var taskToRemove = taskList.FindAll(x => x.IsCanceled == true || x.IsCompleted == true || x.IsFaulted == true).ToList();

            foreach (var t in taskToRemove)
            {
                taskList.Remove(t);
            }
        }

        public static void GetPasswordAndWordlist()
        {
            Console.WriteLine();
            Console.WriteLine("Loading password file...");
            UserInfo = PasswordFileHandler.ReadPasswordFile("passwords.txt");
            Console.WriteLine("Password file loaded sucessfull...");
            Console.WriteLine();
            Console.WriteLine("Loading dictionary... ");
            Dictionary = DictionaryFileHandler.ReadDictionary("webster-dictionary.txt");
            Console.WriteLine("Dictionary count: " + Dictionary.Count);
            Console.WriteLine("Dictionary loaded sucessfull...");
        }

        public static IEnumerable<IEnumerable<string>> DivideWordList(List<string> source, int numberOfClient)
        {
            int i = 0;
            var splits = from item in source
                         group item by i++ % numberOfClient into part
                         select part.ToList();
            return splits;
        }
    }
}
