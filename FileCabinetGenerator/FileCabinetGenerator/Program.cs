
namespace FileCabinetApp
{
    class Program
    {
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

            Console.WriteLine($"{numberOfRecords} records were written to {path}");
        }
    }
}