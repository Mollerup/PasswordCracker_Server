using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PasswordCracker_Server;

namespace PasswordCracker_Server
{
    class ConnectionService
    {
        TcpClient client;
        TcpListener connectionSocket;

        public ConnectionService(TcpListener _connectionSocket)
        {
            connectionSocket = _connectionSocket;
            client = connectionSocket.AcceptTcpClient();
            Console.WriteLine("Client accepted...");
        }

        public void Service()
        {
            Stream ns = client.GetStream();
            StreamWriter sw = new StreamWriter(ns);
            StreamReader sr = new StreamReader(ns);
            sw.AutoFlush = true;

            while (true)
            {
                try
                {
                    var msg = sr.ReadLine();

                    switch (msg)
                    {
                        case "-getpassword":
                            var pass = GetPassword();
                            sw.WriteLine("password");
                            Console.WriteLine("Sending password file to client...");
                            sw.WriteLine(pass);
                            break;

                        case "-getwordlist":
                            sw.WriteLine("wordlist");
                            Console.WriteLine("Sending wordlist part to client...");
                            sw.WriteLine(GetWordListPart());
                            break;

                        case "-getinfo":
                            string result = null;
                            foreach (var v in ServerProgram.resultUserInfo)
                            {
                                result += v.ToString() + "_";
                            }

                            if (string.IsNullOrEmpty(result))
                            {
                                sw.WriteLine("No results so far...");
                            }
                            else
                            {
                                sw.WriteLine(result); 
                            }

                            break;

                        case "result":
                            var cResult = sr.ReadLine();

                            if (!string.IsNullOrEmpty(cResult))
                            {
                                var sResult = cResult.Split('_');
                                foreach (var v in sResult)
                                {
                                    if (!string.IsNullOrEmpty(v))
                                    {
                                        var x = v.Split(':');
                                        ServerProgram.resultUserInfo.Add(new UserInfoClearText(x[0], x[1]));
                                        
                                    }
                                }
                                Console.WriteLine("Result so far:");
                                foreach(var v in ServerProgram.resultUserInfo)
                                {
                                    Console.WriteLine(v);
                                }
                            }
                            break;

                        case "-exit":
                            client.Close();
                            connectionSocket.Stop();
                            ns.Close();
                            sw.Close();
                            sr.Close();
                            Console.WriteLine("A client closed the Program...");
                            break;
                        default:
                            sw.WriteLine("Unknown commando. Type -help for help. ");
                            break;
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("A client closed the Program...");
                    break;
                }
            }
        }

        public string GetPassword()
        {
            string password = null;

            foreach (var vv in ServerProgram.UserInfo)
            {
                password += vv.ToString() + "_";
            }

            return password;
        }

        public string GetWordListPart()
        {
            string wordlist = null;
            var index = ServerProgram.wordlistIndexer;
            ServerProgram.wordlistIndexer++;
            var list = ServerProgram.DictionaryParts.ToList()[index];
            foreach (var v in list)
            {
                wordlist += v + "_";
            }
            return wordlist;
        }


    }
}
