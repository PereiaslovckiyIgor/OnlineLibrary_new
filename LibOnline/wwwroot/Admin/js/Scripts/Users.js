'use strict';

$(document).ready(function () {

    $("#mainDiv").css('visibility', 'visible');

    CreateTable();


});


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
            id: 'IdUser'
        };
        var dataAdapter = new $.jqx.dataAdapter(source, {
            downloadComplete: function (edata, textStatus, jqXHR) {
            }
        });
        return dataAdapter;
    }//GetData
