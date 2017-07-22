const electron = require('electron').remote;
const path = require("path");

class Storage {
  constructor(opts) {
    const userDataPath = app.getPath("userData");

    this.path = path.join(userDataPath, opts.configName + ".json");
    this.data = Storage.parseDataFile(this.path, opts.defaults);
  }

  get(key) {
    return this.data[key];
  }
  set(key, val) {
    this.data[key] = val;

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
