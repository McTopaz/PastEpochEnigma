import { ViewPaths } from "/entities/urlPaths.js";
import { ViewParts } from "/entities/viewParts.js";
import { SplashDuration } from "/entities/constants.js";

export function runAppFlow() {
    const shouldShowSplash  = true;

    if (shouldShowSplash ) {
        showSplash();
    } else {
        showMain();
    }
}

function loadCssInDocument(href) {
    if (!document.querySelector(`link[href="${href}"]`)) {
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = href;
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
  loadCssInDocument(ViewPaths.base.css);
  loadCssInDocument(viewParts.css);

  loadHtmlInDocument(viewParts.html)
    .then(() => import(viewParts.script))
    .then((module) => {

    const ViewClass = module[viewClass];
    
    if (ViewClass && typeof ViewClass === "function") {
      const viewInstance = new ViewClass();
      if (typeof viewInstance.init === "function") {
        viewInstance.init();
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
  loadCssInDocument(ViewPaths.base.css);
  loadCssInDocument(viewParts.css);

  loadHtmlInDocument(viewParts.html)
    .then(() => import(viewParts.script))
    .then((module) => {

      const ViewClass = module[viewClass];
      
      if (ViewClass && typeof ViewClass === "function") {
        const viewInstance = new ViewClass();
        if (typeof viewInstance.init === "function") {
          viewInstance.init();
        }
      } else {
        console.warn("Invalid or missing view class:", viewClassName);
      }
    })
    .catch((error) => {
      console.error("Error while displaying view:", error);
    });
}

function showSplash() {
  showViewThen({
    viewParts: ViewPaths.splash,
    viewClass: "Splash",
    duration: SplashDuration,
    onComplete: showMain
  });
}

function showMain() {
  showView({
    viewParts: ViewPaths.main,
    viewClass: "Main"
  });
}
