$(document).ready(function(){
	CreateChessboard();	
	DefaultPositionOfFigures();
	
	$('td').click(function() {
		$(this).toggleClass('clikedField');
	});
	
});

function CreateChessboard() {
	var position = 1;
	for (var row=0; row<8; row++) {
		var col = "";
		for (var column=0; column<8; column++) { 
			col += "<td data-pos='"+position+"'></td>"; 
			position++; 
		}
		
		$("#chessboard").append("<tr>"+col+"</tr>");
	}
}

function DefaultPositionOfFigures() {

		var player1 = {1:"<i class='fa fa-marg fa-chess-rook'>",2:"<i class='fa fa-marg fa-chess-knight'>", 3:"<i class='fa fa-marg fa-chess-bishop'>",4:"<i class='fa fa-marg fa-chess-queen'>",5:"<i class='fa fa-marg fa-chess-king'>", 6:"<i class='fa fa-marg fa-chess-bishop'>", 7:"<i class='fa fa-marg fa-chess-knight'>",8:"<i class='fa fa-marg fa-chess-rook'>", 9:"<i class='fa fa-marg fa-chess-pawn'>", 10:"<i class='fa fa-marg fa-chess-pawn'>", 11:"<i class='fa fa-marg fa-chess-pawn'>", 12:"<i class='fa fa-marg fa-chess-pawn'>", 13:"<i class='fa fa-marg fa-chess-pawn'>", 14:"<i class='fa fa-marg fa-chess-pawn'>", 15:"<i class='fa fa-marg fa-chess-pawn'>", 16:"<i class='fa fa-marg fa-chess-pawn'>"};
		
		var player2 = {57:"<i class='fa fa-white fa-marg fa-chess-rook'>",58:"<i class='fa fa-white fa-marg fa-chess-knight'>", 59:"<i class='fa fa-white fa-marg fa-chess-bishop'>", 60:"<i class='fa fa-white fa-marg fa-chess-queen'>", 61:"<i class='fa fa-white fa-marg fa-chess-king'>", 62:"<i class='fa fa-white fa-marg fa-chess-bishop'>", 63:"<i class='fa fa-white fa-marg fa-chess-knight'>",64:"<i class='fa fa-white fa-marg fa-chess-rook'>", 49:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 50:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 51:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 52:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 53:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 54:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 55:"<i class='fa fa-white fa-marg fa-chess-pawn'>", 56:"<i class='fa fa-white fa-marg fa-chess-pawn'>"};

		for (position in player1)
		{
			$('[data-pos="'+position+'"]').html(player1[position]);		 
		}
		
		for (position in player2)
		{
			$('[data-pos="'+position+'"]').html(player2[position]);	
		}
	
}
