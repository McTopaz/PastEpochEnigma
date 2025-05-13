import { Base } from "../base/base.js";
import { GameSubTitle } from "/entities/constants.js";

const logos = [
  "/entities/asciiArts/horizontalSplashLogo.txt",
  "/entities/asciiArts/verticalSplashLogo.txt"
];

export class Splash extends Base {
  init() {
    super.init();
    this.#randomLogo();
    document.getElementById("subTitle").textContent = GameSubTitle;
  }

  #randomLogo() {
    const randomIndex = Math.floor(Math.random() * logos.length);
    const chosenLogo = logos[randomIndex];

    fetch(chosenLogo)
      .then(res => res.text())
      .then(logo => {
        document.getElementById("asciiLogo").textContent = logo;
      })
      .catch(err => {
        console.error("Kunde inte ladda splash-logo:", err);
      });
  }
}