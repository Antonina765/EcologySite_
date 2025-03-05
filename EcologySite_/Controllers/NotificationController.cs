using Ecology.Data.Models;
using Ecology.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using EcologySite.Controllers.AuthAttributes;
using EcologySite.Hubs;
using EcologySite.Models.Notification;

namespace EcologySite.Controllers
{
    [IsAdmin]
    public class NotificationController : Controller
    {
        public IHubContext<NotificationHub, INotificationHub> _notificationHub;
        public INotificationRepositoryReal _notificationRepository;

        public NotificationController(
            IHubContext<NotificationHub, INotificationHub> notificationHub, 
            INotificationRepositoryReal notificationRepository)
        {
            _notificationHub = notificationHub;
            _notificationRepository = notificationRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateNotificationViewModel viewModel)
        {
            _notificationRepository.Add(new NotificationData
            {
                Text = viewModel.Text,
                Start = viewModel.Start,
                End = viewModel.End,
            });
            return RedirectToAction("Index");
        }

        public IActionResult Notify(string message)
        {
            _notificationHub.Clients.All.NewNotification(message);
            return RedirectToAction("Index");
        }
    }
}
