// function DefaultPositionOfFigures() {

// var player1 = {1:"<i class='fa fa-marg fa-chess-rook'>",2:"<i class='fa fa-marg fa-chess-knight'>", 3:"<i class='fa fa-marg fa-chess-bishop'>",4:"<i class='fa fa-marg fa-chess-queen'>",5:"<i class='fa fa-marg fa-chess-king'>", 6:"<i class='fa fa-marg fa-chess-bishop'>", 7:"<i class='fa fa-marg fa-chess-knight'>",8:"<i class='fa fa-marg fa-chess-rook'>", 9:"<i class='fa fa-marg fa-chess-pawn'>", 10:"<i class='fa fa-marg fa-chess-pawn'>", 11:"<i class='fa fa-marg fa-chess-pawn'>", 12:"<i class='fa fa-marg fa-chess-pawn'>", 13:"<i class='fa fa-marg fa-chess-pawn'>", 14:"<i class='fa fa-marg fa-chess-pawn'>", 15:"<i class='fa fa-marg fa-chess-pawn'>", 16:"<i class='fa fa-marg fa-chess-pawn'>"};

// var player2 = {57:"<i class='fa fa-white fa-marg fa-chess-rook'>",58:"<i class='fa fa-white fa-marg fa-chess-knight'>", 59:"<i class='fa fa-white fa-marg fa-chess-bishop'>", 60:"<i class='fa fa-white fa-marg fa-chess-queen'>", 61:"<i class='fa fa-white fa-marg fa-chess-king'>", 62:"<i class='fa fa-white fa-marg fa-chess-bishop'>", 63:"<i class='fa fa-white fa-marg fa-chess-knight'>",64:"<i class='fa fa-white fa-marg fa-chess-rook'>", 49:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 50:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 51:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 52:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 53:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 54:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 55:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 56:"<i class='fa fa-white fa-marg fa-chess-pawn'>"};

// for (position in player1)
// {
// $('[data-pos="'+position+'"]').html(player1[position]);		 
// }

// for (position in player2)
// {
// $('[data-pos="'+position+'"]').html(player2[position]);	
// }

// }

$(document).ready(function () {
    for (var row = 1; row <= 8; row++) {
        var col = "";
        for (var column = 1; column <= 8; column++) {
            col += "<td class='field' data-pos='" + row + column + "' data-figure='wp'></td>";
        }

        $("#chessboard").append("<tr>" + col + "</tr>");
    }

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

                method = "get-moves";
            }
            else {
                $("td").removeClass("available-move");
                $("td").removeClass("selected-field");

                firstClick = true;
                request = new Move(new Field(prevClickedField), new Field($(this).attr("data-pos")));

                _url += "make-a-move";

                method = "make-move";
            }
            request = JSON.stringify(request);

            $.ajax({
                type: "POST",
                url: _url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: request,
                success: function (data) {
                    if (method == "get-moves") {
                        var availableFields = data;
                        var pos;
                        for (var i = 0; i < availableFields.length; i++) {
                            field = availableFields[i];
                            pos = field.X.toString() + field.Y.toString();
                            $("td[data-pos=" + pos + "]").addClass("available-move");
                        }
                    }
                    else if (method = "make-move") {
                        var availableFields = data;
                        var pos;
                        var fig;
                        for (var i = 0; i < availableFields.length; i++) {
                            change = availableFields[i];
                            pos = change.Field.X.toString() + change.Field.Y.toString();
                            if (change.PopulateBy != null) {
                                fig = change.PopulateBy.toLowerCase();
                            }
                            else {
                                fig = "na";
                            }
                            $("td[data-pos=" + pos + "]").attr("data-figure", fig);

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