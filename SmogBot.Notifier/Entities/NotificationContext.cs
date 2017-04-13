// ReSharper disable NonReadonlyMemberInGetHashCode
namespace SmogBot.Notifier.Entities
{
    public class NotificationContext
    {
        public string ConversationReference { get; set; }
        public string CityName { get; set; }
        public int UserId { get; set; }

        protected bool Equals(NotificationContext other)
        {
            return string.Equals(ConversationReference, other.ConversationReference) && string.Equals(CityName, other.CityName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((NotificationContext)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ConversationReference?.GetHashCode() ?? 0) * 397) ^ (CityName?.GetHashCode() ?? 0);
            }
        }
    }
}