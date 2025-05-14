export class ViewParts {
  constructor(name, basePath = "") {
    this.html = `${basePath}view/${name}/${name}.html`;
    this.css = `${basePath}view/${name}/${name}.css`;
    this.script = `${basePath}view/${name}/${name}.js`;
  }
}