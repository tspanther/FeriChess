$(document).ready(function () {
	for (var row = 0; row < 8; row++) {
		var col = "";
		for (var column = 0; column < 8; column++) {
			col += "<td class=\"field\" data-pos='" + row + column + "'></td>";
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

			if (firstClick) {
				firstClick = false;
				prevClickedField = $(this).attr("data-pos");

				_url += "get-available-moves";
				request = new Field($(this).attr("data-pos"));
			}
			else {
				firstClick = true;
				request = new Move(new Field(prevClickedField), new Field($(this).attr("data-pos")));

				_url += "make-a-move";
			}
			request = JSON.stringify(request);

			$.ajax({
				type: "POST",
				url: _url,
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				data: request,
				success: function (data) {
					alert(JSON.stringify(data));
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