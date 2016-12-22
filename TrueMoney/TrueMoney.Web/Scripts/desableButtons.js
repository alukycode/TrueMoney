$(document).ready(function() {
    setTimeout(function() {
        var res = confirm('Ваши данные потеряли актуальность. Перегрузить страницу?');
        if (res) {
            location.reload();
        }

        var buttons = $('.btn:not([type=submit])');
        buttons.each(function (index, value) {
            $(value).addClass('disabled');
        });
    }, 60000000);
});