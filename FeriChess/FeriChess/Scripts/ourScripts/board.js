// function DefaultPositionOfFigures() {

 // var player1 = {1:"<img id='wrook1' class='figures' src='img/rook_white.svg'/>",2:"<img id='wknight1' class='figures' src='img/knight_white.svg'/>", 3:"<img class='figures' id='wbishop1' src='img/bishop_white.svg'/>",4:"<img class='figures' id='wqueen' src='img/queen_white.svg'/>",5:"<img id='wking' class='figures' src='img/king_white.svg'/>", 6:"<img class='figures' id='wbishop1' src='img/bishop_white.svg'/>", 7:"<img class='figures'id='wknight2' src='img/knight_white.svg'/>",8:"<img class='figures' src='img/rook_white.svg' id='wrook2'/>", 9:"<img class='figures' id='wpaw1' src='img/paw_white.svg'/>", 10:"<img class='figures' id='wpaw2' src='img/paw_white.svg'/>", 11:"<img class='figures' id='wpaw3' src='img/paw_white.svg'/>", 12:"<img class='figures' id='wpaw4' src='img/paw_white.svg'/>", 13:"<img class='figures' id='wpaw5' src='img/paw_white.svg'/>", 14:"<img class='figures' id='wpaw6' src='img/paw_white.svg'/>", 15:"<img class='figures' id='wpaw7' src='img/paw_white.svg'/>", 16:"<img class='figures' id='wpaw8' src='img/paw_white.svg'/>"};
		
		// var player2 = {57:"<img class='figures' id='brook1' src='img/rook_black.svg'/>",58:"<img class='figures' id='bknight1' src='img/knight_black.svg'/>", 59:"<img class='figures' id='bbishop1' src='img/bishop_black.svg'/>", 60:"<img class='figures' id='bqueen' src='img/queen_black.svg'/>", 61:"<img class='figures' id='bking' src='img/king_black.svg'/>", 62:"<img class='figures' id='bbishop2' src='img/bishop_black.svg'/>", 63:"<img class='figures' id='bknight2' src='img/knight_black.svg'/>",64:"<img class='figures' src='img/rook_black.svg' id='brook2'/>", 49:"<img class='figures' id='bpaw1' src='img/paw_black.svg'/>", 50:"<img class='figures' id='bpaw2' src='img/paw_black.svg'/>", 51:"<img class='figures' id='bpaw3' src='img/paw_black.svg'/>", 52:"<img class='figures' id='bpaw4' src='img/paw_black.svg'/>", 53:"<img class='figures' id='bpaw5' src='img/paw_black.svg'/>", 54:"<img class='figures' id='bpaw6' src='img/paw_black.svg'/>", 55:"<img class='figures' id='bpaw7' src='img/paw_black.svg'/>", 56:"<img class='figures' id='bpaw8' src='img/paw_black.svg'/>"};

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
	// BestPlayers();
	
	
    for (var row = 8; row >= 1; row--) {
        var col = "";
        for (var column = 1; column <= 8; column++) {
            col += "<td class='field' data-pos='" + column + row + "' data-figure='wp'></td>";
        }

        $("#chessboard").append("<tr>" + col + "</tr>");
    }
	
	// DefaultPositionOfFigures();
	
	$('td').click(function() {
		$(this).toggleClass('clikedField');
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


// function BestPlayers() {

	// for(i=0;i<11;i++)
	// {
		// $('#bestplayers').append( '<tr>' + 'result' +  i + '</tr>' );

		// for(ii=0;ii<2;ii++)
		// {
			// $('#bestplayers').append( '<td>' + 'result' +  i + '</tr>' );
		// }
	// }

//}