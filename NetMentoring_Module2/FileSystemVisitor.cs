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
        private bool _isCancelled;
        private int _count;

        public int Count => _count;
        public List<string> Result { get; }


        #region constructors
        public FileSystemVisitor(string directoryName, Func<string, bool> filterPredicate)
        {
            Result = new List<string>();
            _filterPredicate = filterPredicate;
            _directoryName = directoryName;
        }

        public FileSystemVisitor(string directoryName): this(directoryName, null)
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
            Start?.Invoke(this, new ProcessEventArgs("The Program has started", Count, null, false, false));
            GetFiles(_directoryName);
            Start?.Invoke(this, new ProcessEventArgs("The Program has finished", Count, null, false, false));
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
                    if (_isCancelled) return;
                    if (!_filterPredicate(subDirectory)) continue;
                    var processEventArgs = new ProcessEventArgs("[FilteredDirectoryFinded]", _count, subDirectory, false, false);
                    _count++;
                    FilteredDirectoryFinded?.Invoke(this, processEventArgs);
                    if(!processEventArgs.IsExcluded) Result.Add(subDirectory);
                   
                }
                else
                {
                    if (_isCancelled) return;
                    _count++;
                    var processEventArgs = new ProcessEventArgs("[DirectoryFinded] : ", _count, subDirectory, false, false);   
                    DirectoryFinded?.Invoke(this, processEventArgs);
                    if (!processEventArgs.IsExcluded) Result.Add(subDirectory);
                    _isCancelled = processEventArgs.IsCancelled;
                }

                var files = Directory.GetFiles(subDirectory);                   
                     
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        if (_isCancelled) return;
                        if (_filterPredicate != null)
                        {
                            if (!_filterPredicate(file)) continue;
                            _count++;
                            var processEventArgs = new ProcessEventArgs("[DirectoryFinded] : ", _count, subDirectory, false, false);
                            DirectoryFinded?.Invoke(this, processEventArgs);
                            if (!processEventArgs.IsExcluded) Result.Add(subDirectory);
                            _isCancelled = processEventArgs.IsCancelled;
                        }
                        else
                        {
                            if (_isCancelled) return;
                            _count++;
                            var processEventArgs = new ProcessEventArgs("[FileFinded] : ", _count, file, false, false);
                            FileFinded?.Invoke(this, processEventArgs);
                            if (!processEventArgs.IsExcluded) Result.Add(file);
                            _isCancelled = processEventArgs.IsCancelled;
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
