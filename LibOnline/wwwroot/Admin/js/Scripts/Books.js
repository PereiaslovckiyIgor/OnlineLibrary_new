'use strict';
const _defaultBookName = 'generalBook.jpg';
let _imgFileName;
let _imgId;
let _bookFileName;


$(document).ready(function () {

    $("#mainDiv").css('visibility', 'visible');

    padeElemsInit();

    CreateTable();

    FileUploadSelect();
    FileUploadImgNameStart();

    FileUploadBookNameStart();


    AddBook();

    // UPDATE TEXT
    BookTextToUpdateClick();

    // UPDATE IMG
    FileUploadUpdateSelect();
    FileUploadUpdateImgNameStart();
    FileUploadUpdateConfirm();

    // UPDATE DATE
    ReleasedDataUpdate();

    // UPDATE AUTHORS
    BookAuthorsUpdate();

    // UPDATE CATEGORIES
    BookCategoriesUpdate();

    //UPDATE ISACTIVE 
    IsActiveToUpdate();


    CancelAddBook();
    CancelUpdateBook();
    CancelUpdateBookDate();
    CancelUpdateBookAuthors();
    CancelUpdateBookCategory();
    CancelUpdateIactive();
    CancelUpdateImage();

});// Инициализация большинства элементов на странице

function padeElemsInit() {

    $("#btnAddNewBook, #btnUpdateBook, #btnConfirmAddBook, #btnCanselAddBook, #btnConfirmUpdateTextBook," +
        "#btnCanselUpdateTextBook, #btnConfirmUpdateDate, #btnCanselUpdateDate," +
        "#btnConfirmUpdateAuthor, #btnCanselUpdateAithor, #btnConfirmUpdateCategory, #btnCanselUpdateCategory," +
        "#btnConfirmUpdateIsActive, #btnCanselUpdateIsActive, #btnConfirmUpdateImage, #btnCanselUpdateImage")
        .jqxButton({ template: "primary" });

    $("#Primary").jqxButtonGroup({ template: "primary", mode: 'checkbox' });
    $("#Info").jqxButtonGroup({ template: "info", mode: 'checkbox' });

    //$("#switchButton").jqxSwitchButton({
    //    width: '95%', theme: 'energyblue'
    //});

    // Кнопка диалога добавления новой книги
    $('#btnAddNewBook').click(function () {
        $('#dialodAddNewBook').dialog('open');
    });

    // Кнопки изменения текста книги(Название и Описание)
    $('#Info').on('buttonclick', function (event) {
        let param = event.args.index;
        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        if (rowindex === -1) param = 100;

        switch (param) {
            case 0:
                $('#dialodUpdateBookText').dialog('open');
                break;
            case 1:
                $('#dialodUpdateImage').dialog('open');
                break;
            case 2:
                $('#dialodUpdateDate').dialog('open');
                break;
            case 3:
                $('#dialodUpdateAuthors').dialog('open');
                break;
            case 4:
                $('#dialodUpdateCategory').dialog('open');
                break;
            case 5:
                $('#dialodUpdateIsActive').dialog('open');
                break;
            default:
                showNotification('Выберите строку!', false);
                break;
        }//switch
    });



    // Табы
    $('#jqxtabs').jqxTabs({ width: '100%', height: 360 });

    // FileUpload  IMG
    $('#jqxFileUpload, #jqxFileUploadUpdate').jqxFileUpload({
        width: 300,
        localization: localizationFileUpload,
        browseTemplate: 'primary',
        cancelTemplate: 'warning',
        uploadTemplate: 'primary',
        multipleFilesUpload: false,
        accept: 'image/*'
    });

    // FileUpload  Books
    $('#jqxBookFileUpload').jqxFileUpload({
        width: 300,
        localization: localizationFileUpload,
        browseTemplate: 'primary',
        cancelTemplate: 'warning',
        uploadTemplate: 'primary',
        multipleFilesUpload: false,
        accept: '.txt'
    });


    $(".chosen-select").chosen({ width: '75%', height: '100pt' });
    $(".chosen-selectUpdate").chosen({ width: '95%', height: '100pt' });

    $("#input, #inputUpdate").jqxInput({ theme: 'energyblue', width: '100%', height: '25px', placeHolder: "Название книги" });

    $("#jqxDateTimeInput, #jqxDTI_updateReleasedDate").jqxDateTimeInput({ culture: 'ru-RU', template: "primary", width: 300, height: 30, theme: 'energyblue' });

    $('#editor, #bDecriptionUpdate').jqxEditor({
        height: "100%",
        width: '99%',
        theme: 'energyblue',
        tools: "bold italic underline | left center right | outdent indent | ul ol | html",
        pasteMode: "html"
    });

}//padeElemsInit

