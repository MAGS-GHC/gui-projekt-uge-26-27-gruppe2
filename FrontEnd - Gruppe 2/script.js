const homebutton = document.querySelector(".home home1");


var link = ""
function getData(){
    var xhttp = new XMLHttpRequest ();
    xhttp.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            var obj = JSON.parse(this.response);
            document.getElementById("title").innerHTML = obj.Title;
        }
    };
    xhttp.open("GET", link, true);
    xhttp.send();
}
getData();