var seats = [];
var button;
var bookSeats;
var container;

function openChooseSeats() {
    openBlankWindow();
    populateChooseSeats();
}

function openBlankWindow() {
    bookSeats = document.createElement("div");
    bookSeats.setAttribute("class", "new-div");
    bookSeats.style.width = "700px";
    bookSeats.style.height = "600px";
    bookSeats.style.backgroundColor = "white";
    bookSeats.style.position = "absolute";
    bookSeats.style.top = "50%";
    bookSeats.style.left = "50%";
    bookSeats.style.transform = "translate(-50%, -50%)";
    bookSeats.style.borderRadius = "10px";

    container = document.createElement("div");
    container.setAttribute("class", "container");
    container.style.display = "flex";
    container.style.flexDirection = "column";
    container.style.justifyContent = "center";
    container.style.alignItems = "center";
    bookSeats.appendChild(container);
}

function populateChooseSeats() {
    seats = [];

    var showcase = document.createElement("ul");
    showcase.setAttribute("class", "showcase");
    bookSeats.appendChild(showcase);

    var showcaseItem1 = document.createElement("li");
    showcase.appendChild(showcaseItem1);

    var showcaseSeat1 = document.createElement("div");
    showcaseSeat1.setAttribute("class", "seat");
    showcaseItem1.appendChild(showcaseSeat1);

    var showcaseSeatLabel1 = document.createElement("small");
    showcaseSeatLabel1.textContent = "Tilgængelige";
    showcaseItem1.appendChild(showcaseSeatLabel1);

    var showcaseItem2 = document.createElement("li");
    showcase.appendChild(showcaseItem2);

    var showcaseSeat2 = document.createElement("div");
    showcaseSeat2.setAttribute("class", "seat selected");
    showcaseItem2.appendChild(showcaseSeat2);

    var showcaseSeatLabel2 = document.createElement("small");
    showcaseSeatLabel2.textContent = "Valgt";
    showcaseItem2.appendChild(showcaseSeatLabel2);

    var showcaseItem3 = document.createElement("li");
    showcase.appendChild(showcaseItem3);

    var showcaseSeat3 = document.createElement("div");
    showcaseSeat3.setAttribute("class", "seat occupied");
    showcaseItem3.appendChild(showcaseSeat3);

    var showcaseSeatLabel3 = document.createElement("small");
    showcaseSeatLabel3.textContent = "Optaget";
    showcaseItem3.appendChild(showcaseSeatLabel3);

    var screen = document.createElement("div");
    screen.setAttribute("class", "screen");
    container.appendChild(screen);

    button = document.createElement("button");
    button.setAttribute("class", "button");
    button.textContent = "Bekræft pladser";
    button.addEventListener("click", confirmSeats);
    bookSeats.appendChild(button);

    createSeats(container);
    selectSeats(container);

    document.body.appendChild(bookSeats);

    updateSelectedCount();

}

function createSeats(container) {
    for (var i = 0; i < 24; i++) {
        var row = document.createElement("div");
        row.setAttribute("class", "row");
        container.appendChild(row);

        for (var j = 0; j < 25; j++) {
            var seat = document.createElement("div");
            seat.setAttribute("class", "seat");
            row.appendChild(seat);
        }
    }

}

function selectSeats(container) {
    container.addEventListener("click", function (event) {
        var target = event.target;

        if (target.classList.contains("seat")) {
            target.classList.toggle("selected");
            var rowIndex = target.parentElement.rowIndex;
            var seatIndex = Array.from(target.parentElement.children).indexOf(target);
            var seatId = rowIndex + "-" + seatIndex;

            if (target.classList.contains("selected")) {
                seats.push(seatId);
            } else {
                seats = seats.filter(function (id) {
                    return id !== seatId;
                });
            }

            updateSelectedCount();
        }
    });

}



function updateSelectedCount() {
    var selectedCount = seats.length;

    if (selectedCount > 0) {
        button.textContent = "Bekræft " + selectedCount + " plads(er)";
        button.disabled = false;
    } else {
        button.textContent = "Ingen pladser vælgt";
        button.disabled = true;
    }

}

function confirmSeats() {
    var selectedCount = seats.length;

    if (selectedCount > 0) {
        var confirmationMessage = "Du har valgt " + selectedCount + " plads(er)";

        alert(confirmationMessage);
        document.body.removeChild(bookSeats);
        updateSelectedCount();
    } else {
        alert("Vælg mindst en plads.");
    }
}
