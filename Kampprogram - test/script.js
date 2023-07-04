let bool = true;

async function getMatches() {
  let container = document.getElementById('container');
  
  
  while(bool) {
    try {
      const response = await fetch('https://localhost:7085/Stadium/GetStadium?stadiumId=1');
      const data = await response.json();
      
      const arrayLength = data.matchList.length;

      for (var i = 0; i < arrayLength; i++) {
          const homeTeam = data.matchList[i].homeTeam;
          const awayTeam = data.matchList[i].awayTeam;
          const matchDate = data.matchList[i].matchDate;

          let div = document.createElement('div');
          let button = document.createElement('button');
          button.id = 'myButton';
          button.textContent = 'Køb billeter';
            button.onclick = () => {
  
            };
          
          div.className = 'box';
          div.textContent = homeTeam + " mod " + awayTeam + " " + matchDate;

          container.appendChild(div);
          div.appendChild(button);
          
          bool = false;
      }
    } catch (error) {
        console.log('Error:', error);
      }
  }
};






  