function openWindow() {
    var bookSeats = document.createElement("div");
    bookSeats.setAttribute("class", "new-div");
    bookSeats.style.width = "700px";
    bookSeats.style.height = "600px";
    bookSeats.style.backgroundColor = "white";
    bookSeats.style.position = "absolute";
    bookSeats.style.top = "50%";
    bookSeats.style.left = "50%";
    bookSeats.style.transform = "translate(-50%, -50%)";
    bookSeats.style.borderRadius = "10px";

    var container = document.createElement("div");
    container.setAttribute("class", "container");
    container.style.display = "flex";
    container.style.flexDirection = "column";
    container.style.justifyContent = "center";
    container.style.alignItems = "center";
    bookSeats.appendChild(container);

    var showcase = document.createElement("ul");
    showcase.setAttribute("class", "showcase");
    bookSeats.appendChild(showcase);

    var showcaseItem1 = document.createElement("li");
    showcase.appendChild(showcaseItem1);

    var showcaseSeat1 = document.createElement("div");
    showcaseSeat1.setAttribute("class", "seat");
    showcaseItem1.appendChild(showcaseSeat1);

    var showcaseSeatLabel1 = document.createElement("small");
    showcaseSeatLabel1.textContent = "Avaliable";
    showcaseItem1.appendChild(showcaseSeatLabel1);

    var showcaseItem2 = document.createElement("li");
    showcase.appendChild(showcaseItem2);

    var showcaseSeat2 = document.createElement("div");
    showcaseSeat2.setAttribute("class", "seat selected");
    showcaseItem2.appendChild(showcaseSeat2);

    var showcaseSeatLabel2 = document.createElement("small");
    showcaseSeatLabel2.textContent = "Selected";
    showcaseItem2.appendChild(showcaseSeatLabel2);

    var showcaseItem3 = document.createElement("li");
    showcase.appendChild(showcaseItem3);

    var showcaseSeat3 = document.createElement("div");
    showcaseSeat3.setAttribute("class", "seat occupied");
    showcaseItem3.appendChild(showcaseSeat3);

    var showcaseSeatLabel3 = document.createElement("small");
    showcaseSeatLabel3.textContent = "Occupied";
    showcaseItem3.appendChild(showcaseSeatLabel3);

    var screen = document.createElement("div");
    screen.setAttribute("class", "screen");
    container.appendChild(screen);

    for (var i = 0; i < 24; i++) {
        var row = document.createElement("div");
        row.setAttribute("class", "row");
        container.appendChild(row);

        for (var j = 0; j < 25; j++) {
            var seat = document.createElement("div");
            seat.setAttribute("class", "seat");
            row.appendChild(seat);

            seat.addEventListener("click", function () {
                this.classList.toggle("selected");
                updateSelectedCount();
            });
        }
    }

    document.body.appendChild(bookSeats);

    function updateSelectedCount() {
        document.querySelectorAll(".seat.selected");

    }
}
