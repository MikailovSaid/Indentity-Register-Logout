﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Utilities.Helpers
{
    public static class Helper
    {
        public static string GetFilePath(string root, string folder, string fileName)
        {
            return Path.Combine(root, folder, fileName);
        }

        public static void DeleteFile(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            
        }
    }
}
