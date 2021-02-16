using SQLite;

namespace nayuta.Internal
{
    [Table("users")]
    public class InternalUser
    {
        [Column("id"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        
        [Column("discord_id")]
        public string DiscordID { get; set; }
        
        [Column("osu_id")]
        public string OsuUserID { get; set; }

        [Ignore]
        public bool IsNewUser { get; set; } = false;
        
        public InternalUser() => InternalUserManager.Instance.Register(this);
    }
}