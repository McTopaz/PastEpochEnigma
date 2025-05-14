const repoName = "PastEpochEnigma";
export const BASE_PATH = window.location.pathname.includes(`/${repoName}/`)
  ? `/${repoName}/`
  : "/";

export const ViewPaths = {
  baseHtml: `${BASE_PATH}view/base/base.html`,
  baseCss: `${BASE_PATH}view/base/base.css`,
  baseScript: `${BASE_PATH}view/base/base.js`,
  splashHtml: `${BASE_PATH}view/splash/splash.html`,
  splashCss: `${BASE_PATH}view/splash/splash.css`,
  splashScript: `${BASE_PATH}view/splash/splash.js`,
  mainHtml: `${BASE_PATH}view/main/main.html`,
  mainCss: `${BASE_PATH}view/main/main.css`,
  mainScript: `${BASE_PATH}view/main/main.js`
};

export const asciiPaths = {
  horizontalLogo: `${BASE_PATH}entities/asciiArts/horizontalSplashLogo.txt`,
  verticalLogo: `${BASE_PATH}entities/asciiArts/verticalSplashLogo.txt`,
  menuLogo: `${BASE_PATH}entities/asciiArts/menuLogo.txt`
};