function galaxy(el, widthMax, heightMax) {
	var canvas = $(el);
	var ctx = null;

	init();

	function init(){
		if(canvas){
			bind();	
		}
	}

	function bind(){
		canvas.attr('width', widthMax);
		canvas.attr('height', heightMax);	
		ctx = $(el)[0].getContext("2d");	
	}

	function correctBounds(x, y, radius){
		var buffer = (radius + 5);

		// left and top bounds
		x = x - buffer < 0 ? x + buffer : x;
		y = y - buffer < 0 ? y + buffer : y;

		// bottom and right bounds
		x = x + buffer > heightMax ? x - buffer : x;
		y = y + buffer > widthMax ? y - buffer : y;

		return [x, y];
	}

	this.drawPlanet = function(x, y, radius) {		
		if(ctx){
			var bounds = correctBounds(x, y, radius)
			ctx.beginPath();
			ctx.arc(bounds[0], bounds[1], radius, 0, Math.PI*2, true);						
			ctx.strokeStyle = "green";
			ctx.stroke();			

			ctx.closePath();
			ctx.fillStyle = "white";
  			ctx.font = "14px Arial";
			ctx.fillText(parseInt(radius / 2), bounds[0] - (parseInt(radius / 2) >= 10 ? 8 : 4), bounds[1] + 5);
			
			
			
		}
	}
}
