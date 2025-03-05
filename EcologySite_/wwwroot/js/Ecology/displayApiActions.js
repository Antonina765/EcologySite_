$(document).ready(function() {
    $.ajax({
        url: '/api/apiinfo',
        method: 'GET',
        success: function(data) {
            const apiList = $('#apiActions');
            apiList.empty();
            data.forEach(api => {
                const listItem = $('<li></li>');
                listItem.text(`${api.HttpMethod} - ${api.Controller} - ${api.Action} (Route: ${api.Route})`);
                apiList.append(listItem);
            });
        },
        error: function() {
            $('#apiActions').text('Error loading API info');
        }
    });
});
