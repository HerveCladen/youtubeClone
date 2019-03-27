/*Terms and conditions checkbox validation*/
var defaultRangeValidator = $.validator.methods.range;
$.validator.methods.range = function (value, element, param) {
    if (element.type === 'checkbox') {
        return element.checked;
    } else {
        return defaultRangeValidator.call(this, value, element, param);
    }
}
/*********************************************************************/

/*Only allow commenting if the textbox isn't empty*/
function verifier() {
    if (document.getElementById("commentaire").value === "") {
        document.getElementById('btnPoster').disabled = true;
    } else {
        document.getElementById('btnPoster').disabled = false;
    }
}
/************************************************************/ 


/*Recently uploaded scroll buttons*/
var button = document.getElementById('slide');
button.onclick = function () {
    var container = document.getElementById('container');
    sideScroll(container, 'right', 25, 100, 10);
};

var back = document.getElementById('slideBack');
back.onclick = function () {
    var container = document.getElementById('container');
    sideScroll(container, 'left', 25, 100, 10);
};

function sideScroll(element, direction, speed, distance, step) {
    scrollAmount = 0;
    var slideTimer = setInterval(function () {
        if (direction == 'left') {
            element.scrollLeft -= step;
        } else {
            element.scrollLeft += step;
        }
        scrollAmount += step;
        if (scrollAmount >= distance) {
            window.clearInterval(slideTimer);
        }
    }, speed);
}
/**********************************************************/