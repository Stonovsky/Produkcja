using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.Utilities.Wrappers
{
    public class DirectoryInfoWrapper : IDirectoryInfoWrapper
    {

        public DirectoryInfoWrapper()
        {
        }

        public DirectoryInfo CreateDirectory(string directoryPath)
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory(directoryPath);
            return directoryInfo;
        }

        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
    }
}
