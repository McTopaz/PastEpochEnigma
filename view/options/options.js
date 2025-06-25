import { Base } from "../base/base.js";
import { showMain } from "/usecases/appFlow.js";
import { settings } from "/entities/settings.js";

const menuLogo = "/entities/asciiArts/optionsLogo.txt";

export class Options extends Base {
  init() {
    super.init();
    this.#logo();
    this.#applySettings();
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

  #applySettings() {
    const difficultRadios = document.querySelectorAll('input[name="difficulty"]');
    difficultRadios.forEach(radio => {
      radio.checked = (radio.value === settings.difficulty);
    });

    const modeRadios = document.querySelectorAll('input[name="mode"]');
    modeRadios.forEach(radio => {
      radio.checked = (radio.value === settings.mode);
    });
  }

  hanldeKeyboardEvent(event) {
    if (event.key === "Escape" || event.key === "Enter") {
      showMain();
    }
    else if (event.key === "1" || event.key.toUpperCase() === "D") {
      const difficulties = ["easy", "medium", "hard"];
      settings.difficulty = this.#cycleOptions(difficulties, settings.difficulty);
      this.#applySettings();
    }
    else if (event.key === "2" || event.key.toUpperCase() === "M") {
      const modes = ["normal", "speedrun"];
      settings.mode = this.#cycleOptions(modes, settings.mode);
      this.#applySettings();
    }
  }

  #cycleOptions(options, currentValue) {
    const currentIndex = options.indexOf(currentValue);
    const nextIndex = (currentIndex + 1) % options.length;
    return options[nextIndex];
  }
}