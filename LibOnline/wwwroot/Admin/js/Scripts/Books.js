'use strict';
const _defaultBookName = 'generalBook.jpg';
let _imgFileName;
let _bookFileName;

$(document).ready(function () {

    $("#mainDiv").css('visibility', 'visible');

    padeElemsInit();

    CreateTable();

    FileUploadSelect();
    FileUploadImgNameStart();

    FileUploadBookNameStart();


    AddBook();
    CancelAddBook();
});// Инициализация большинства элементов на странице

function padeElemsInit() {

    $("#btnAddNewBook, #btnUpdateBook, #btnConfirmAddBook, #btnCanselAddBook")
        .jqxButton({ template: "primary" });


    // Кнопка диалога добавления нового автора
    $('#btnAddNewBook').click(function () {
        $('#dialodAddNewBook').dialog('open');
    });

    // Табы
    $('#jqxtabs').jqxTabs({ width: '100%', height: 360 });

    // FileUpload  IMG
    $('#jqxFileUpload, #jqxBookFileUpload').jqxFileUpload({
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

    $("#input").jqxInput({ theme: 'energyblue', width: '100%', height: '25px', placeHolder: "Название книги" });

    $("#jqxDateTimeInput").jqxDateTimeInput({ culture: 'ru-RU', template: "primary", width: 300, height: 30, theme: 'energyblue' });

    $('#editor').jqxEditor({
        height: "100%",
        width: '99%',
        theme: 'energyblue',
        tools: "bold italic underline | left center right | outdent indent | ul ol | html",
        pasteMode: "text"
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

            $.ajax({
                url: '/Admin/Books/AddNewBook/',
                data: BookToInsert,
                success: function (result) {
                    //showNotification(result.responseText, result.success);
                    //$('#jqxgrid').jqxGrid('updatebounddata');
                },
                complete: function (result) {
                    //$('#dialodAddAuthor').dialog('close');
                }
            });


        }
        else {
            showNotification("Форма добваления книги заполнена неверно", false);
        }
    });

}//AddBook

// Отмена добавления
function CancelAddBook() {
    $('#btnCanselAddBook').click(function () {
        $('#dialodAddNewBook').dialog('close');
    });
}//CancelAddBook

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
    //Создание модального окна Верификации
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
};