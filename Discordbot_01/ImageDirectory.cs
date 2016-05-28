using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Overbot
{
    public class ImageDirectory
    {
        static List<string> ValidExtensions = new List<string> { ".jpg", ".png", ".gif", ".webm" };

        static Random rnd = new Random();

        public ImageDirectory(string DirectoryAddress)
        {
            _DirectoryAddress = DirectoryAddress;

            RefreshFileList();

            AlreadyUsed = new List<string>{};
        }

        protected List<string> FileList
        {
            get;
            set;
        }

        private List<string> AlreadyUsed
        {
            get;
            set;
        }

        protected string _DirectoryAddress
        {
            get;
            set;
        }

        protected List<string> DirectoryList
        {
            get;
            set;
        }

        //rechecks the Directory for new files and folders by clearing and repopulating the directory and file lists
        protected void RefreshFileList()
        {
            RefreshDirectoryList();

            //wipe the file list
            FileList = new List<string> { };
                
            //adds the files from the top level of the Image Directory to the file list
            foreach (var file in Directory.GetFiles(_DirectoryAddress)) { FileList.Add(file); }

            foreach (var directory in DirectoryList)
            {
                foreach (var file in Directory.GetFiles(directory).ToList())
                {
                    FileList.Add(file);
                }
            }
        }

        //rechecks the Directory for new folders by clearing and repopulating the directory list
        protected void RefreshDirectoryList() 
        {
            //wipes the directory list
            DirectoryList = new List<string> { };

            //Populates the Directory List
            DirectoryList = Directory.GetDirectories(_DirectoryAddress, "*", SearchOption.AllDirectories).ToList();

        }

        //returns the address for a random file
        public string RandomFile()
        {

            //Randomly selects a file from the filelist
            string selectedFile = FileList[rnd.Next(0, FileList.Count())];

            //Checks if the randomly selected file has already been used and has a valid extension, if not, it selects another.
            while (!ValidExtensions.Any(selectedFile.Contains) || AlreadyUsed.Any(file => file == selectedFile))
            {

                selectedFile = FileList[rnd.Next(0, FileList.Count())];

            }

            AlreadyUsed.Add(selectedFile);

            return selectedFile;
        }
    }
}
