using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Overbot
{
    public class Overbot
    {
        static Assembly _assembly;

        static ImageDirectory overwatch = new ImageDirectory(@"K:\User Files\Pictures\overwatch");

        /*private static List<BattleNetUser> users = new List<BattleNetUser>();*/
        static Dictionary<string,string> users = new Dictionary<string,string>();

        /*Dictionary<string, KnownImageDirectory> KnownDirectories = new Dictionary<string, KnownImageDirectory>()
        {
        };*/


        static Random rnd = new Random();
   
        //This code is executed when the bot is created
        public Overbot()
            {
            var bot = new DiscordClient();

            bot.MessageReceived += bot_MessageReceived;
            //////////////////////////////////////////
            ///Hey their, I just tested the login and it works, discord will ban you for this and it isn't safe to show public.
            bot.Connect("Your Email here", "Password");

            try{
            _assembly = Assembly.GetExecutingAssembly();
            
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error accessing resources");
            }

            LoadUsers();

            bot.Wait();
            }

        //Checks if user has ban permission or is Tolinky
        private static bool IsAdmin(User user)
        {
            if (user.Name == "Tolinky") {return true;}

            bool IsAdmin = false;
            
            foreach(Role role in user.Roles)
            {
                if (role.Permissions.BanMembers == true)
                {
                    IsAdmin = true;
                }
            }

            return IsAdmin;
        }

        //Locates the first 1 or 2 digit number in a message and returns it as an integer
        private static int getSmallNumber(object sender, MessageEventArgs e){

            int number = 1;

            string resultString = System.Text.RegularExpressions.Regex.Match(e.Message.Text, @"\d{1,2}").Value;

            if (resultString != "")
            {
                number = Int32.Parse(resultString);
            }

            if (number >= 10)
            {
                number = 10;
            }
            return number;

        }

        //This code is called whenever a message is received
        private void bot_MessageReceived(object sender, MessageEventArgs e)
        {
            if (e.Message.IsAuthor) return;

            Console.WriteLine("{0} said: '{1}' in the {2} channel ({4}) of {3}", e.User.Name, e.Message.Text, e.Channel.Name, e.Server.Name, e.Channel.Id);

            //If two files have the same name in different KnownFileDirectories, both will be posted. BUG
            if (e.Message.Text.StartsWith("!"))
            {
                string command = Regex.Match(e.Message.Text, @"(?<=!)\w+").Value;

                switch (command)
                {
                    case "help":

                        e.Channel.SendMessage("Recognised commands include: !battletag, !stats, !help. Please use !battletag and your Battle.Net battletag (e.g. User#1234) to link your discord and Battle.Net accounts.");
                        break;

                    case "clean":

                        Clean(e);
                        break;

                    case "battletag":

                        e.Channel.SendMessage(Battletag(e));
                        break;
                }



                /*if (System.Text.RegularExpressions.Regex.Match(command,@"(^help)").Success)
                {
                    string helpDirectory = System.Text.RegularExpressions.Regex.Match(command, @"\w+$").Value;

                    if (KnownDirectories.ContainsKey(helpDirectory))
                    { e.Channel.SendMessage(KnownDirectories[helpDirectory].HelpList); }

                }
                else
                    foreach (KeyValuePair<string, KnownImageDirectory> directory in KnownDirectories) 
                    {
                        if (directory.Value.ImageCommands.ContainsKey(command))
                        {
                            e.Channel.SendFile(directory.Value.SpecificFile(command));
                        }
                    }*/
            }

            

            {
            }

            }

        //Cleans previous messages
        private static void Clean(MessageEventArgs e)
        {
            var messages = e.Channel.Messages;
            foreach (var message in messages)
            {
                if (message.IsAuthor)
                {
                    message.Delete();
                }
            }
        }

        //Extracts whatever text comes after a command
        private static string ExtractInformation(string input)
        {
            string info = "";

            //returns the last word from a string, removing all whitespace around it (just in case). If there isn't one, returns ""
            if (System.Text.RegularExpressions.Regex.Match(input, @"\S+$").Success)
            {
                info = System.Text.RegularExpressions.Regex.Match(input, @"\S+$").Value;
                info.Replace(" ", string.Empty);
                info.Trim();
            }
            return info;
        }

        //Handles responses to the !Battlenet command
        private static string Battletag(MessageEventArgs e)
        {
            string ID = "";
            ID = ExtractInformation(e.Message.Text);
            Discord.User mentioneduser = null;

            if (IsBattlenetID(ID))
            {
                SaveUser(ID, e);
                return("Battletag " + ID + " saved for " + e.User.Name + ".");
            }
            else if (ID.StartsWith("@"))
            {
                foreach (Discord.User user in e.Server.Users)
                {
                    if (user.NicknameMention == ID)
                    {
                        mentioneduser = user;
                        break;
                    }
                }

                if (users.ContainsKey(mentioneduser.Name))
                {
                    string battletag;
                    users.TryGetValue(mentioneduser.Name, out battletag);
                    return battletag;
                }
                else
                {
                    return "Battletag not found.";
                }
            }
            else if (ID.ToLower() == "!battletag")
            {
                 if (users.ContainsKey(e.User.Name))
                        {
                            string battletag;
                            users.TryGetValue(e.User.Name,out battletag);
                            return battletag;
                        }
                        else
                        {
                            return "You have not yet set your Battletag with !Battletag [your battletag].";
                        }
            }
            else { return "Set your battletag by using !Battletag [Your battletag]. Post your battletag by using !Battletag. See another user's battletag by using !Battletag then @mentioning them"; };
        }

        private static void SaveUser(string battletag, MessageEventArgs e)
        {
            string path = AssemblyDirectory;
            StreamWriter streamwriter = new StreamWriter(path + "Users.txt");
            MessageBox.Show(path + "Users.txt");
            streamwriter.(e.User.Name + "," + battletag);
            streamwriter.Dispose();
            LoadUsers();
        }

        private static void LoadUsers()
        {
            //users = new List<BattleNetUser>();
            users = new Dictionary<string,string>();

            StreamReader StreamReader = new StreamReader(AssemblyDirectory + "Users.txt");
            string inputstring;

            while (StreamReader.Peek() != -1)
            {
               inputstring = StreamReader.ReadLine();
               users.Add(Regex.Match(inputstring, @"^\w+").Value,Regex.Match(inputstring, @"\w+#\d+").Value);
            }

            StreamReader.Dispose();
                
        }

        private static bool IsBattlenetID(string input)
        {
            return Regex.Match(input, @"\w+#\d+").Success;
        }

        /*private static BattleNetUser CreateFromCSV(string inputstring) 
        private static KeyValuePair<string,string> CreateFromCSV(string inputstring) 
        {
            //BattleNetUser user = new BattleNetUser("", "");
            KeyValuePair<string,string> user = new KeyValuePair<string,string>(Regex.Match(inputstring, @"^\d+").Value,Regex.Match(inputstring, @"\w+#\d+").Value);
            //user.DiscordID = Regex.Match(inputstring, @"^\d+").Value;
            //user.BattleTag = Regex.Match(inputstring, @"\w+#\d+").Value;

            return user;
        }*/

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

    }   
}
