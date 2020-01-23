$(document).ready(function () {
  if ('@ViewBag.Message' == 'Sent') {
    alert('Mail has been sent successfully');
  }
  $('#ddlTeacher').select2({
    placeholder: "Select Teacher",
    width: 'resolve'
  });
  $('#ddlTeacher').change(function (){
    alert($(this).val());
  });
  $('#ddlCourse').select2({
      placeholder: "Select Course",
      width: 'resolve'
  });
});