import { GameSubTitle } from "/entities/constants.js";

const logos = [
    "/entities/asciiArts/horizontalSplashLogo.txt",
    "/entities/asciiArts/verticalSplashLogo.txt"
  ];

export function showRandomSplash() {
  const randomIndex = Math.floor(Math.random() * logos.length);
  const chosenLogo = logos[randomIndex];

  fetch(chosenLogo)
    .then(res => res.text())
    .then(logo => {
      document.getElementById("asciiLogo").textContent = logo;
    })
    .catch(error => {
      console.error("Unable to load splash logo", error);
    });
}

showRandomSplash();
document.getElementById("subTitle").textContent = GameSubTitle;