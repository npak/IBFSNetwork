//
$(function () {
      $(".mydatepicker").datepicker({
          format: 'MM.DD.YYYY',
          changeMonth: true,
          changeYear: true
      });

  });

$(function My1() {
    var i = 1;
    $('#addLink').click(function () {
        i++;
        if (i == 2)
            $("#addelement3").show();
        else if (i == 3)
            $("#addelement4").show();
        else
            alert("Error while adding additional IDs.");
    })
});
