using Ecology.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using EcologySite.Models.Notification;
using EcologySite.Services;

namespace EcologySite.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiNotificationController : ControllerBase
    {
        private AuthService _authService;
        private INotificationRepositoryReal _notificationRepository;

        public ApiNotificationController(AuthService authService,
            INotificationRepositoryReal notificationRepository)
        {
            _authService = authService;
            _notificationRepository = notificationRepository;
        }

        [HttpGet]
        public List<NotificationViewModel> GetUnseenMessage()
        {
            var userId = _authService.GetUserId();
            if (userId == null)
            {
                return new List<NotificationViewModel>();
            }

            return _notificationRepository
                .GetUnseenMessages(userId.Value)
                .Select(x => new NotificationViewModel
                {
                    Id = x.Id,
                    Text = x.Text,
                })
                .ToList();
        }

        public void MessageWasGetted(int id)
        {
            var userId = _authService.GetUserId();
            if (userId == null)
            {
                return ;
            }

            _notificationRepository.MessageWasSeen(id, userId.Value);
        }
    }
}
