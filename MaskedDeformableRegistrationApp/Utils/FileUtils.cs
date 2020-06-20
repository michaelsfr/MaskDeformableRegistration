﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.Utils
{
    public static class FileUtils
    {
        public static List<string> GetPathsFromTextBoxInput(string textBoxInput)
        {
            List<string> wsi = new List<string>();
            var pathes_splitted = textBoxInput.Split(';');
            string pattern = "\"(.+)\"";
            Regex rgx = new Regex(pattern, RegexOptions.None);

            // add file names to list if they match the pattern, i.e. if they are quoted
            foreach (string p in pathes_splitted)
            {
                MatchCollection matches = rgx.Matches(p);
                foreach (Match match in matches)
                    wsi.Add(match.Groups[1].Value);
            }
            return wsi;
        }

        public static void CopyFile(string src, string dest)
        {
            if (File.Exists(src))
            {
                if (File.Exists(dest))
                {
                    File.Delete(dest);
                }
                File.Copy(src, dest);
            }
        }
    }
}
