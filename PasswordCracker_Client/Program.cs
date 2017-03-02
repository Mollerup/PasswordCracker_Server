using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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


        }
    }
}
