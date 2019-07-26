function showDoctorDetails()
{
    var encryptedToken = $("#hiddenToken").val();

    $.ajax({
        type: "GET",
        url: 'http://apiweb.com/api/DoctorAPI/',
        contentType: "application/json; charset=utf-8",
        headers: { "Token": encryptedToken },
        dataType: "json",
        success: function (data) {
            $('#tableDoctor').DataTable({
                data: data,
                columns: [
                    { 'data': 'DoctorID' },
                    { 'data': 'FirstName' },
                    { 'data': 'LastName' },
                    { 'data': 'EmailID' },
                    { 'data': 'RelatedHospital' },
                    { 'data': 'Specialty' },
                    { 'data': 'Address' },
                    { 'data': 'ContactNumber1' },
                    { 'data': 'ContactNumber2' },
                    { 'data': 'PrimaryDoctorMark' },
                    { "render": function () { return '<input type="button" class="btn btn-primary updateButtonClicked" name="updateButton" value="Edit">'; } },
                    { "render": function () { return '<input type="button" class="btn btn-danger deleteButtonClicked" name="deleteButton" value="Delete">'; } },
                ],
            });
        }
    });
}


$(document).ready(function () {
    
    //Populating data to datatable
    showDoctorDetails();

    //Uploading the value to DoctorDB from Modal by AJAX
    $("#bthSubmitDoctor").click(function () {

        var doctorModel = {
            "DoctorID": parseInt($("#txtDoctorID").val()),
            "FirstName": $("#txtFirstName").val(),
            "LastName": $("#txtLastName").val(),
            "EmailID": $("#txtEmail").val(),
            "RelatedHospital": $("#ddlPartialHospital").val(),
            "Specialty": $("#ddlPartialSpecialty").val(),
            "Address": $("#txtAddress").val(),
            "ContactNumber1": parseInt($("#txtContact1").val(),10),
            "ContactNumber2": parseInt($("#txtContact2").val(),10),
            "PrimaryDoctorMark": $("#ddlPrimaryMark").val()
        };
        $.ajax({
            url: 'http://apiweb.com/api/DoctorAPI/',
            data: JSON.stringify(doctorModel),
            headers: { "Token": encryptedToken },
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                alert("Data Saved");
                $("#tableDoctor").DataTable().destroy();
                showDoctorDetails();
            },
            error: function () {
                alert("Error");
            }
        });
            
    });
    //Updating Data 
    $("#tableDoctor").on("click", ".updateButtonClicked", function (event) {
        $("#bthSubmitDoctor").val("Update");
        $('#addDoctorDetails').modal('show');
        $('#idDoctorIDDiv').hide();
        var curRow = $(this).closest('tr');
        doctorID = curRow.find('td:eq(0)').text();
        $("#txtDoctorID").val(doctorID);
        $('#txtFirstName').val(curRow.find('td:eq(1)').text());
        $('#txtLastName').val(curRow.find('td:eq(2)').text());
        $('#txtEmail').val(curRow.find('td:eq(3)').text());
        $('#ddlPartialHospital').val(curRow.find('td:eq(4)').text()).change();
        $('#ddlPartialSpecialty').val(curRow.find('td:eq(5)').text()).change();
        $('#txtAddress').val(curRow.find('td:eq(6)').text());
        $('#txtContact1').val(curRow.find('td:eq(7)').text());
        $('#txtContact2').val(curRow.find('td:eq(8)').text());
        $('#ddlPrimaryMark').val(curRow.find('td:eq(9)').text()).change();
    });
    //Deleting Details
    $("#tableDoctor").on("click", ".deleteButtonClicked", function (event) {
        var curRow = $(this).closest('tr');
        doctorID = curRow.find('td:eq(0)').text();
        $('#deleteDoctorDetails').modal('show');
    });
    //Delete Confirmation
    $("#btnDeleteDoctor").click(function () {
        //alert(doctorID);
        var obj = {};
        obj.id = doctorID;
        $.ajax({
            type: 'DELETE',
            url: 'http://apiweb.com/api/DoctorAPI/',
            data: JSON.stringify(obj),
            headers: { "Token": encryptedToken },
            contentType: 'application/json',
            success: function (data) {
                //alert("Data Deleted");
                $("#tableDoctor").DataTable().destroy();
                showDoctorDetails();
            },
            error: function () {
                alert("Error");
            }
        });
    });
});


$(document).ajaxComplete(function () {
    $("#tableDoctor").find('tr').each(function (i, el) {
        var tds = $(this).find('td');
        if (tds.eq(9).text() == "Yes") {
            tds.eq(9).parent().css("background-color", "#38acff");
        }
    });
    $("#tableDoctor").find('tr').each(function (i, el) {
        var tds = $(this).find('td');
        tds.eq(9).css("display", "none");
    });
    $('th:eq(9)', this).css('display', 'none');
});