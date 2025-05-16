import { initializeApp } from "./usecases/initApp.js";
import { runAppFlow } from "./usecases/appFlow.js";
import { BASE_PATH } from "./entities/urlPaths.js"
import { initKeyboardListeners } from './view/listeners/keyboardListener.js';

console.log("Base path:", BASE_PATH);

window.addEventListener("DOMContentLoaded", () => {
  initializeApp();
  initKeyboardListeners();
  runAppFlow();
});