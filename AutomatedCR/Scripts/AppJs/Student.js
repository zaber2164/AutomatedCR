﻿$(document).ready(function () {

    var StudentId;
    $('#student-list tbody').on('click', 'td .student-update', function () {

        var $row = $(this).closest("tr");
        debugger;
        if ($row.hasClass('child')) {
            $row = $row.prev();
        }

        var cell = $(this).closest('td');
        var stdId = $row.find("#hdnId").val();
        StudentId = stdId;
        window.localStorage.setItem('StudentId', stdId);
        var url = "/Student/GetStudentById";
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: true,
            data: "{'Id':'" + stdId + "'}",
            success: function (result) {
                debugger;
                var output = $.parseJSON(result);

                var Name = output.Name;
                var Email = output.Email;
                var PhoneNumber = output.PhoneNumber;

                $('#Name').val(Name);
                $('#Email').val(Email);
                $('#PhoneNumber').val(PhoneNumber);

                $('#UpdateModalForm').modal('show');

            },
            error:
                function (request, status, error) {
                    alert("Failed to save. Error Details -  Request: " +
                        request +
                        ", Status: " +
                        status +
                        ", Error: " +
                        error);
                }
        });
    });

    $('.new-student')
    .on('click',
        function () {
            //$('#addcanteen').select2({ placeholder: "Select Canteen" });
            $('#AddModalForm').modal('show');
            //loadCanteenNameCombo('#addcanteen');
            $('#tablecontainer').hide();
        });

    $(document).on('click', '.Update', function () {
        //event.preventDefault();
        debugger;
        var jsonobject = createjsonobjectForUpdate();
        if (jsonobject != null) {
            var url = '/Student/Update/';
            saveData(url, jsonobject);
        }
    });

    $(document).on('click', '.add', function () {
        debugger;
        var jsonobject = createjsonobjectForAdd();
        if (jsonobject != null) {
            var url = '/Student/Add/';
            saveData(url, jsonobject);
        }
    });

    function createjsonobjectForAdd() {

        debugger;
        var obj = new Object();
        var d = new Date();

        var month = d.getMonth() + 1;
        var day = d.getDate();

        var currentdate = d.getFullYear() + '/' +
            (month < 10 ? '0' : '') + month + '/' +
            (day < 10 ? '0' : '') + day;

        obj.StudentId = StudentId;
        obj.Name = $('#addName').val();
        obj.Email = $('#addEmail').val();
        obj.PhoneNumber = $('#addPhoneNumber').val();
        obj.UpdatedDate = currentdate;
        obj.UpdatedBy = StudentId;

        return obj;
    }

    function createjsonobjectForUpdate() {

        debugger;
        var obj = new Object();
        var d = new Date();

        var month = d.getMonth() + 1;
        var day = d.getDate();

        var currentdate = d.getFullYear() + '/' +
            (month < 10 ? '0' : '') + month + '/' +
            (day < 10 ? '0' : '') + day;

        //obj.StudentId = StudentId;
        obj.StudentId = window.localStorage.getItem('StudentId');
        obj.Name = $('#Name').val();
        obj.Email = $('#Email').val();
        obj.PhoneNumber = $('#PhoneNumber').val();
        obj.UpdatedDate = currentdate;
        obj.UpdatedBy = StudentId;

        return obj;
    }

    function saveData(url, jsonobject) {
        debugger;
        //var dataa = JSON.stringify({ PostData: jsonobject });
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: true,
            data: JSON.stringify({ Data: jsonobject }),
            success: function (result) {
                debugger
                if (result === 'success') {
                    //window.location.reload(true);
                    //window.location.href = '/Student';
                    //window.location = "https://www.aspsnippets.com/";

                    swal("Data Operation Successful !", "", "success");
                    $('#UpdateModalForm').modal('hide');
                    $('#AddModalForm').modal('hide');
                    UpdateTableData();
                }
            },
            error:
                function (request, status, error) {
                    alert("Failed to save. Error Details -  Request: " +
                        request +
                        ", Status: " +
                        status +
                        ", Error: " +
                        error);
                    //alert("Failed to save. Error Details -  Request: " + request + ", Status: " + status + ", Error: " + error);
                }
        });
    }

    function UpdateTableData() {
        debugger;
        $.ajax({
            type: "POST",
            url: 'Student/UpdateTableData',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: true,
            data: JSON.stringify({}),
            success: function (result) {
                debugger
                var content = '';
                var table = '';
                var tableStart = '<table id="Grid" class="table table-borderless table-data3" style="overflow-x:auto">' +
                                            '<thead>' +
                                                '<tr>' +
                                                    '<th style="width:30%">Student Name</th>' +
                                                    '<th style="width:20%">Email</th>' +
                                                    '<th style="width:25%">Phone Number</th>' +
                                                    '<th style="width:20%">Action</th>' +
                                                '</tr>' +
                                            '</thead>'
                var tabeEnd = '</table>'
                if (result != null) {
                    $.each(result, function (i, obj) {
                        content += '<tbody>' + '<tr>' + '<td>' + obj.Name + '<input type="hidden" value="' + obj.StudentId + '" id="hdnhdnId" class="hidden-CardMapId" />' + '</td><td>' + obj.Email + '</td><td>' + obj.PhoneNumber + '</td><td>' +
                            '<div class="table-data-feature"><button class="item student-update-2" data-toggle="tooltip" data-placement="top" title="Edit" onclick="GetDataById(' + obj.StudentId + ')";><i class="zmdi zmdi-edit"></i></button><button class="item" data-toggle="tooltip" data-placement="top" title="Delete"><i class="zmdi zmdi-delete"></i></button></div>' +
                            '</td>' + '</tr>' + '</tbody>';
                    });
                }
                else {
                    content += '<tr><td colspan="10"><h1 style="color:red;text-align: center;">No Data Found</h1></td></tr>'
                }
                tableStart += content + tabeEnd;
                table += tableStart;
                $("#Grid").empty()
                $('#updatedTable').html(table);
                $('#updatedTable').show();
                $('#initialGrid').remove();
            },
            error:
                function (request, status, error) {
                    alert("Failed to save. Error Details -  Request: " +
                        request +
                        ", Status: " +
                        status +
                        ", Error: " +
                        error);
                }
        });
    }

});
function GetDataById(StdId) {

    var url = "/Student/GetStudentById";
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        data: "{'Id':'" + StdId + "'}",
        success: function (result) {
            debugger;
            var output = $.parseJSON(result);

            window.localStorage.setItem('StudentId', StdId);
            //alert(window.localStorage.getItem('StudentId'));
            var Name = output.Name;
            var Email = output.Email;
            var PhoneNumber = output.PhoneNumber;

            $('#Name').val(Name);
            $('#Email').val(Email);
            $('#PhoneNumber').val(PhoneNumber);

            $('#UpdateModalForm').modal('show');

        },
        error:
            function (request, status, error) {
                alert("Failed to save. Error Details -  Request: " +
                    request +
                    ", Status: " +
                    status +
                    ", Error: " +
                    error);
            }
    });
}