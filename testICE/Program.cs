using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace testICE
{
    class Program
    {
        static List<CusIP> ObjCusIp = new List<CusIP>();
        static void Main(string[] args)
        {
            CreateMapped();
            PrintResult();


            Console.ReadLine();
        }

        private static async void CreateMapped()
        {

             long MMF_MAX_SIZE = 1024;  // allocated memory for this memory mapped file (bytes)
            const int MMF_VIEW_SIZE = 1024; // how many bytes of the allocated memory can this process access
            try
            {
                const string fileName = @"c:\users\apaez\desktop\testice.txt";

               
                FileInfo f = new FileInfo(fileName);
                MMF_MAX_SIZE = f.Length;
                using (MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(fileName, FileMode.OpenOrCreate, "test-map"))
                {
                    using (MemoryMappedViewAccessor viewAccessor = memoryMappedFile.CreateViewAccessor())
                    {

                        byte[] bytes = new byte[MMF_MAX_SIZE];
                        viewAccessor.ReadArray(0, bytes, 0, bytes.Length);
                        string text = Encoding.UTF8.GetString(bytes).Trim('\0');
                        GetPriceByCUSIP(text);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    
        
        private static void GetPriceByCUSIP(string Result)
        {
            try
            {
                string[] lines = Result.Split( new[] { Environment.NewLine },   StringSplitOptions.None);
                CusIP PricesCusIP = new CusIP();
                double Price ;
                for (int a = 0; a < lines.Length; a++)
                {
                    if (lines[a].Length == 8 && double.TryParse(lines[a],out Price) ==false)
                    {
                        if (PricesCusIP.CusIp==null)
                        { 
                        PricesCusIP.CusIp = lines[a].ToString();
                        }
                        else
                        {
                            
                            ObjCusIp.Add(PricesCusIP);
                            PricesCusIP = new CusIP();
                            PricesCusIP.CusIp = lines[a].ToString();
                        }
                    }
                    else
                    {
                        PricesCusIP.Price.Add(decimal .Parse(lines[a].ToString()));
                    }
                }
                ObjCusIp.Add(PricesCusIP);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void PrintResult()
        {
            try
            {
                foreach (var i in ObjCusIp)
                {
                    Console.WriteLine($"CUSIP :{ i.CusIp}, Last Price: { i.Price.Last().ToString() }");
                    

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
