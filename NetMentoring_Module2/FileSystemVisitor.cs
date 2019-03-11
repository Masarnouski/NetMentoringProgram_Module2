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
            GetFiles(_directoryName);
        }

        #region private methods

        private void GetFiles(string directoryName)
        {
            if (!Directory.Exists(directoryName)) return;
            var subDirectories = Directory.GetDirectories(directoryName);
              
            if (subDirectories.Length <= 0)
                return;
            foreach (var subDirectory in subDirectories)
            {
                if (_filterPredicate != null)
                {
                    if (!_filterPredicate(subDirectory)) continue;
                        Result.Add(subDirectory);
                        _count++;
                        FilteredDirectoryFinded?.Invoke(this, new ProcessEventArgs("[FilteredDirectoryFinded]", Count, subDirectory));
                }
                else
                {
                    Result.Add(subDirectory);
                    _count++;
                    DirectoryFinded?.Invoke(this, new ProcessEventArgs("[DirectoryFinded] : ", Count, subDirectory));
                }

                var files = Directory.GetFiles(subDirectory);                   
                     
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        if (_filterPredicate != null)
                        {
                            if (!_filterPredicate(file)) continue;
                            Result.Add(subDirectory);
                            _count++;
                            DirectoryFinded?.Invoke(this,
                                new ProcessEventArgs("[DirectoryFinded] : ", Count, subDirectory));
                        }
                        else
                        {
                            Result.Add(file);
                            _count++;
                            FileFinded?.Invoke(this, new ProcessEventArgs("[FileFinded] : ", Count, file));
                        }


                    }
                }
                GetFiles(subDirectory);
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
