using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RoszdravGoodsCSVtoSQL
{
    class Program
    {
        static void Main(string[] args)
        {
        
            if (args.Length !=1)
            {
                Console.WriteLine("Передайте в параметр запуска назыание CSV  файла выгрузки");
                return;
            }

            string fName = args[0];

            if (!File.Exists(fName))
            {
                Console.WriteLine("Нет такого файла.");
                return;
            }

            BuildInsert(fName);

        }
        static void BuildInsert (string fName)
        {
            string csvHeader = "";
            List<string> csvBody = new List<string>();
            csvBody = File.ReadAllLines(fName, Encoding.GetEncoding(1251)).ToList();
            if (csvBody.Count<2)
            {
                Console.WriteLine("Мало строк в файле");
                return;
            }

            
            csvHeader = csvBody.ElementAt(0);
            csvBody.RemoveAt(0);
            int sizeofheader = csvHeader.Split(';').Length;
            string[] header = csvHeader.Split(';');


            StringBuilder resultsql = new StringBuilder();
            foreach (string s in csvBody)
            {
                bool is_cooorect = true;
                string sanitizedString = s.Replace("'","");
                sanitizedString = sanitizedString.Replace("\"", "");
                string[] splitted = sanitizedString.Split(';');
                if (splitted.Length != sizeofheader)
                {
                    is_cooorect = false;
                    continue;
                }
                StringBuilder insertstring = new StringBuilder();
                insertstring.Append("insert into d2a_zhnvls_roszdrav (");
                

                foreach (string h in header)
                {
                    insertstring.Append(h);
                    insertstring.Append(",");
                }
                insertstring.Remove(insertstring.Length - 1, 1);
                insertstring.Append(") values (");
                foreach (string spvalue in splitted)
                {
                    insertstring.Append("'" + spvalue + "' ,");
                }
                insertstring.Remove(insertstring.Length - 1, 1);
                insertstring.Append(");");
                if (is_cooorect)
                    resultsql.AppendLine(insertstring.ToString());
            }

            File.WriteAllText("result.sql", resultsql.ToString());

        }
    }
}
