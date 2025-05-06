export function runAppFlow() {
    const SHOW_SPLASH = true;
  
    if (SHOW_SPLASH) {
      showSplash();
    } else {
      showMainGame();
    }
  }
  
  function showSplash() {
    fetch("view/splash/splash.html")
      .then(res => res.text())
      .then(html => {
        const container = document.createElement("div");
        container.innerHTML = html;
        document.body.prepend(container);
  
        window.startGame = () => {
          container.remove();
          showMainGame();
        };
      });
  }
  
  function showMainGame() {
    fetch("view/main/main.html")
      .then(res => res.text())
      .then(html => {
        const mainContainer = document.createElement("div");
        mainContainer.innerHTML = html;
        document.body.appendChild(mainContainer);
      });
  }