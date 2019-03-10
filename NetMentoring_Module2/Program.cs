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
            Func<string, bool> sortingDelegate = (directory) =>
            {
                if (directory.Length > 45)
                    return false;
                else
                    return true;
            };

            //To test with Func
                   
            //FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"C:\TestCatalog", sortingDelegate);

            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"C:\TestCatalog");
            fileSystemVisitor.Start += Show_Message;
            fileSystemVisitor.Finish += Show_Message;
            fileSystemVisitor.FileFinded += Show_Message;
            fileSystemVisitor.DirectoryFinded += Show_Message;
            fileSystemVisitor.FilteredFileFinded += Show_Message;
            fileSystemVisitor.FilteredDirectoryFinded += Show_Message;


            fileSystemVisitor.GetFiles();
            //Here you can try to itrerate the object

            //foreach (var file in fileSystemVisitor)
            //{
            //    Console.WriteLine(file);
            //}

            Console.ReadLine();

        }
        readonly static List<string> directoriesToExclude = new List<string>()
        {
            @"C:\TestCatalog\Catalog_1\Catalog_1_3\Catalog_1_3_3",
            @"C:\TestCatalog\Catalog_1\Catalog_1_3\Catalog_1_3_4",
            @"C:\TestCatalog\Catalog_2",
        };

        static void Show_Message(object sender, ProcessEventArgs e)
        {
            // if you want to exclude some directories

            //foreach (string directory in directoriesToExclude)
            //{
            //    if (e.Directory.Contains(directory))
            //        return;
            //}

            //if you want to return after some messages

            //if (e.Count >= 7)
            //{
            //    return;
            //}
            Console.WriteLine(e.Message + e.Directory);
        }
    }
}
