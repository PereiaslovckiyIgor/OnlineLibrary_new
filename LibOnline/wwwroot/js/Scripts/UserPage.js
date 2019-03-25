'use strict';

$(document).ready(function () {

    // для пагинации
    let currPage = $("#idPageNumber").data("value");
    let pagesCount = $("#idCountPages").data("value");


    setPaginarion(currPage, pagesCount);

    clickPagination();

});//(document).ready


// Переход на страницу книги
function ContinueToRead(BookId, PageNamber) {
    //let Url = '@Url.Action("GetPageContent", "BooksPage")?IdBook=' + BookId + '&PageNumber=' + PageNamber;
    let Url = '/BooksPage/GetPageContent?IdBook=' + BookId + '&PageNumber=' + PageNamber;
    $(location).attr('href', Url);
}//ContinueToRead


// Удаление книги из избранного
function RemoveUserBook(IdBook) {
    $.ajax({
        url: '/UserPage/RemoveUserBook',
        dataType: "json",
        data: {
            IdBook: IdBook
        },
        success: function (result) {
            if (result.success === true)
                swal(result.responseText, "", "success").then(okay => {
                    if (okay) {
                        window.location.reload();
                    }
                });
            else
                swal("Ошибка", result.responseText, "error");
        }
    });//ajax
}//RemoveUserBook


// Пагинация
function setPaginarion(currPage, pagesCount) {
    $("#pagin").pagination({
        pageIndex: currPage-1,
        pageSize: 1,
        total: pagesCount,
        debug: true,
        showInfo: true,
        showJump: true
    });
}

// Переход по страница пагинации
function clickPagination() {

    $("#pagin").on("pageClicked", function (event, data) {

        let pNum = parseInt(data.pageIndex) + 1;
        let Url = '/UserPage/UserPage?pageNumber=' + pNum;

        $(location).attr('href', Url);
    }).on('jumpClicked', function (event, data) {

        let pNum = parseInt(data.pageIndex) + 1;
        let Url = '/UserPage/UserPage?pageNumber=' + pNum;
        $(location).attr('href', Url);
    });
}

