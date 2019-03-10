using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetMentoring_Module2
{
    public delegate void ProcessStateHandler(object sender, ProcessEventArgs e);

    public class FileSystemVisitor : IEnumerable
    {
        public event ProcessStateHandler Start;
        public event ProcessStateHandler Finish;
        public event ProcessStateHandler FileFinded;
        public event ProcessStateHandler DirectoryFinded;
        public event ProcessStateHandler FilteredFileFinded;
        public event ProcessStateHandler FilteredDirectoryFinded;
        //private readonly List<string> Result;
        private readonly string _directoryName;
        private readonly Func<string, bool> _filterPredicate;
        private int _count;

        public int Count { get { return _count; } }
        public List<string> Result { get; }


        #region constructors
        public FileSystemVisitor(string directoryName, Func<string, bool> filterPredicate)
        {
            Result = new List<string>();
            _filterPredicate = filterPredicate;
            _directoryName = directoryName;
        }

        public FileSystemVisitor(string directoryName)
        {
            Result = new List<string>();
            _directoryName = directoryName;
        }
        #endregion

        public void GetFiles()
        {
            if (Result.Count > 0)
            {
                Result.Clear();
                _count = 0;
            }
            if (_filterPredicate != null)
            {
                GetFiles(_directoryName, _filterPredicate);
            }
            else
            {
                GetFiles(_directoryName);
            }
        }

        #region private methods
        private void GetFiles(string directoryName, Func<string,bool> filterPredicate)
        {
            if (Directory.Exists(directoryName))
            {

                string[] subDirectories = Directory.GetDirectories(directoryName);

                if (subDirectories.Length <= 0 || subDirectories == null)
                    return;
                foreach (string subDirectory in subDirectories)
                {
                    if (filterPredicate(subDirectory))
                    {
                        Result.Add(subDirectory);
                        _count++;
                        FilteredDirectoryFinded?.Invoke(this, new ProcessEventArgs("[FilteredDirectoryFinded]", Count, subDirectory));
                    }
                    string[] files = Directory.GetFiles(subDirectory);
                    if (files.Length > 0 && files != null)
                    {
                        foreach (string file in files)
                        {
                            if (filterPredicate(file))
                            {
                                Result.Add(file);
                                _count++;
                                FilteredFileFinded?.Invoke(this, new ProcessEventArgs("[FilteredFileFinded]", Count, file));
                            }
                        }
                    }
                    GetFiles(subDirectory,filterPredicate);
                }
            }
        }

        private void GetFiles(string directoryName)
        {
            if (Directory.Exists(directoryName))
            {
                
                string[] subDirectories = Directory.GetDirectories(directoryName);
              
                if (subDirectories.Length <= 0 || subDirectories == null)
                    return;
                foreach (string subDirectory in subDirectories)
                {
                    
                    Result.Add(subDirectory);
                    _count++;
                    DirectoryFinded?.Invoke(this, new ProcessEventArgs("[DirectoryFinded] : ", Count, subDirectory));
                    string[] files = Directory.GetFiles(subDirectory);
                    if (files.Length > 0 && files != null)
                    {
                        foreach (string file in files)
                        {
                            Result.Add(file);
                            _count++;
                            FileFinded?.Invoke(this,new ProcessEventArgs("[FileFinded] : ", Count, file));
                        }
                    }
                    GetFiles(subDirectory);
                }
            }
        }
        #endregion

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Result.Count; i++)
            {
                yield return Result[i];
            }
        }
    }
}
