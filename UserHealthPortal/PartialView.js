$(document).ready(function () {
    //Hospital List
    $.ajax({
        type: "POST",
        url: '/Doctor/GetDDLHospital',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#ddlPartialHospital').find('option').remove();
            var optionhtml1 = '<option value="' +
                0 + '">' + "--Select Hospital--" + '</option>';
            $("#ddlPartialHospital").append(optionhtml1);

            $.each(data, function (i) {

                var optionhtml = '<option value="' +
                    data[i] + '">' + data[i] + '</option>';
                $("#ddlPartialHospital").append(optionhtml);
            });
        },
        error: function () {
            alert(" An error occurred.");
        }
    });
    //Doctor List
    $.ajax({
        type: "POST",
        url: '/Report/GetDDLDoctor',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#ddlPartialDoctor').find('option').remove();
            var optionhtml1 = '<option value="' +
                0 + '">' + "--Select Doctor--" + '</option>';
            $("#ddlPartialDoctor").append(optionhtml1);

            $.each(data, function (i) {

                var optionhtml = '<option value="' +
                    data[i] + '">' + data[i] + '</option>';
                $("#ddlPartialDoctor").append(optionhtml);
            });
        },
        error: function () {
            alert(" An error occurred.");
        }
    });


    //Specialty
    $.ajax({
        type: "POST",
        url: '/Doctor/GetDDlSpecialty',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#ddlPartialSpecialty').find('option').remove();
            var optionhtml1 = '<option value="' +
                0 + '">' + "--Select Specialty--" + '</option>';
            $("#ddlPartialSpecialty").append(optionhtml1);

            $.each(data, function (i) {

                var optionhtml = '<option value="' +
                    data[i] + '">' + data[i] + '</option>';
                $("#ddlPartialSpecialty").append(optionhtml);
            });
        },
        error: function () {
            alert(" An error occurred.");
        }
    });
});