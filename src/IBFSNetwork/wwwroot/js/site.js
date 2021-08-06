    $(document).ready(function () {
        $("#CountryHidden").hide();
        var currenDate = new Date();
        $(".mydatepicker".datepicker("setDate", currenDate));
        //$(".selector").datepicker("setDate", "10/12/2012");

        $("#myLocation").change(function () {
            var str = "";
            $("#myLocation").each(function () {
                str += $(this).find(":selected").text();

                if (str == "Other")
                    $('#CountryHidden').show();
                else
                    $('#CountryHidden').hide();
            });
        });

    })
