import { ViewPaths } from "/entities/urlPaths.js";
import { ViewParts } from "/entities/viewParts.js";
import { SplashDuration } from "/entities/constants.js";

let currentViewInstance = null;

export function runAppFlow() {
  const shouldShowSplash = false; // Change this to true to show the splash screen
  
  if (shouldShowSplash ) {
      showSplash();
  } else {
      showMain();
  }
}

export function showIntroduction() {
  showView({
    viewParts: ViewPaths.introduction,
    viewClass: "Introduction"
  })
}

function ensureBaseCssLoaded() {
  if (!document.querySelector(`link[data-base-css]`)) {
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = ViewPaths.base.css;
    link.setAttribute('data-base-css', '');
    document.head.appendChild(link);
  }
}

function loadCssInDocument(href) {
  document.querySelectorAll('link[data-view-css]').forEach(link => link.remove());

  if (!document.querySelector(`link[href="${href}"]`)) {
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = href;
    link.setAttribute('data-view-css', '');
    document.head.appendChild(link);
  }
}

function loadHtmlInDocument(viewHtmlPath) {
  return fetch(ViewPaths.base.html)
    .then(res => res.text())
    .then(baseHtml => {
      document.body.innerHTML = baseHtml;
      return fetch(viewHtmlPath);
    })
    .then(res => res.text())
    .then(viewHtml => {
      const content = document.getElementById("content");
      content.innerHTML = viewHtml;
    });
}

function showViewThen({ viewParts, viewClass, duration, onComplete }) {
  ensureBaseCssLoaded();
  loadCssInDocument(viewParts.css);

  loadHtmlInDocument(viewParts.html)
    .then(() => import(viewParts.script))
    .then((module) => {

      if (currentViewInstance && typeof currentViewInstance.destroy === "function") {
          console.log("Destroying current view instance:", currentViewInstance);
          currentViewInstance.destroy();
      }

      const ViewClass = module[viewClass];
      if (ViewClass && typeof ViewClass === "function") {
        currentViewInstance = new ViewClass();
        if (typeof currentViewInstance.init === "function") {
          currentViewInstance.init();
        }
      } else {
        console.warn("Invalid or missing view class:", viewClassName);
      }

      if (duration > 0 && typeof onComplete === "function") {
        setTimeout(onComplete, duration);
      }
    })
    .catch((error) => {
      console.error("Error while displaying view:", error);
      if (typeof onComplete === "function") {
        onComplete();
      }
    });
}

function showView({ viewParts, viewClass }) {
  ensureBaseCssLoaded();
  loadCssInDocument(viewParts.css);

  loadHtmlInDocument(viewParts.html)
    .then(() => import(viewParts.script))
    .then((module) => {

      if (currentViewInstance && typeof currentViewInstance.destroy === "function") {
        console.log("Destroying current view instance:", currentViewInstance);
        currentViewInstance.destroy();
      }

      const ViewClass = module[viewClass];
      if (ViewClass && typeof ViewClass === "function") {
        currentViewInstance = new ViewClass();
        if (typeof currentViewInstance.init === "function") {
          console.log("Initializing view:", viewClass);
          currentViewInstance.init();
        }
      } else {
        console.warn("Invalid or missing view class:", viewClassName);
      }
    })
    .catch((error) => {
      console.error("Error while displaying view:", error);
    });
}

function showBase() {
  loadCssInDocument(ViewPaths.base.css);

  fetch('/view/base/base.html')
    .then(response => response.text())
    .then(html => {
      document.body.innerHTML = html;

      import('/view/base/base.js')
        .then(module => {
            const ViewClass = module["Base"];
            const instance = new ViewClass();
            instance.init();
        });
    })
}

function showSplash() {
  showViewThen({
    viewParts: ViewPaths.splash,
    viewClass: "Splash",
    duration: SplashDuration,
    onComplete: showMain
  });
}

export function showMain() {
  showView({
    viewParts: ViewPaths.main,
    viewClass: "Main"
  });
}

export function showOptions() {
  showView({
    viewParts: ViewPaths.options,
    viewClass: "Options"
  });
}
