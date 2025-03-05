$(document).ready(function () {
    const delayBeforeDeleteNotificatoin = 10 * 1000;
    const animationTime = 500;

    init();

    const hub = new signalR.HubConnectionBuilder()
        .withUrl("/hub/notification")
        .build();

    hub.on("newNotification", (text) => {
        createNotification(text, -1);
    });

    hub.start();

    function init() {
        getUnseenMessage();
    }

    function getUnseenMessage() {
        $.get('/api/ApiNotification/GetUnseenMessage')
            .then((notifications) => {
                notifications.forEach(notification => {
                    createNotification(notification.text, notification.id);
                });
            });
    }

    function createNotification(text, id) {
        const notificationDiv = $('.notification.template').clone();
        notificationDiv.removeClass('template');
        notificationDiv.find('.message').text(text);
        notificationDiv.attr('data-id', id);
        notificationDiv.find('.icon.close').click(onCloseClick);

        $('.notifications-container').append(notificationDiv);
        notificationDiv.show(animationTime);

        hideAndRemiveNotification(notificationDiv, delayBeforeDeleteNotificatoin);
    }

    function onCloseClick() {
        const notificationDiv = $(this).closest('.notification');
        hideAndRemiveNotification(notificationDiv, 0);
    }

    function hideAndRemiveNotification(notificationDiv, delay) {
        setTimeout(() => {
            notificationDiv.hide(animationTime);
        }, delay);

        setTimeout(() => {
            notificationDiv.remove();

            const id = notificationDiv.attr('data-id');
            $.get('/api/ApiNotification/MessageWasGetted?id=' + id);
        }, delay + animationTime);
    }
});
