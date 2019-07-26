$(document).ready(function () {
    $("#btnUpdateInformation").on("click", function () {
        var content =@Html.Raw(Json.Encode(ViewBag.content));
        alert("");
    });
});