using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overbot
{
    class BattleNetUser
    {
        public BattleNetUser(string battletag, string discordid)
        {
            this.DiscordID = discordid;
            this.BattleTag = battletag;
            this.DoNotShare = false;
        }
        public string BattleTag
        {
            get;
            set;
        }

        public string DiscordID
        {
            get;
            set;
        }

        public bool DoNotShare
        {
            get;
            set;
        }
    }
}
