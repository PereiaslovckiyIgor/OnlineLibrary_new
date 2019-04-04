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

        let Url = getLinkedStr(data);
        $(location).attr('href', Url);
    }).on('jumpClicked', function (event, data) {

        let Url = getLinkedStr(data);
        $(location).attr('href', Url);
    });
}

// Динамически формируется строка нужному URL
function getLinkedStr(data) {

    let Url;
    let pNum = parseInt(data.pageIndex) + 1;
    let urlPathname = window.location.pathname;

    if (urlPathname.search('GetBooksAuthors') === -1)
        Url = urlPathname + '?pageNumber=' + pNum;
    else {
        let cNum = $("#idPageNumber").data("value");
        let str = window.location.search;
        Url = urlPathname + str.replace('pageNumber=' + cNum, 'pageNumber=' + pNum);
    }//if-else
    return Url;
}