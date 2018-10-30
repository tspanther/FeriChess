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
				firstClick = false;
				prevClickedField = $(this).attr("data-pos");

				_url += "get-available-moves";
				request = new Field($(this).attr("data-pos"));

				method = "get-moves";
			}
			else {
				$("td").removeClass("available-move");

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