'use strict';

$(document).ready(function () {

    $("#mainDiv").css('visibility', 'visible');

    CreateTable();

    UserUpdateConfirm();
    UserUpdateСancel();

    padeElemsInit();

});

// Инициализация большинства элементов на странице
function padeElemsInit() {
    $("#btnUserUpdate, #btnConfirmUpdateUser, #btnCanselUpdateUser")
        .jqxButton({ template: "primary" });

    $(".chosen-select").chosen({ width: "95%" });



    // Кнопка диалога изменения пользователя
    $('#btnUserUpdate').click(function () {
        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

        if (rowindex === -1) {
            showNotification('Выберите строку!', false);
            return;
        }//if



        // Очистить спиское перед  началом заполнения
        $('#selIsActiveUser').empty();
        // Select с данными из строки таблицы
        let optionFromRow = '<option selected value=' + (row.isActive === true ? 1 : 0) + '>' + (row.isActive === true ? 'Активен' : 'Заблокирован') + '</option>';
        let optionFromRowOposite = '<option value=' + (row.isActive !== true ? 1 : 0) + '>' + (row.isActive !== true ? 'Активен' : 'Заблокирован') + '</option>';
        $('#selIsActiveUser')
            .append(optionFromRow)
            .append(optionFromRowOposite);
        $('#selIsActiveUser').trigger("chosen:updated");

        $('#dialodUpdateUser').dialog('open');
    });

}//padeElemsInit
// Создание таблицы
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
                    { text: 'IdUser', datafield: 'idUser', align: 'center', type: 'number', filterable: false, hidden: true },
                    { text: 'Имя пользователя', datafield: 'login', align: 'center', type: 'string' },
                    { text: 'E-mail пользователя', datafield: 'email', align: 'center', type: 'string' },
                    { text: 'IdRole', datafield: 'idRole', align: 'center', type: 'number', filterable: false, hidden: true },
                    { text: 'Уровень допуска', datafield: 'roleName', cellsalign: 'center', align: 'center', type: 'string'},
                    { text: 'Активен', datafield: 'isActive', columntype: 'checkbox', filtertype: 'bool', align: 'center' }
                ]
            });

    }//CreateTable

// Получение данных из контроллера
function GetData() {
        let source = {
            datatype: 'json',
            datafield: [
                { name: 'idUser' },
                { name: 'login' },
                { name: 'email' },
                { name: 'idRole' },
                { name: 'roleName' },
                { name: 'isActive' }
            ],
            url: '/Admin/Users/GetUsersAndRoles/',
            id: 'IdUser',
            updaterow: function (rowid, rowdata, commit) {
                // synchronize with the server - send update command
                // call commit with parameter true if the synchronization with the server was successful 
                // and with parameter false if the synchronization has failed.
                commit(true);
            }
        };
        let dataAdapter = new $.jqx.dataAdapter(source, {
            downloadComplete: function (edata, textStatus, jqXHR) {
            }
        });
        return dataAdapter;
}//GetData

// Обработчик событий на клик кнопки, подтверждающей добавление
function UserUpdateConfirm() {
    $('#btnConfirmUpdateUser').click(function (e) {

        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex); 
       
        let data = {
            IdUser: row.idUser,
            IdRole: $('#selUesrRole').val(),
            IsActive: $('#selIsActiveUser').val() === '1' ? true : false
        };

        $.ajax({
            url: '/Admin/Users/UpdateUserAndRole/',
            data: data,
            contentType: 'application/json',
            success: function (result) {
                // Измененные данные в редактируемой строке
                row.idUser = row.idUser;
                row.login = row.login;
                row.email = row.email;
                row.idRole = data.IdRole;
                row.roleName = $('#selUesrRole option:selected').text();
                row.isActive = data.IsActive;

                $('#jqxgrid').jqxGrid('updaterow', row['uid'], row);
                showNotification(result.responseText, result.success);
            },
            complete: function (result) {
                $('#dialodUpdateUser').dialog('close');
            }
        });
    });//click
}//AddAthor

// Обработчик событий на клик кнопки, отклонающий добавление
function UserUpdateСancel() {
    $('#btnCanselUpdateUser').click(function (e) {
        $('#dialodUpdateUer').dialog('close');
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

    //Создание модального окна изменение
    $('#dialodUpdateUser').dialog({
        autoOpen: false,
        modal: true,
        width: 300,
        height: 350,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Изменить пользователя',
        close: function () {
        }
    });

};//window.onload 