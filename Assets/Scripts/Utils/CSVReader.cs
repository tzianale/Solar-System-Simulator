using System.IO;
using UnityEngine;
using System.Collections.Generic;


namespace Utils
{
    public class CsvReader : MonoBehaviour
    {
        public static List<List<string>> ReadCsv(string path)
        {
            var table = new List<List<string>>();
            
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    
                    table.Add(new List<string>(values));
                }
                
                reader.Close();
            }

            return table;
        }
    }
}