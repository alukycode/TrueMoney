$(document).ready(function() {
    setTimeout(function() {
        var buttons = $('.btn:not([type=submit])');
        var res = confirm('Ваши данные потеряли актуальность. Перегрузить страницу?');
        if (res) {
            location.reload();
        }

        buttons.each(function (index, value) {
            $(value).addClass('disabled');
        });
    }, 600000);
});