$(document).ready(function () {

    var CourseId;
    $('#Course-list tbody').on('click', 'td .Course-update', function () {

        var $row = $(this).closest("tr");
        debugger;
        if ($row.hasClass('child')) {
            $row = $row.prev();
        }

        var cell = $(this).closest('td');
        var crsId = $row.find("#hdnId").val();
        CourseId = crsId;
        window.localStorage.setItem('CourseId', crsId);
        $('#Teacher').select2({
            placeholder: "Select Teacher",
            width: 'resolve',
            ajax: {
                url: '/Course/LoadDdlTeacher',
                dataType: 'json',
                processResults: function (data) {
                    debugger
                    // Transforms the top-level key of the response object from 'items' to 'results'
                    var data_array = [];
                    data.forEach(function (value, key) {
                        data_array.push({ id: value.TeacherId, text: value.Name })
                    });
                    return {
                        results: data_array
                    };
                }
            }
        });
        var url = "/Course/GetCourseById";
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: true,
            data: "{'Id':'" + crsId + "'}",
            success: function (result) {
                debugger;
                var output = $.parseJSON(result);

                var Title = output.Title;
                var Semester = output.Semester;
                var Location = output.Location;
                var TeacherId = output.TeacherId;

                $('#Title').val(Title);
                $('#Semester').val(Semester);
                $('#Location').val(Location);
                //$('#Teacher').val(TeacherId);
                $('#Teacher').select2('data', { id: TeacherId, text: 'test' });

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

    $('.new-Course')
    .on('click',
        function () {
            $('#addTeacher').select2({
                placeholder: "Select Teacher",
                width: 'resolve',
                ajax: {
                    url: '/Course/LoadDdlTeacher',
                    dataType: 'json',
                    processResults: function (data) {
                        debugger
                        // Transforms the top-level key of the response object from 'items' to 'results'
                        var data_array = [];
                        data.forEach(function (value, key) {
                            data_array.push({ id: value.TeacherId, text: value.Name })
                        });
                        return {
                            results: data_array
                        };
                    }
                }
            });
            $('#AddModalForm').modal('show');
            //loadCanteenNameCombo('#addcanteen');
            $('#tablecontainer').hide();
        });

    $(document).on('click', '.Update', function () {
        //event.preventDefault();
        debugger;
        var jsonobject = createjsonobjectForUpdate();
        if (jsonobject != null) {
            var url = '/Course/Update/';
            saveData(url, jsonobject);
        }
    });

    $(document).on('click', '.add', function () {
        debugger;
        var jsonobject = createjsonobjectForAdd();
        if (jsonobject != null) {
            var url = '/Course/Add/';
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

        obj.CourseId = CourseId;
        obj.Title = $('#addName').val();
        obj.Semester = $('#addSemester').val();
        obj.Location = $('#addLocation').val();
        obj.TeacherId = $("#addTeacher").val();
        //obj.UpdatedDate = currentdate;
        //obj.UpdatedBy = CourseId;

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

        //obj.CourseId = CourseId;
        obj.CourseId = window.localStorage.getItem('CourseId');
        obj.Title = $('#Title').val();
        obj.Semester = $('#Semester').val();
        obj.Location = $('#Location').val();
        //obj.TeacherId = $("#addTeacher").val();
        //obj.UpdatedDate = currentdate;
        //obj.UpdatedBy = CourseId;

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
                    //window.location.href = '/Course';
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
            url: '/Course/UpdateTableData',
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
                                                    '<th style="width:30%">Course Name</th>' +
                                                    '<th style="width:20%">Email</th>' +
                                                    '<th style="width:25%">Phone Number</th>' +
                                                    '<th style="width:20%">Action</th>' +
                                                '</tr>' +
                                            '</thead>'
                var tabeEnd = '</table>'
                if (result != null) {
                    $.each(result, function (i, obj) {
                        content += '<tbody>' + '<tr>' + '<td>' + obj.Title + '<input type="hidden" value="' + obj.CourseId + '" id="hdnhdnId" class="hidden-CardMapId" />' + '</td><td>' + obj.Semester + '</td><td>' + obj.Location + '</td><td>' +
                            '<div class="table-data-feature"><button class="item Course-update-2" data-toggle="tooltip" data-placement="top" title="Edit" onclick="GetDataById(' + obj.CourseId + ')";><i class="zmdi zmdi-edit"></i></button><button class="item" data-toggle="tooltip" data-placement="top" title="Delete"><i class="zmdi zmdi-delete"></i></button></div>' +
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
                    alert("Failed to Load Course Data. Error Details -  Request: " +
                        request +
                        ", Status: " +
                        status +
                        ", Error: " +
                        error);
                }
        });
    }

});
function GetDataById(crsId) {

    var url = "/Course/GetCourseById";
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        data: "{'Id':'" + crsId + "'}",
        success: function (result) {
            debugger;
            var output = $.parseJSON(result);

            window.localStorage.setItem('CourseId', crsId);
            var Title = output.Title;
            var Semester = output.Semester;
            var Location = output.Location;

            $('#Title').val(Title);
            $('#Semester').val(Semester);
            $('#Location').val(Location);

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