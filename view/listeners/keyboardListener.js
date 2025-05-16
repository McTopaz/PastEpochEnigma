import { showIntroduction } from "/usecases/appFlow.js";

export function initKeyboardListeners() {
  document.addEventListener('keydown', (event) => {
    const key = event.key.toUpperCase();

    if (key === 'I' || key === '3') {
      showIntroduction();
    }
  });
}