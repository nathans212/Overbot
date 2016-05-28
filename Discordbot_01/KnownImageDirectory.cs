using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overbot
{
    class KnownImageDirectory : ImageDirectory
    {
        public Dictionary<string, string> ImageCommands = new Dictionary<string, string>() { };

        private string _HelpList = "";
        public string HelpList
        {
            get;
            set;
        }

        public KnownImageDirectory(string DirectoryAddress) : base(DirectoryAddress)
        {
            _DirectoryAddress = DirectoryAddress;

            RefreshFileList();

            GenerateImageCommands();

            ConstructHelpList();
        }

        private void GenerateImageCommands()
        {
                ImageCommands =
                    FileList.ToDictionary(f => System.Text.RegularExpressions.Regex.Match(f, @"[^\\\/]+(?=\..+$)").Value);
        }


        //returns the address for a file if the entered string is a key, otherwise returns an empty string.
        public string SpecificFile(string FileAlias) 
        {
            //returns the address if it's in the command list
            if(ImageCommands.ContainsKey(FileAlias))

            {return ImageCommands[FileAlias];}

            // returns an empty string otherwise.
            else

            { return ""; }
        }

        private void ConstructHelpList()
        {
            HelpList = "";
            foreach (KeyValuePair<string, string> command in ImageCommands)
            {
                HelpList = HelpList + command.Key + ", ";
            }
        }
    }
}
