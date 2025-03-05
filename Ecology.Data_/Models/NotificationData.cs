using Ecology.Data.Interface.Models;

namespace Ecology.Data.Models
{
    public class NotificationData : BaseModel, INotificationData
    {
        public string Text { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public virtual List<UserData> UsersWhoAlreadySawIt {  get; set; }
        
        public NotificationData()
        {
            Start = DateTime.UtcNow;
        }
    }
}