function CreateTable() {
    $("#jqxgrid").jqxGrid(
        {
            showfilterrow: true,
            filterable: true,
            source: GetData(),
            theme: 'energyblue',
            width: '85%',
            autoheight: true,
            pageable: true,
            pagesizeoptions: ['5', '10', '15'],
            localization: localizationGrid,
            columns: [
                { text: '№', datafield: 'idBook', align: 'center', type: 'number', filterable: false, hidden: true },
                { text: 'Книга', datafield: 'bookName', width: '70%', align: 'center', type: 'string' },
                { text: 'Дата', datafield: 'releasedData', width: '20%', align: 'center', filterable: false, format: 'dd.MM.yyyy' },
                { text: 'Активно', datafield: 'isActive', columntype: 'checkbox', filtertype: 'bool', width: '10%', align: 'center' }
            ]
        });

}//CreateTable

// Получение данных из контроллера
function GetData() {
    let source = {
        datatype: 'json',
        datafield: [
            { name: 'idBook' },
            { name: 'bookName' },
            { name: 'releasedData' },
            { name: 'isActive' }
        ],
        url: '/Admin/Books/GetBooks/',
        id: 'idComments'
    };
    var dataAdapter = new $.jqx.dataAdapter(source, {
        downloadComplete: function (edata, textStatus, jqXHR) {
        }
    });
    return dataAdapter;
}//GetData

// При выборе изображения
function FileUploadSelect() {

    $('#jqxFileUpload').on('select', function (event) {

        let args = event.args;
        let fileName = args.file;

        let templateSrc = $('#imgNewBook').attr('src');
        let imgName = templateSrc.substring(templateSrc.lastIndexOf("/") + 1, templateSrc.lastIndexOf("?"));

        $('#imgNewBook').attr('src', templateSrc.replace(imgName, fileName));


    });

}//FileUploadSelect

//  Передяется Имя изображения в осн. класс
function FileUploadImgNameStart() {
    $('#jqxFileUpload').on('uploadStart', function (event) {
        _imgFileName = '';
        _imgFileName = event.args.file;

        $('#jqxFileUpload').jqxFileUpload('cancelAll');
        showNotification("Изображение выбрано", true);
    });
}//FileUploadImgNameStart

//  Передяется Имя текстовго файла в осн. класс
function FileUploadBookNameStart() {
    $('#jqxBookFileUpload').on('uploadStart', function (event) {
        _bookFileName = '';
        _bookFileName = event.args.file;

        $('#jqxBookFileUpload').jqxFileUpload('cancelAll');
        showNotification("Книга выбрана", true);

    });
}//FileUploadBookNameStart

function AddBook() {
    $('#btnConfirmAddBook').click(function () {


        let imgName = _imgFileName;
        let selectedAuthors = []; selectedAuthors = $("#selAddAuthors").val();
        let selectedCategories = []; selectedCategories = $("#selAddCategories").val();
        let txtBookName = $('#input').val();
        let txtBookDescription = $('#editor').val();
        let dateBookRelisedDate = $("#jqxDateTimeInput").jqxDateTimeInput('getText');
        let bookFileName = _bookFileName;

        let BookToInsert = {
            BookName: txtBookName,
            ReleasedData: dateBookRelisedDate,
            BooksDescription: txtBookDescription,
            ImagePath: imgName,
            BookPath: bookFileName,
            IdAuthor: selectedAuthors.join(),
            IdCategory: selectedCategories.join()
        };

        if (imgName !== 'undefined' && bookFileName !== 'undefined' &&
            selectedAuthors.length > 0 && selectedCategories.length > 0 &&
            txtBookName !== '' && txtBookDescription !== '') {

            $.post('/Admin/Books/AddNewBook/', BookToInsert).done(function (result) {
                $('#jqxgrid').jqxGrid('updatebounddata');
                showNotification(result.responseText, result.success);
                $('#dialodAddNewBook').dialog('close');
            });
        }
        else {
            showNotification("Форма добваления книги заполнена неверно", false);
        }
    });

}//AddBook

