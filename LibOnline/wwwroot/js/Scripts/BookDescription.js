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

    // AJAX отправка формы
    $("#comment-form").submit(function (e) {
        debugger;
        e.preventDefault();

        let form_data = {
            IdBook: $("#idBook").data("value"),
            textComment: $('textarea#textComment').val()
        }

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

});// document ready