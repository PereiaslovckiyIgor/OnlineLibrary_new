'use strict';

$(document).ready(function () {

    // для пагинации
    let currPage = $("#idPageNumber").data("value");
    let pagesCount = $("#idCountPages").data("value");


    setPaginarion(currPage, pagesCount);

    clickPagination();

});//(document).ready

// Пагинация
function setPaginarion(currPage, pagesCount) {
    $("#pagin").pagination({
        pageIndex: currPage - 1,
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
        let Url = window.location.pathname + '?pageNumber=' + pNum;
        //let Url = '/BooksCategories/GetPopularBooks?pageNumber=' + pNum;

        $(location).attr('href', Url);
    }).on('jumpClicked', function (event, data) {
        
        let pNum = parseInt(data.pageIndex) + 1;
        let Url = window.location.pathname + '?pageNumber=' + pNum;
        //let Url = '/BooksCategories/GetPopularBooks?pageNumber=' + pNum;
        $(location).attr('href', Url);
    });
}