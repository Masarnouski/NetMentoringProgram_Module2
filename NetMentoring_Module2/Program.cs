using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMentoring_Module2
{

    class Program
    {
        static void Main(string[] args)
        {
            //Func<string, bool> sortingDelegate = (directory) =>
            //{
            //    if (directory.Length > 40)
            //        return false;
            //    else
            //        return true;
            //};

            //To test with Func
                   
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"D:\TestCatalog");

            //FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"D:\TestCatalog", sortingDelegate);
            fileSystemVisitor.Start += Show_Message;
            fileSystemVisitor.Finish += Show_Message;
            fileSystemVisitor.FileFinded += Show_Message;
            fileSystemVisitor.DirectoryFinded += Show_Message;
            fileSystemVisitor.FilteredFileFinded += Show_Message;
            fileSystemVisitor.FilteredDirectoryFinded += Show_Message;


            fileSystemVisitor.GetFiles();
            //Here you can try to itrerate the object

            foreach (var file in fileSystemVisitor)
            {
                Console.WriteLine(file);
            }

            Console.ReadLine();

        }
        readonly static List<string> directoriesToExclude = new List<string>()
        {
            @"D:\TestCatalog\TestCatalog_1\TestCatalog_1_2",
            @"D:\TestCatalog\TestCatalog_1\TestCatalog_1_3",
        };

        static void Show_Message(object sender, ProcessEventArgs e)
        {
            // if you want to exclude some directories

            //foreach (string directory in directoriesToExclude)
            //{
            //    if (e.Directory.Contains(directory))
            //        e.IsExcluded = true;
            //}

            //if you want to return after some messages

            //if (e.Count >= 5)
            //{
            //    e.IsCancelled = true;
            //}
            Console.WriteLine(e.Message + e.Directory);
        }
    }
}