// Заполнение формы для изменения Названия и опсания
function GetBookTextToUpdate() {
    // Данные выбранной строки строки
    let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
    let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

    // Данные Измененные пользователем
    let data = {
        IdBook: row.idBook
    };

    $.ajax({
        url: '/Admin/Books/GetBookTextToUpdate/',
        contentType: 'application/json',
        data: data,
        success: function (result) {
            $('#inputUpdate').val(result.bookName);
            $('#bDecriptionUpdate').val(result.booksDescription);
        }
    });

}//GetBookTextToUpdate

// Изменение с учетом редактирования
function BookTextToUpdateClick() {
    $('#btnConfirmUpdateTextBook').click(function () {
        // Данные выбранной строки строки
        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);


        // Данные Измененные пользователем
        let data = {
            IdBook: row.idBook,
            BookName: $('#inputUpdate').val(),
            BookDescription: $('#bDecriptionUpdate').val()
        };

        $.post('/Admin/Books/UpdateBookText/', data).done(function (result) {
            row.bookName = data.BookName;
            $('#jqxgrid').jqxGrid('updaterow', row, row['uid']);
            showNotification(result.responseText, result.success);
            $('#dialodUpdateBookText').dialog('close');
        });

    });//click

}//BookTextToUpdateClick

// Изменить вывод изображения со стандартного на актуальное
function GetImgToUpdate() {
    // Данные выбранной строки строки
    let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
    let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

    // Данные Измененные пользователем
    let data = {
        IdBook: row.idBook
    };

    $.ajax({
        url: '/Admin/Books/GetImageToUpdate',
        contentType: 'application/json',
        data: data,
        success: function (result) {

            _imgId = result.idImage;

            let iPath = result.imagePath;
            let templateSrc = $('#imgUpdateBook').attr('src');
            let imgName = templateSrc.substring(templateSrc.lastIndexOf("/") + 1, templateSrc.lastIndexOf("?"));

            $('#imgUpdateBook').attr('src', templateSrc.replace(imgName, iPath));
        }
    });

}//GetImgToUpdate

// При выборе изображения UPDATE
function FileUploadUpdateSelect() {

    $('#jqxFileUploadUpdate').on('select', function (event) {

        let args = event.args;
        let fileName = args.file;

        let templateSrc = $('#imgUpdateBook').attr('src');
        let imgName = templateSrc.substring(templateSrc.lastIndexOf("/") + 1, templateSrc.lastIndexOf("?"));

        $('#imgUpdateBook').attr('src', templateSrc.replace(imgName, fileName));

    });

}//FileUploadUpdateSelect

//  Подтвердить UPDATE
function FileUploadUpdateImgNameStart() {
    $('#jqxFileUploadUpdate').on('uploadStart', function (event) {

        _imgFileName = '';
        _imgFileName = event.args.file;
        $('#jqxFileUploadUpdate').jqxFileUpload('cancelAll');
        showNotification("Изображение выбрано", true);
    });
}//FileUploadImgNameStart

// Update Click Confirm
function FileUploadUpdateConfirm() {
    $('#btnConfirmUpdateImage').click(function () {

        // Данные выбранной строки строки
        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

        let data = {
            ImgName: _imgFileName,
            IdBook: row.idBook
        };

        $.ajax({
            url: '/Admin/Books/BookImageUpdate',
            contentType: 'application/json',
            data: data,
            success: function (result) {
                showNotification(result.responseText, result.success);
            },
            complete: function (result) {
                $('#dialodUpdateImage').dialog('close');
            }
        });


    });//click
}//FileUploadUpdateConfirm

// Получить дату из выбранной строки
function GetDateToUpdate() {
    // Данные выбранной строки строки
    let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
    let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);
    
    let dateArr = row.releasedData.split('.');
    $('#jqxDTI_updateReleasedDate ').jqxDateTimeInput('setDate', new Date(dateArr[2], dateArr[1]-1, dateArr[0]));

}//GetDateToUpdate

