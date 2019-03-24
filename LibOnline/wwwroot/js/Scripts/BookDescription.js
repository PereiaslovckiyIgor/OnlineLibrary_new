$(document).ready(function (){

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
            }//success
        });//ajax

    });//function

    // Удаление коментария
    $('body').on('click', '#btnRemoveComment', function () {

        /*
           Странный вызов в связи с диномическим созданием кнопок
            ... 
            я так думаю ...
         */ 

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

});// document ready