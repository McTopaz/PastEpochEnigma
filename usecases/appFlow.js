import { SplashDuration } from "../entities/constants.js";

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
  return fetch("view/base/base.html")
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

function showViewThen({ htmlUrl, cssUrl, scriptUrl, viewClass, duration, onComplete }) {
  const baseCssHref = "view/base/base.css";

  loadCssInDocument(baseCssHref);
  loadCssInDocument(cssUrl);

  loadHtmlInDocument(htmlUrl)
    .then(() => import(scriptUrl))
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

function showView({ htmlUrl, cssUrl, scriptUrl, viewClass }) {
  const baseCssHref = "view/base/base.css";

  loadCssInDocument(baseCssHref);
  loadCssInDocument(cssUrl);

  loadHtmlInDocument(htmlUrl)
    .then(() => import(scriptUrl))
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
    htmlUrl: "view/splash/splash.html",
    cssUrl: "view/splash/splash.css",
    scriptUrl: "/view/splash/splash.js",
    viewClass: "Splash",
    duration: SplashDuration,
    onComplete: showMain
  });
}

function showMain() {
  showView({
    htmlUrl: "view/main/main.html",
    cssUrl: "view/main/main.css",
    scriptUrl: "/view/main/main.js",
    viewClass: "Main"
  });
}