// Изменить дату издания книги
function ReleasedDataUpdate() {
    $('#btnConfirmUpdateDate').click(function () {
        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

        let newDate = $('#jqxDTI_updateReleasedDate ').jqxDateTimeInput('getText');

        let data = {
            ReleasedData: newDate,
            IdBook: row.idBook
        };
        $.ajax({
            url: '/Admin/Books/ReleasedDataUpdate',
            contentType: 'application/json',
            data: data,
            success: function (result) {
                row.releasedData = data.ReleasedData;
                $('#jqxgrid').jqxGrid('updaterow', row, row['uid']);
                showNotification(result.responseText, result.success);
            },
            complete: function (result) {
                $('#dialodUpdateDate').dialog('close');
            }
        });//ajax

    });//click
}//ReleasedDataUpdate

// Список текущих авторов у книги
function GetAuthorsToUpdate() {
    let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
    let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

    let data = {
        IdBook: row.idBook
    };
    $.ajax({
        url: '/Admin/Books/GetAuthorsToUpdate',
        contentType: 'application/json',
        data: data,
        success: function (result) {
            $('#selUpdateAuthors').html(result);
            $('#selUpdateAuthors').trigger('chosen:updated');

        }//success
    });//ajax

}//GetAuthorsToUpdate

// Обновить авторов у книги
function BookAuthorsUpdate() {
    $('#btnConfirmUpdateAuthor').click(function () {

        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

        let selectedAuthors = []; selectedAuthors = $('#selUpdateAuthors').val();

        let data = {
            IdBook: row.idBook,
            Authors: selectedAuthors.join(',')
        };

        $.ajax({
            url: '/Admin/Books/BookAuthorsUpdate',
            contentType: 'application/json',
            data: data,
            success: function (result) {
                showNotification(result.responseText, result.success);
            },
            complete: function (result) {
                $('#dialodUpdateAuthors').dialog('close');
            } 

        });//ajax

    });//click
}//BookAuthorsUpdate

// Список текущих жанорв у книги
function GetCategoriesToUpdate() {
    let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
    let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

    let data = {
        IdBook: row.idBook
    };
    $.ajax({
        url: '/Admin/Books/GetCategoriesToUpdate',
        contentType: 'application/json',
        data: data,
        success: function (result) {
            $('#selUpdateCategories').html(result);
            $('#selUpdateCategories').trigger('chosen:updated');

        }//success
    });//ajax

}//GetCategoriesToUpdate

// Обновить жанры книги
function BookCategoriesUpdate() {
    $('#btnConfirmUpdateCategory').click(function () {

        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

        let selectedCategories = []; selectedCategories = $('#selUpdateCategories').val();

        let data = {
            IdBook: row.idBook,
            Categories: selectedCategories.join(',')
        };

        $.ajax({
            url: '/Admin/Books/BookCategoriesUpdate',
            contentType: 'application/json',
            data: data,
            success: function (result) {
                showNotification(result.responseText, result.success);
            },
            complete: function (result) {
                $('#dialodUpdateCategory').dialog('close');
            }

        });//ajax

    });//click
}//BookCategoriesUpdate

// Получить состоние книги (Активна / Забокирована)
function GetIsActiveToUpdate() {

    let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
    let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);
    let IsActive = row.isActive;

    $("#switchButton").jqxSwitchButton({
        width: '95%', theme: 'energyblue'
    });
    $("#switchButton").val(IsActive);

}//GetIsActiveToUpdate

// Обновить IsActive
function IsActiveToUpdate() {
    $("#btnConfirmUpdateIsActive").click(function () {
        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

        let data = {
            IdBook: row.idBook,
            IsActive: $("#switchButton").val()
        };

        $.ajax({
            url: '/Admin/Books/IsActiveToUpdate',
            contentType: 'application/json',
            data: data,
            success: function (result) {
                row.isActive = data.IsActive;
                $('#jqxgrid').jqxGrid('updaterow', row, row['uid']);
                showNotification(result.responseText, result.success);
            },
            complete: function (result) {
                $('#dialodUpdateIsActive').dialog('close');
            }
        });//ajax

    });
}//IsActiveToUpdate


