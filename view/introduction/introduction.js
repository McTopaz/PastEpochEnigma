import { Base } from "../base/base.js";

const menuLogo = "/entities/asciiArts/introductionLogo.txt";

export class Introduction extends Base {
  init() {
    super.init();
    this.#logo();
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