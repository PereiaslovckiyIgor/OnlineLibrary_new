$(document).ready(function () {

    let data = {
        IdBook: $("#idBook").data("value")
    };

    // Ссылка обращается в метод конроллера
    $('#btnReadBook').click(function () {
        //let Url = Url.Action("GetPageContent", "BooksPage", new { IdBook = data.IdBook, PageNumber = 1 });
        let Url = '/BooksPage/GetPageContent?IdBook=' + data.IdBook + '&PageNumber=1';
        $(location).attr('href', Url);
    });

    // Ссылка обращается в метод конроллера
    $('#btnAddInUserBooks').click(function () {
        $.ajax({
            url: '/BooksDescription/AddInUserBooks',
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.success === true)
                    swal(result.responseText, "", "success");
                else
                    swal("", result.responseText, "error");
            }//success
        });//ajax
    });

    // AJAX отправка формы Добавления коментария
    $("#comment-form").submit(function (e) {

        e.preventDefault();

        let form_data = {
            IdBook: $("#idBook").data("value"),
            textComment: $('textarea#textComment').val()
        };

        $.ajax({
            type: "POST",
            url: '/BooksDescription/SendComment/',
            data: form_data,
            success: function (result) {
                if (result.success === true)
                    swal(result.responseText, "", "success").then(okay => {
                        if (okay) {
                            window.location.reload();
                        }
                    });
                else
                    swal("", result.responseText, "error");
                $('html, body').animate({
                    scrollTop: $("#commentSection").offset().top
                }, 2000);
            }//success
        });//ajax

    });//function

    // Удаление коментария
    $('body').on('click', '#btnRemoveComment', function () {

        let CommentId = $(this).val();

        $.ajax({
            url: '/BooksDescription/RemoveComment/',
            data: {
                idComment: CommentId
            },
            success: function (result) {
                if (result.success === true)
                    swal(result.responseText, "", "success").then(okay => {
                        if (okay) {
                            window.location.reload();
                        }
                    });
                else
                    swal("", result.responseText, "error");
            }//success
        });//ajax
    });


    // "Звездный" рейтинг (Активный, для зарегистр. пользователей)
    $("#rateYo").rateYo({
        fullStar: true,
        starWidth: "25px",
        spacing: "10px",
        rating: $("#idBookRaring").data("value")
    }).on("rateyo.set", function (e, data) {
        $.post("/BooksDescription/SetBookRating/", { IdBook: $("#idBook").data("value"), UsreRating: data.rating });
    });


    // "Звездный" рейтинг (Не ктивный, для не зарегистр. пользователей)
    // Просто отображение рейтинга книги 
    $("#rateYo_readOnly").rateYo({
        fullStar: true,
        starWidth: "25px",
        spacing: "10px",
        readOnly: true,
        rating: $("#idBookRaring").data("value")
    });


});// document ready