function GeneratePlanets(planetMax ,xMax, yMax, radiusMin, radiusMax){
	var arr = [];
	
	for (var i = 0; i < planetMax; i++) {
    	var obj = {
        	x: getRandomNumber(0, xMax),
        	y: getRandomNumber(0, yMax),
        	radius: getRandomNumber(radiusMin, radiusMax)
    	};
    	arr.push(obj);
	}
	return arr;
}

function getRandomNumber (min, max) {
    return Math.random() * (max - min) + min;
}