'use strict';

$(document).ready(function () {

    $("#mainDiv").css('visibility', 'visible');

    padeElemsInit();

    VerificationCancel();

    CreateTable();

});
// Инициализация большинства элементов на странице
function padeElemsInit() {

    $('#jqxTextArea').jqxTextArea({ height: '80%', width: '100%', disabled: true });

    $("#btnVerificatComment, #btnAllowVerification, #btnCanselVerification")
    .jqxButton({ template: "primary", width:'100pt' });

    $('#btnForbitVerification').jqxButton({ template: "danger" });

// Кнопка диалога проверки коментария
    $('#btnVerificatComment').click(function () {

        //Если не выбрана ни обна запись, то выход
    let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
    if (rowindex === -1) {
        showNotification('Выберите строку!', false);
    }//if
    VerificatComment(rowindex);
    });//click

}//padeElemsInit

function VerificatComment(rowindex) {
    if (rowindex === -1) {
        return;
    }

    let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

    let data = {
        IdComments: row.idComments
    };

    $.ajax({
        url: '/Admin/Comments/GetCommentText/',
        contentType: 'application/json',
        data: data,
        success: function (result) {
            $('#jqxTextArea').jqxTextArea('val', result.commentText);
        }
    });

    $('#dialodVerificatComment').dialog('open');

}//VerificatComment

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
                { text: '№', datafield: 'idComments', align: 'center', type: 'number', filterable: false, hidden: true },
                { text: '№', datafield: 'idBook', align: 'center', type: 'number', filterable: false, hidden: true },
                { text: 'Книга', datafield: 'bookName', width: '60%', align: 'center', type: 'string' },
                { text: '№', datafield: 'idUser', align: 'center', type: 'number', filterable: false, hidden: true },
                { text: 'Пользователь', datafield: 'login', width: '10%', align: 'center', type: 'string' },

                { text: 'Дата', datafield: 'commentDate', width: '10%', align: 'center', type: 'string' },

                { text: 'Опубликован', datafield: 'isPuplic', columntype: 'checkbox', filtertype: 'bool', width: '10%', align: 'center' },
                { text: 'Проверенно', datafield: 'isVerificated', columntype: 'checkbox', filtertype: 'bool', width: '10%', align: 'center' }
            ]
        });

}//CreateTable

// Получение данных из контроллера
function GetData() {
    let source = {
        datatype: 'json',
        datafield: [
            { name: 'idComments' },
            { name: 'idBook' },
            { name: 'bookName' },
            { name: 'idUser' },
            { name: 'login' },
            { name: 'commentDate' },
            { name: 'isPuplic' },
            { name: 'isVerificated' }
        ],
        url: '/Admin/Comments/GetComments/',
        id: 'idComments'
    };
    var dataAdapter = new $.jqx.dataAdapter(source, {
        downloadComplete: function (edata, textStatus, jqXHR) {
        }
    });
    return dataAdapter;
}//GetData

// Обработчик событий на клик кнопки, отклонающий верификацию
function VerificationCancel() {
    $('#btnCanselVerification').click(function (e) {
        $('#dialodVerificatComment').dialog('close');
    });
}//AddСancel

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
    $('#dialodVerificatComment').dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        height: 500,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Верификация отзыва',
        close: function () {
            
        }
    });
}