// Отмена добавления
function CancelAddBook() {
    $('#btnCanselAddBook').click(function () {
        $('#dialodAddNewBook').dialog('close');
    });
}//CancelAddBook

function CancelUpdateBook() {
    $('#btnCanselUpdateTextBook').click(function () {
        $('#dialodUpdateBookText').dialog('close');
    });
}

function CancelUpdateBookDate() {
    $('#btnCanselUpdateDate').click(function () {
        $('#dialodUpdateDate').dialog('close');
    });
}

function CancelUpdateBookAuthors() {
    $('#btnCanselUpdateAithor').click(function () {
        $('#dialodUpdateAuthors').dialog('close');
    });
}

function CancelUpdateBookCategory() {
    $('#btnCanselUpdateCategory').click(function () {
        $('#dialodUpdateCategory').dialog('close');
    });
}

function CancelUpdateIactive() {
    $('#btnCanselUpdateIsActive').click(function () {
        $('#dialodUpdateIsActive').dialog('close');
    });
}

function CancelUpdateImage() {
    $('#btnCanselUpdateImage').click(function () {
        $('#dialodUpdateImage').dialog('close');
    });
}







// alert
function showNotification(notificationText, isSucces) {

    let template;
    isSucces === true ? template = 'success' : template = 'error';

    $("#jqxNotificationShow").text(notificationText);
    $("#messageNotification").jqxNotification({
        width: 250, position: "top-right", opacity: 0.9,
        autoOpen: false, animationOpenDelay: 800, autoClose: true, autoCloseDelay: 3000, template: template
    }).jqxNotification('open');

} //showNotification


window.onload = function () {

    $('#dialodAddNewBook').dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 500,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Добавить книгу',
        close: function () {

            var templateSrc = $('#imgNewBook').attr('src');
            var imgName = templateSrc.substring(templateSrc.lastIndexOf("/") + 1, templateSrc.lastIndexOf("?"));

            $('#imgNewBook').attr('src', templateSrc.replace(imgName, _defaultBookName));


            $('#jqxFileUpload').jqxFileUpload('cancelAll');
            $('#jqxBookFileUpload').jqxFileUpload('cancelAll');

            $('#input').val('');
            $('#editor').val('');

            $(".chosen-select").val('').trigger("chosen:updated");
        }
    });

    $('#dialodUpdateBookText').dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 500,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Название и Описание',
        open: function () {
            GetBookTextToUpdate();
        },
        close: function () {
            $('#inputUpdate').val('');
            $('#bDecriptionUpdate').val('');
        }
    });

    $('#dialodUpdateDate').dialog({
        autoOpen: false,
        modal: true,
        width: 350,
        height: 200,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Дата Выпуска',
        open: function () {
            GetDateToUpdate();
        }
       
    });

    $('#dialodUpdateAuthors').dialog({
        autoOpen: false,
        modal: true,
        width: 350,
        height: 450,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Авторы',
        open: function () {
            GetAuthorsToUpdate();
        },
        close: function () {
            $(".chosen-selectUpdate").val('').trigger("chosen:updated");
        }
    });

    $('#dialodUpdateCategory').dialog({
        autoOpen: false,
        modal: true,
        width: 350,
        height: 450,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Жанры',
        open: function () {
            GetCategoriesToUpdate();
        },
        close: function () {
            $(".chosen-selectUpdate").val('').trigger("chosen:updated");
        }
    });

    $('#dialodUpdateIsActive').dialog({
        autoOpen: false,
        modal: true,
        width: 250,
        height: 200,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Активно / Заблокировать',
        open: function () {
            GetIsActiveToUpdate();
        },
        close: function () {
        }
    });

    $('#dialodUpdateImage').dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 450,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Изображение',
        open: function () {
            GetImgToUpdate();
        },
        close: function () {

            var templateSrc = $('#imgUpdateBook').attr('src');
            var imgName = templateSrc.substring(templateSrc.lastIndexOf("/") + 1, templateSrc.lastIndexOf("?"));

            $('#imgUpdateBook').attr('src', templateSrc.replace(imgName, _defaultBookName));

            $('#jqxFileUploadUpdate').jqxFileUpload('cancelAll');
        }
    });

};