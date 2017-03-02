using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedPasswordCracker_Server.Handlers
{
    class DictionaryFileHandler
    {
        public static List<string> ReadDictionary(String filename)
        {
            List<string> Dictionary = new List<string>();

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    Dictionary.Add(sr.ReadLine());
                }
                return Dictionary;
            }
        }
    }
}
