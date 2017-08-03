const path = require("path");
const fs = require("fs");

class Storage {
  constructor(opts) {
    const userDataPath = path.resolve(__dirname) + "/..";

    this.path = path.join(userDataPath, opts.configName + ".json");
    this.data = Storage.parseDataFile(this.path, opts.defaults);
  }

  get(key) {
    return this.data[key];
  }
  set(key, val) {
    this.data[key] = val;

    this.update();
  }
  update() {
    fs.writeFileSync(this.path, JSON.stringify(this.data));
  }

  static parseDataFile(filePath, defaults) {
    try {
      return JSON.parse(fs.readFileSync(filePath));
    } catch(error) {
      return defaults;
    }
  }
}
module.exports = Storage;
