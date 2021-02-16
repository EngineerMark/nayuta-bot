using System.Collections.Generic;

namespace nayuta.Internal
{
    public class InternalUserManager : Manager<InternalUserManager>
    {
        /// <summary>
        /// List of user data used in the program session
        /// </summary>
        public List<InternalUser> SessionUsers { get; }

        public InternalUserManager()
        {
            SessionUsers = new List<InternalUser>();
        }
        
        public InternalUser GetUser(ulong DiscordID)
        {
            foreach(InternalUser user in SessionUsers)
                if (user.DiscordID == DiscordID.ToString())
                    return user;
            return null;
        }

        public void Register(InternalUser user) => SessionUsers.Add(user);
    }
}