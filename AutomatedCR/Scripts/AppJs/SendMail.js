$(document).ready(function () {
  if ('@ViewBag.Message' == 'Sent') {
    alert('Mail has been sent successfully');
  }
  $('#addTeacher').select2({
    placeholder: "Select Teacher",
    width: 'resolve'
  });
  $('#addTeacher').change(function (){
    alert($(this).val());
  });
});