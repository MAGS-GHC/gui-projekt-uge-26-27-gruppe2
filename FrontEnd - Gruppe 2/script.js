

const homebutton = document.querySelector(".home home1");


// var link = ""
// function getData(){
//     var xhttp = new XMLHttpRequest ();
//     xhttp.onreadystatechange = function() {
//         if (this.readyState == 4 && this.status == 200) {
//             var obj = JSON.parse(this.response);
//             document.getElementById("title").innerHTML = obj.Title;
//         }
//     };
//     xhttp.open("GET", link, true);
//     xhttp.send();
// }
// getData();
fetch('https://localhost:44342/Stadium/GetTicketOrder?id=1')
.then(response => response.json())
 .then(data => {
 // Handle the API response data
 console.log(data);
 document.getElementById('order').innerHTML = (data.orderNumber);
  })
.catch(error => {
  // Handle any errors that occurred during the API call
console.error('Error:', error);
  });