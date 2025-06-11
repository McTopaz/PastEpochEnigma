import { GameVersion, Createdby } from "/entities/constants.js";

export class Base {
  constructor() {
  }

  init() {
    this._onKeyDown = this._onKeyDown.bind(this);
    this.attachKeyboard();
    
    document.getElementById("version").textContent = `Version: ${GameVersion}`;
    document.getElementById("createdBy").textContent = `Created by: ${Createdby}`;
  }

  destroy() {
    this.detachKeyboard();
  }

  _onKeyDown(event) {
    this.hanldeKeyboardEvent(event);
  }

  hanldeKeyboardEvent(event) {
    // This method can be overridden by subclasses to handle keyboard events
  }

  attachKeyboard() {
    document.addEventListener('keydown', this._onKeyDown);
  }

  detachKeyboard() {
    document.removeEventListener('keydown', this._onKeyDown);
  }
}

