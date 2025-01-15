using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    class Program
    {
        public static Random rnd = new Random();

        public static void Main(string[] args)
        {
            string outputType = string.Empty;
            string path = string.Empty;
            int numberOfRecords = 0;
            int startId = 0;
            

            if (args.Length > 0)
            {
                if (args.Contains("--output-type=csv"))
                {
                    outputType = "csv";
                }
                else if (args.Contains("--output-type=xml"))
                {
                    outputType = "xml";
                }
                else if (args.Contains("-t"))
                {
                    int index = Array.IndexOf(args, "-t");
                    if (args[index + 1].Equals("xml", StringComparison.Ordinal) || args[index + 1].Equals("csv", StringComparison.Ordinal))
                    {
                        outputType = args[index + 1];
                    }
                }

                if (args.Where(str => str.StartsWith("--output=")).ToArray().Length != 0)
                {
                    string[] array = args.Where(str => str.StartsWith("--output=")).ToArray();
                    

                    foreach (string str in array)
                    {
                        string temp = str.Split('=')[1];
                        if (temp.StartsWith("C:\\Users\\kvuso\\.vscode\\newtry\\new-try\\FileCabinetGenerator\\bin\\Debug\\net8.0"))
                        {
                            path = temp.Split('\\')[temp.Split('\\').Length - 1];
                        }
                        else
                        {
                            path = temp;
                        }
                    }
                }
                else if (args.Contains("-o"))
                {
                    int index = Array.IndexOf(args, "-o");

                    if (args[index + 1].StartsWith("C:\\Users\\kvuso\\.vscode\\newtry\\new-try\\FileCabinetGenerator\\bin\\Debug\\net8.0"))
                    {
                        path = args[index + 1].Split("\\")[args[index + 1].Split("\\").Length - 1];
                    }
                    else
                    {
                        path = args[index + 1];
                    }
                }

                if (args.Where(str => str.StartsWith("--records-amount")).ToArray().Length != 0)
                {
                    string[] array = args.Where(str => str.StartsWith("--records-amount")).ToArray();

                    foreach(string str in array)
                    {
                        int.TryParse(str.Split('=')[1], out numberOfRecords);
                    }
                }
                else if (args.Contains("-a"))
                {
                     int index = Array.IndexOf(args, "-a");
                     int.TryParse(args[index + 1], out numberOfRecords);
                }

                if (args.Where(str => str.StartsWith("--start-id")).ToArray().Length != 0)
                {
                    string[] array = args.Where(str => str.StartsWith("--start-id")).ToArray();

                    foreach (string str in array)
                    {
                        int.TryParse(str.Split('=')[1], out startId);
                    }
                }
                else if (args.Contains("-i"))
                {
                    int index = Array.IndexOf(args, "-i");
                    int.TryParse(args[index + 1], out startId);
                }
            }

            if (outputType.Equals("csv", StringComparison.Ordinal))
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i < numberOfRecords; i++)
                    {
                        string record = $"{startId++},{CreateRandomString()},{CreateRandomString()},{CreateRandomDateOfBirth()},{CreateRandomAge()},{CreateRandomFavouriteNumeral()},{CreateRandomIncome()}";
                        writer.WriteLine(record);
                    }
                }
            }
            else
            {
                Records records = new Records();
                records.RecordList = new List<FileCabinetGeneratorRecord>();
                for (int i = 0; i < numberOfRecords; i++)
                {
                    FileCabinetGeneratorRecord record = new FileCabinetGeneratorRecord()
                    {
                        Id = startId++,
                        RecordName = new Name { FirstName = CreateRandomString(), LastName = CreateRandomString() },
                        DateOfBirth = CreateRandomDateOfBirth(),
                        Age = CreateRandomAge(),
                        FavouriteNumeral = CreateRandomFavouriteNumeral(),
                        Income = CreateRandomIncome(),
                    };
                    records.RecordList.Add(record);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Records));

                using (StreamWriter writ = new StreamWriter(path))
                {
                    using (XmlWriter writer = new XmlTextWriter(writ))
                    {
                        serializer.Serialize(writer, records);
                    }
                }
            }
        }

        private static string CreateRandomString()
        {
            string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            
            StringBuilder stringBuilder = new StringBuilder();
            int position = 0;
            int length = rnd.Next(3, 59);
            for (int i = 0; i < length; i++)
            {
                position = rnd.Next(0, Alphabet.Length - 1);
                stringBuilder.Append(Alphabet[position]);
            }

            return stringBuilder.ToString();    
        }

        private static short CreateRandomAge()
        {
            return (short)rnd.Next(1, 99);
        }

        private static DateTime CreateRandomDateOfBirth()
        {
            int year = rnd.Next(1950, DateTime.Now.Year - 1);
            int month = rnd.Next(1, 12);
            int day = rnd.Next(1, 31);
            return new DateTime(year, month, day);
        }

        private static char CreateRandomFavouriteNumeral()
        {
            return (char)rnd.Next(49, 57);
        }

        private static decimal CreateRandomIncome()
        {
            return (decimal)rnd.Next(350, 2000000);
        }
    }
}