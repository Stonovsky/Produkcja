using System.IO;

namespace GAT_Produkcja.Utilities.Wrappers
{
    public interface IDirectoryInfoWrapper
    {
        DirectoryInfo CreateDirectory(string directoryPath);
        bool DirectoryExists(string directoryPath);

    }
}