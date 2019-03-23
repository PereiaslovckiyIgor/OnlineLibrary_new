'use strict'

$(document).ready(function () {

    // Кнопка Продолжить читать
    $("#btnUserBookmark").click(function () {

        let data = {
            IdBook: $("#idBook").data("value"),
            IdPage: $("#idPage").data("value")
        };
        console.log(data);
        $.ajax({
            // Для обращение к методам одного контроллера из любого друго
            url: '/BooksPage/AddUserBookmark',
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.success === true)
                    swal(result.responseText, "", "success");
                else
                    swal("Ошибка", result.responseText, "error");
            }
        });//ajax
    });//click event  


}