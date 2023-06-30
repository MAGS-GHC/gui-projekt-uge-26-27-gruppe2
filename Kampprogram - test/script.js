function createDivElements() {
    var container = document.getElementById('container');

    for (var i = 0; i < 5; i++) {
        var div = document.createElement('div');
        div.className = 'box';
        div.textContent = "Arena + Hold + Tidspunkt"

        container.appendChild(div);
    }
}