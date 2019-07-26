
$(document).ready(function () {
    var st = viewBagCredentials;
    if (viewBagCredentials!="")
    {
        document.getElementById("messageDiv").style.display = "block";
        setTimeout(function () { document.getElementById("messageDiv").style.display = "none"; }, 2000);
        
    }
});
