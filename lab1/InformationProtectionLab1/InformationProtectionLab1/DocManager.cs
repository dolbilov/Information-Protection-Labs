using System.IO;

namespace InformationProtectionLab1
{
    public class DocManager
    {
        private readonly FileInfo _fileInfo;
        
        public DocManager(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"File {filename} not found");
            }

            _fileInfo = new FileInfo(filename);
            
        }

        public long GetFileLength()
        {
            return _fileInfo.Length;
        }
    }
}