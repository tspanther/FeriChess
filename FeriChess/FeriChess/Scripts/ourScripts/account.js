 $(document).ready(function () {
     $("#login-btn").click(function () {
         if ($("#Login").hasClass("closeform")) {
             $("#Login").removeClass("closeform").addClass("openform");
         } else {
             $("#Login").removeClass("openform").addClass("closeform");
         }
     });

     $("#signin-btn").click(function () {
         if ($("#Signin").hasClass("closeform")) {
             $("#Signin").removeClass("closeform").addClass("openform");
         } else {
             $("#Signin").removeClass("openform").addClass("closeform");
         }
     });
 });

function login(){
    var type = 0;
    var usrn = document.getElementById("loginusrn").value;
    var email = document.getElementById("loginemail").value;
    var passwd = document.getElementById("loginpass").value;
    var auth = new AuthenticationRequest(type, usrn, email, passwd);
    sendRequest(auth);
}

function signin(){
    var type = 1;
    var usrn = document.getElementById("signinusrn").value;
    var email = document.getElementById("signinemail").value;
    var passwd = document.getElementById("signinpass").value;
    var auth = new AuthenticationRequest(type, usrn, email, passwd);
    sendRequest(auth);
}

function AuthenticationRequest(type, usrn, email, password) {
    var self = this;
    self.Type = type;
    self.Username = usrn;
    self.Email = email;
    self.Password = password;
}

function sendRequest(requestData) {
    console.log(requestData);
    $.ajax({
        type: "POST",
        url: "api/account/authenticate",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(requestData),
        success: function (data) {
            alert(data.Response);
        },
        failure: function (data) {
            console.log("failure");
        },
        error: function (data) {
            console.log("error");
        }
    });
}