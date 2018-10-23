$(document).ready(function(){
	var position = 1;
	for (var row=0; row<8; row++) {
		var col = "";
		for (var column=0; column<8; column++) { 
			col += "<td data-pos='"+position+"'></td>"; 
			position++; 
		}

		$("#chessboard").append("<tr>"+col+"</tr>");
    }
});