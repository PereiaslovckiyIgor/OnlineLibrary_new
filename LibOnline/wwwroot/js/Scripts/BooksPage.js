'use strict'


$(document).ready(function () {

    // Возможные размеры 
    let FontSizesArray;

    // Текущая Пагинация
    let currPage = $("#idPageNumber").data("value");
    let pagesCount = $("#idCountPages").data("value");
    let bId = $("#idBook").data("value");

    $.ajax({
        // Для обращение к методам одного контроллера из любого друго
        url: '/BooksPage/GetAllFontSizesValues',
        success: function (result) {
            FontSizesArray = result;
        }
    });

    // Конопка увеличения шрифта
    $("#btnIncreaseTextSize").click(function () {

        let currFontSize = $("#PageText").css("font-size");
        let lastValue = FontSizesArray[FontSizesArray.length - 1].fontSizeValue;

        if (currFontSize !== lastValue) {
            for (var i = 0; i <= FontSizesArray.length - 1; i++) {
                if (FontSizesArray[i].fontSizeValue === currFontSize) {
                    let size = FontSizesArray[i + 1].fontSizeValue;
                    $("#PageText").css("font-size", size);
                    break;
                }//if
            }//for

        }//if
    });

    // Конопка уменьшения шрифта
    $("#btnReduceTextSize").click(function () {
        let currFontSize = $("#PageText").css("font-size");
        let firstValue = FontSizesArray[0].fontSizeValue;

        if (currFontSize !== firstValue) {
            for (var i = 0; i <= FontSizesArray.length - 1; i++) {
                var t = FontSizesArray[i].fontSizeValue;
                if (t === currFontSize) {
                    let size = FontSizesArray[i - 1].fontSizeValue;
                    $("#PageText").css("font-size", size);
                    break;
                }//if
            }//for

        }//if
    });

    // Кнока сохранения настроек
    $("#btnSaveUserSettings").click(function () {
        $.ajax({
            url: '/BooksPage/SaveUserSettings',
            data: {
                fs_Value: $("#PageText").css("font-size")
            },
            success: function (result) {
                if (result.success === true)
                    swal(result.responseText, "", "success");
                else
                    swal("", result.responseText, "error");
            }
        });//ajax
    });//click event    

    // Кнопка доавления закладки
    $("#btnUserBookmark").click(function () {

        let data = {
            IdBook: bId,
            IdPage: $("#idPage").data("value")
        };

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

    // Пагинация
    $("#pagin").pagination({
        pageIndex: currPage-1,
        pageSize: 1,
        total: pagesCount,
        debug: true,
        showInfo: true,
        showJump: true
    });

    $("#pagin").on("pageClicked", function (event, data) {
       
        let pNum = parseInt(data.pageIndex) + 1;
        let Url = '/BooksPage/GetPageContent?IdBook=' + bId + '&PageNumber=' + pNum;

        $(location).attr('href', Url);
    }).on('jumpClicked', function (event, data) {

        let pNum = parseInt(data.pageIndex) + 1;
        let Url = '/BooksPage/GetPageContent?IdBook=' + bId + '&PageNumber=' + pNum;
        $(location).attr('href', Url);
    });




  
});// $(document).ready


