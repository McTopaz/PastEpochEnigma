import { Base } from "../base/base.js";
import { GameSubTitle } from "/entities/constants.js";

const menuLogo = "/entities/asciiArts/menuLogo.txt";

export class Main extends Base {
  init() {
    super.init();
    this.#logo();
    document.getElementById("subTitle").textContent = GameSubTitle;
  }

  #logo() {
   fetch(menuLogo)
      .then(res => res.text())
      .then(logo => {
        document.getElementById("asciiLogo").textContent = logo;
      })
      .catch(err => {
        console.error("Unable to load logo:", err);
      });
  }
}