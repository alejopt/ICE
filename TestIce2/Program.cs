using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestIce2
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string Source1 =  ConfigurationManager.AppSettings["Source1"].ToString();
            string Source2 = ConfigurationManager.AppSettings["Source2"].ToString();           
            byte[] S1= ReadMemoryMappedFile(Source1);
            byte[] S2 = ReadMemoryMappedFile(Source2);
            var r= Merge(S1, S2);
            CreateNewFile(r);

        }
       
        static byte[] ReadMemoryMappedFile(string fileName)
        {
            long length = new FileInfo(fileName).Length;
            using (var stream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var mmf = MemoryMappedFile.CreateFromFile(stream, null, length, MemoryMappedFileAccess.Read, null, HandleInheritability.Inheritable, false))
                {
                    using (var viewStream = mmf.CreateViewStream(0, length, MemoryMappedFileAccess.Read))
                    {
                        using (BinaryReader binReader = new BinaryReader(viewStream))
                        {
                            var result = binReader.ReadBytes((int)length);
                            return result;
                        }
                    }
                }
            }
        }

        static IOrderedEnumerable<string> Merge(byte[] S1, byte[] S2)
        {
          
            try
            {
                string [] a= Encoding.UTF8.GetString(S1).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                string[] b =Encoding.UTF8.GetString(S2).Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                var list = new List<string>();
                list.AddRange(a);
                list.AddRange(b);
                list.Sort();
                var ordered = list.OrderBy(s => int.Parse(s.ToString()));

                return ordered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static void CreateNewFile(IOrderedEnumerable<string> Lists)
        {
            try
            {

                System.IO.File.WriteAllLines(string.Concat(ConfigurationManager.AppSettings["Result"].ToString(), @"\result.txt"), Lists);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
   
}
