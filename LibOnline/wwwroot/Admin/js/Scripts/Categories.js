'use strict';

$(document).ready(function () {

    $("#mainDiv").css('visibility', 'visible');

    CreateTable();
    padeElemsInit();

    AddAthor();
    AddСancel();


    UpdateCategoryConfirm();
    UpdateСancel();

    onlyLetters();

});// $(document).ready

// Инициализация большинства элементов на странице
function padeElemsInit() {

    $("#btnAddNewCategory, #btnCategoryUpdate, #btnCanselAddCategory, #btnConfirmAddCategory, #btnConfirmUpdateCategory, #btnCanselUpdateCategory")
        .jqxButton({ template: "primary" });

    $("#txtCategoryFullNameInsert").jqxInput({ placeHolder: "Жанр", height: 30, width: '100%', minLength: 1 });

    $("#txtCategoryFullNameUpdate").jqxInput({ placeHolder: "Жанр", height: 30, width: '100%', minLength: 1 });
    $("#jqxcheckbox").jqxCheckBox({ height: 25, width: 100 });


    // Кнопка диалога добавления нового автора
    $('#btnAddNewCategory').click(function () {
        $('#dialodAddCategory').dialog('open');
    });

    // Кнопка диалога изменения автроа
    $('#btnCategoryUpdate').click(function () {
        //Если не выбрана ни обна запись, то выход
        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        if (rowindex === -1) {
            showNotification('Выберите строку!', false);
        }
        UpdateCategory(rowindex);
    });

}//padeElemsInit

// Обработчик событий на клик кнопки, подтверждающей добавление
function AddAthor() {
    $('#btnConfirmAddCategory').click(function (e) {
        if (!$('#dialodAddCategory').jqxValidator('validate')) {
            return;
        }
        $.ajax({
            url: '/Admin/Categories/CategoryInsert/',
            data: {
                CategoryName: $.trim($("#txtCategoryFullNameInsert").val())
            },
            contentType: 'application/json',
            success: function (result) {
                showNotification(result.responseText, result.success);
                $('#jqxgrid').jqxGrid('updatebounddata');
            },
            complete: function (result) {
                $('#dialodAddCategory').dialog('close');
            }
        });
    });//click
}//AddAthor

// Обработчик событий на клик кнопки, отклонающий добавление
function AddСancel() {
    $('#btnCanselAddCategory').click(function (e) {
        $('#dialodAddCategory').jqxValidator('hide');
        $('#dialodAddCategory').dialog('close');
    });
}//AddСancel

// Обработчик события изменения катрегории
function UpdateCategoryConfirm() {
    $('#btnConfirmUpdateCategory').click(function () {
        if (!$('#dialodUpdateCategory').jqxValidator('validate')) {
            return;
        }
        // Данные выбранной строки строки
        let rowindex = $('#jqxgrid').jqxGrid('getselectedrowindex');
        let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

        // Данные Измененные пользователем
        let data = {
            IdCategory: $('#idCategoryForUpdate').val(),
            CategoryName: $('#txtCategoryFullNameUpdate').val(),
            IsActive: $('#jqxcheckbox').val()
        };

        $.ajax({
            url: '/Admin/Categories/CategoryUpdate/',
            contentType: 'application/json',
            data: data,
            success: function (result) {
                // Измененные данные в редактируемой строке
                row.idCategory = data.IdCategory;
                row.CategoryFullName = data.СategoryName;
                row.isActive = data.IsActive;

                $('#jqxgrid').jqxGrid('updaterow', 0, row['uid']);

                showNotification(result.responseText, result.success);
            },

            complete: function (result) {
                $('#dialodUpdateCategory').dialog('close');
            }
        });
    });//click
}//UpdateCategory


// Обработчик событий на клик кнопки, отклонающий изменение
function UpdateСancel() {
    $('#btnCanselUpdateCategory').click(function (e) {
        $('#dialodUpdateCategory').jqxValidator('hide');
        $('#dialodUpdateCategory').dialog('close');
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
                { text: '№', datafield: 'idCategory', align: 'center', type: 'number', filterable: false, hidden: true },
                { text: 'Жанр', datafield: 'categoryName', width: '65%', align: 'center', type: 'string' },
                { text: 'Активен', datafield: 'isActive', columntype: 'checkbox', filtertype: 'bool', width: '35%', align: 'center' }
            ]
        });

}//CreateTable

// Получение данных из контроллера
function GetData() {
    let source = {
        datatype: 'json',
        datafield: [
            { name: 'idCategory' },
            { name: 'categoryName' },
            { name: 'isActive' }
        ],
        url: '/Admin/Categories/GetCategories/',
        id: 'idCategory'
    };
    var dataAdapter = new $.jqx.dataAdapter(source, {
        downloadComplete: function (edata, textStatus, jqXHR) {
        }
    });
    return dataAdapter;
}//GetData


// Обработчик события на клик кнопки, изменения записи
function UpdateCategory(rowindex) {

    // Получить данные из выбранной строчки
    let row = $('#jqxgrid').jqxGrid('getrowdata', rowindex);

    //Заполнение элементов формы для изменения
    $('#idCategoryForUpdate').val(row.idCategory);
    $('#txtCategoryFullNameUpdate').val(row.categoryName);
    $('#jqxcheckbox').val(row.isActive);

    $('#dialodUpdateCategory').dialog('open');
}

// Только русские символы и пробел на полях ввода
function onlyLetters() {
    $("#txtCategoryFullNameInsert, #txtCategoryFullNameUpdate").on("input", function () {
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
    $('#dialodAddCategory').dialog({
        autoOpen: false,
        modal: true,
        width: 400,
        height: 230,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Добавить Автора',
        close: function () {
            $('#dialodAddCategory input[type=text]').val('');
            $('#dialodAddCategory').jqxValidator('hide');
        }
    });

    //Создание модального окна изменение
    $('#dialodUpdateCategory').dialog({
        autoOpen: false,
        modal: true,
        width: 400,
        height: 250,
        resizable: false,
        dialogClass: 'modal-dialog',
        title: 'Изменить Автора',
        close: function () {
            $('#dialodUpdateCategory input[type=hidden]').val('');
            $('#dialodUpdateCategory input[type=text]').val('');
            $('#dialodUpdateCategory').jqxValidator('hide');
        }
    });

    // Валидация При Добавлении
    $('#dialodAddCategory').jqxValidator({
        focus: false,
        hintType: "label",
        rules:
            [
                {
                    input: '#txtCategoryFullNameInsert',
                    message: 'Поле  должно быть заполнено!',
                    action: 'keyup',
                    rule: 'required'
                },
                {
                    input: '#txtCategoryFullNameInsert',
                    message: 'Текст от 2 до 100 символов',
                    action: 'keyup',
                    rule: 'length=2,100'
                }


            ]
    });

    // Валидация При Изменении
    $('#dialodUpdateCategory').jqxValidator({
        focus: false,
        hintType: "label",
        rules:
            [
                {
                    input: '#txtCategoryFullNameUpdate',
                    message: 'Поле  должно быть заполнено!',
                    action: 'keyup',
                    rule: 'required'
                },
                {
                    input: '#txtCategoryFullNameUpdate',
                    message: 'Текст от 2 до 100 символов',
                    action: 'keyup',
                    rule: 'length=2,100'
                }
            ]
    });


};//window.onload 