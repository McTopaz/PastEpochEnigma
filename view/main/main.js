import { showOptions } from "../../usecases/appFlow.js";
import { Base } from "../base/base.js";
import { GameSubTitle } from "/entities/constants.js";
import { showIntroduction } from "/usecases/appFlow.js";

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

  hanldeKeyboardEvent(event) {
    if (event.key === "3" || event.key.toUpperCase() === "I") {
      showIntroduction();
    }
    else if (event.key === "4" || event.key.toUpperCase() === "O") {
      showOptions();
    }
  }
}