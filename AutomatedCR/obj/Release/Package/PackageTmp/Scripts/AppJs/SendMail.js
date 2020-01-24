$(document).ready(function () {
    if ('@ViewBag.Message' == 'Sent') {
        alert('Mail has been sent successfully');
    }
    $('#ddlTeacher').select2({
        placeholder: "Select Teacher",
        width: 'resolve'
    });
    $('#ddlTeacher').change(function () {
        alert($(this).val());
    });
    $('#ddlCourse').select2({
        placeholder: "Select Course",
        width: 'resolve'
    });
    ddlTeacher();
    ddlCourse();
    function ddlTeacher() {
        debugger
        $('#ddlTeacher').select2({
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
    }
    function ddlCourse() {
        debugger
        $('#ddlCourse').select2({
            placeholder: "Select Course",
            width: 'resolve',
            ajax: {
                url: '/Course/LoadDdlCourse',
                dataType: 'json',
                processResults: function (data) {
                    debugger
                    // Transforms the top-level key of the response object from 'items' to 'results'
                    var data_array = [];
                    data.forEach(function (value, key) {
                        data_array.push({ id: value.CourseId, text: value.Title })
                    });
                    return {
                        results: data_array
                    };
                }
            }
        });
    }
});