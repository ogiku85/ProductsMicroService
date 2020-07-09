using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsMicroService.Service.Utilities
{
    public class FileUploadSettings
    {
        public int MaxBytes { get; set; }
        public string[] AcceptedFileTypes { get; set; }
        public bool IsSupported(string fileName)
        {
            return AcceptedFileTypes.Any(s => s == Path.GetExtension(fileName).ToLower());
        }
        public bool IsMaxBytesExceeded(long fileLenght)
        {
            bool maxBytesExceeded = false;
            try
            {
                if (fileLenght > MaxBytes)
                {
                    maxBytesExceeded = true;
                }
                else
                {
                    maxBytesExceeded = false;
                }
            }
            catch (Exception Ex)
            {
                maxBytesExceeded = false;
            }
            return maxBytesExceeded;
        }
        public bool IsFileEmpty(long fileLenght)
        {
            bool fileEmpty = false;
            try
            {
                if (fileLenght <= 0)
                {
                    fileEmpty = true;
                }
                else
                {
                    fileEmpty = false;
                }
              
            }
            catch(Exception Ex)
            {
                fileEmpty = false;
            }
            return fileEmpty;
        }
    }
}
