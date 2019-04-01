'use strict';
const _defaultBookName = 'generalBook.jpg';


$(document).ready(function () {

    $("#mainDiv").css('visibility', 'visible');

    padeElemsInit();

    CreateTable();

    FileUploadSelect();
    FileUploadStart();



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

    // FileUpload 
    $('#jqxFileUpload').jqxFileUpload({
        width: 300,
        localization: localizationFileUpload,
        browseTemplate: 'primary',
        cancelTemplate: 'warning',
        uploadTemplate: 'primary',
        multipleFilesUpload: false
    });

    $(".chosen-select").chosen({ disable_search_threshold: 5, width: '75%' }); 

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
        var args = event.args;
        var fileName = args.file;
        
        var templateSrc = $('#imgNewBook').attr('src');
        var imgName = templateSrc.substring(templateSrc.lastIndexOf("/") + 1, templateSrc.lastIndexOf("?"));

        $('#imgNewBook').attr('src', templateSrc.replace(imgName, fileName));
    });

}//FileUploadSelect

//  Передяется Имя изображения в осн. класс
function FileUploadStart() {
    $('#jqxFileUpload').on('uploadStart', function (event) {
        var fileName = event.args.file;
        
    }); 

}

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


            $(".chosen-select").val('').trigger("chosen:updated");
        }
    });
}