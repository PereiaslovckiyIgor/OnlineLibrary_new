    'use strict';

$(document).ready(function () {

    $("#mainDiv").css('visibility', 'visible');

CreateTable();
padeElemsInit();

AddAthor();
AddСancel();


UpdateAuthorConfirm();
UpdateСancel();

onlyLetters();

});// $(document).ready

// Инициализация большинства элементов на странице
function padeElemsInit() {

    $("#btnAddNewAuthor, #btnAuthorUpdate, #btnCanselAddAuthor, #btnConfirmAddAuthor, #btnConfirmUpdateAuthor, #btnCanselUpdateAuthor")
        .jqxButton({ template: "primary" });

$("#txtAuthorFullNameInsert").jqxInput({placeHolder: "Полное имя автора", height: 30, width: '100%', minLength: 1 });

    $("#txtAuthorFullNameUpdate").jqxInput({placeHolder: "Полное имя автора", height: 30, width: '100%', minLength: 1 });
    $("#jqxcheckbox").jqxCheckBox({height: 25, width:100 });


// Кнопка диалога добавления нового автора
    $('#btnAddNewAuthor').click(function () {
    $('#dialodAddAuthor').dialog('open');
});

// Кнопка диалога изменения автроа
    $('#btnAuthorUpdate').click(function () {
    //Если не выбрана ни обна запись, то выход
    let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        if (rowindex === -1) {
    showNotification('Выберите строку!', false);
}
UpdateAuthor(rowindex);
});

}//padeElemsInit

// Обработчик событий на клик кнопки, подтверждающей добавление
function AddAthor() {
    $('#btnConfirmAddAuthor').click(function (e) {
        if (!$('#dialodAddAuthor').jqxValidator('validate')) {
            return;
        }
        $.ajax({
            url: '/Admin/Authors/AuthorInsert/',
            data: {
                AuthorFullName: $.trim($("#txtAuthorFullNameInsert").val())
            },
            contentType: 'application/json',
            success: function (result) {
                showNotification(result.responseText, result.success);
                $('#jqxgrid').jqxGrid('updatebounddata');
            },
            complete: function (result) {
                $('#dialodAddAuthor').dialog('close');
            }
        });
    });//click
}//AddAthor

// Обработчик событий на клик кнопки, отклонающий добавление
function AddСancel() {
    $('#btnCanselAddAuthor').click(function (e) {
        $('#dialodAddAuthor').jqxValidator('hide');
        $('#dialodAddAuthor').dialog('close');
    });
}//AddСancel

// Обработчик события изменения автроа
function UpdateAuthorConfirm() {
    $('#btnConfirmUpdateAuthor').click(function () {
        if (!$('#dialodUpdateAuthor').jqxValidator('validate')) {
            return;
        }
        // Данные выбранной строки строки
        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

        // Данные Измененные пользователем
        let data = {
            IdAuthor: $('#idAithorForUpdate').val(),
            AuthorFullName: $('#txtAuthorFullNameUpdate').val(),
            IsActive: $('#jqxcheckbox').val()
        };

        $.ajax({
            url: '/Admin/Authors/AuthorUpdate/',
            contentType: 'application/json',
            data: data,
            success: function (result) {
                // Измененные данные в редактируемой строке
                row.idAuthor = data.IdAuthor;
                row.authorFullName = data.AuthorFullName;
                row.isActive = data.IsActive;

                $('#jqxgrid').jqxGrid('updaterow', 0, row['uid']);

                showNotification(result.responseText, result.success);
            },

            complete: function (result) {
                $('#dialodUpdateAuthor').dialog('close');
            }
        });
    });//click
}//UpdateAuthor


// Обработчик событий на клик кнопки, отклонающий изменение
function UpdateСancel() {
    $('#btnCanselUpdateAuthor').click(function (e) {
        $('#dialodUpdateAuthor').jqxValidator('hide');
        $('#dialodUpdateAuthor').dialog('close');
    });
}//UpdateСancel

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
                { text: '№', datafield: 'idAuthor', align: 'center', type: 'number', filterable: false, hidden: true },
                { text: 'Автор', datafield: 'authorFullName', width: '65%', align: 'center', type: 'string' },
                { text: 'Активен', datafield: 'isActive', columntype: 'checkbox', filtertype: 'bool', width: '35%', align: 'center' }
            ]
        });

}//CreateTable

// Получение данных из контроллера
function GetData() {
    let source = {
    datatype: 'json',
    datafield: [
            {name: 'idAuthor' },
            {name: 'authorFullName' },
            {name: 'isActive' }
            ],
    url: '/Admin/Authors/GetAuthors/',
    id: 'idAuthor'
};
    var dataAdapter = new $.jqx.dataAdapter(source, {
    downloadComplete: function (edata, textStatus, jqXHR) {
}
});
return dataAdapter;
}//GetData


// Обработчик события на клик кнопки, изменения записи
function UpdateAuthor(rowindex) {

    // Получить данные из выбранной строчки
    let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

    //Заполнение элементов формы для изменения
    $('#idAithorForUpdate').val(row.idAuthor);
    $('#txtAuthorFullNameUpdate').val(row.authorFullName);
    $('#jqxcheckbox').val(row.isActive);

    $('#dialodUpdateAuthor').dialog('open');
}

// Только русские символы и пробел на полях ввода
function onlyLetters() {
    $("#txtAuthorFullNameInsert, #txtAuthorFullNameUpdate").on("input", function () {
        var regexp = /[^а-яА-ЯёЁ .-]/g;
        if ($(this).val().match(regexp)) {
            $(this).val($(this).val().replace(regexp, ''));
        }
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
    //Создание модального окна Добавление
    $('#dialodAddAuthor').dialog({
        autoOpen: false,
        modal: true,
        width: 400,
        height: 230,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Добавить Автора',
        close: function () {
            $('#dialodAddAuthor input[type=text]').val('');
            $('#dialodAddAuthor').jqxValidator('hide');
        }
    });

    //Создание модального окна изменение
    $('#dialodUpdateAuthor').dialog({
        autoOpen: false,
        modal: true,
        width: 400,
        height: 250,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Изменить Автора',
        close: function () {
            //$('#idAithorForUpdate').val('');
            //$('#txtAuthorFullNameUpdate').val('');
            $('#dialodUpdateAuthor input[type=hidden]').val('');
            $('#dialodUpdateAuthor input[type=text]').val('');
            $('#dialodUpdateAuthor').jqxValidator('hide');
        }
    });

    // Валидация При Добавлении
    $('#dialodAddAuthor').jqxValidator({
        focus: false,
        hintType: "label",
        rules:
            [
                {
                    input: '#txtAuthorFullNameInsert',
                    message: 'Поле  должно быть заполнено!',
                    action: 'keyup',
                    rule: 'required'
                },
                {
                    input: '#txtAuthorFullNameInsert',
                    message: 'Ролное имя от 2 до 100 символов',
                    action: 'keyup',
                    rule: 'length=2,100'
                }


            ]
    });

    // Валидация При Изменении
    $('#dialodUpdateAuthor').jqxValidator({
        focus: false,
        hintType: "label",
        rules:
            [
                {
                    input: '#txtAuthorFullNameUpdate',
                    message: 'Поле  должно быть заполнено!',
                    action: 'keyup',
                    rule: 'required'
                },
                {
                    input: '#txtAuthorFullNameUpdate',
                    message: 'Полное имя от 2 до 100 символов',
                    action: 'keyup',
                    rule: 'length=2,100'
                }
            ]
    });


};//window.onload 