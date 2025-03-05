namespace Ecology.Data.Interface.Models
{
    public interface INotificationData : IBaseModel
    {
        string Text { get; }
        DateTime? Start {  get; }
        DateTime? End { get; }
    }
}
