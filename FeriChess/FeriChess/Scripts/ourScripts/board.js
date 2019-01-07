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
;
        }   
    });

    for (var row = 8; row >= 1; row--) {
        var col = "";
        for (var column = 1; column <= 8; column++) {
            col += "<td class='field' data-pos='" + column + row + "' data-figure='na'></td>";
        }

        $("#chessboard").append("<tr>" + col + "</tr>");
    }

    $.ajax({
        type: "GET",
        url: "api/game/load-boardstate/0",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var fieldUpdates = data;
            var pos;
            var fig;
            for (var i = 0; i < fieldUpdates.length; i++) {
                change = fieldUpdates[i];
                pos = change.Field.X.toString() + change.Field.Y.toString();
                if (change.PopulateBy != null) {
                    fig = change.PopulateBy.toLowerCase();
                }
                else {
                    fig = "na";
                }
                $("td[data-pos=" + pos + "]").attr("data-figure", fig);
            }
        },
        failure: function (data) {
            alert("shit");
        },
        error: function (data) {
            alert("shitshit");
        }
    });

    var firstClick = true;
    var prevClickedField = "";

    function Move(pos1, pos2) {
        var self = this;
        self.From = pos1;
        self.To = pos2;
    }

    function Field(pos) {
        var self = this;
        self.X = parseInt(pos[0]);
        self.Y = parseInt(pos[1]);
    }

    $(".field").each(function () {
        $(this).on("click", function () {
            var _url = "api/game/";
            var request;
            var method; // to know, what type of response to render

            if (firstClick) {
                $(this).addClass("selected-field");
                
                firstClick = false;
                prevClickedField = $(this).attr("data-pos");

                _url += "get-available-moves";
                request = new Field($(this).attr("data-pos"));

                method = "get-available-moves";
            }
            else {
                $("td").removeClass("available-move");
                $("td").removeClass("selected-field");

                firstClick = true;
                request = new Move(new Field(prevClickedField), new Field($(this).attr("data-pos")));

                _url += "make-a-move";

                method = "make-a-move";
            }
            request = JSON.stringify(request);

            $.ajax({
                type: "POST",
                url: _url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: request,
                success: function (data) {
                    if (method == "get-available-moves") {
                        var fields = data;
                        var pos;
                        for (var i = 0; i < fields.length; i++) {
                            field = fields[i];
                            pos = field.X.toString() + field.Y.toString();
                            $("td[data-pos=" + pos + "]").addClass("available-move");
                        }
                    }
                    else if (method = "make-a-move") {
                        var fieldUpdates = data.UpdateFields;
                        var pos;
                        var fig;
                        for (var i = 0; i < fieldUpdates.length; i++) {
                            change = fieldUpdates[i];
                            pos = change.Field.X.toString() + change.Field.Y.toString();
                            if (change.PopulateBy != null) {
                                fig = change.PopulateBy.toLowerCase();
                            }
                            else {
                                fig = "na";
                            }
                            $("td[data-pos=" + pos + "]").attr("data-figure", fig);
                        }
                        var gameResult = data.GameResult;
                        if (gameResult!="") {
                            alert(gameResult);
                        }
                    }
                },
                failure: function (data) {
                    alert("shit");
                },
                error: function (data) {
                    alert("shitshit");
                }
            });
        });
    });
});

function clearBoard(){
    for (var i=1; i<=8; i++){
        for (var j=1; j<=8;j++){
            var pos = "";
            pos+=i;
            pos+=j;
            $("td[data-pos=" + pos + "]").attr("data-figure", "na");
        }
    }
}

function loadcustom(){
    clearBoard();
    $.ajax({
        type: "GET",
        url: "api/game/load-boardstate/1",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var fieldUpdates = data;
            var pos;
            var fig;
            for (var i = 0; i < fieldUpdates.length; i++) {
                change = fieldUpdates[i];
                pos = change.Field.X.toString() + change.Field.Y.toString();
                if (change.PopulateBy != null) {
                    fig = change.PopulateBy.toLowerCase();
                }
                else {
                    fig = "na";
                }
                $("td[data-pos=" + pos + "]").attr("data-figure", fig);
            }
        },
        failure: function (data) {
            alert("shit");
        },
        error: function (data) {
            alert("shitshit");
        }
    });
}