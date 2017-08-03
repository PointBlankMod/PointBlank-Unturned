const request = require('sync-request');

class WebData {
  static DownloadText(url) {
    return request("GET", url).getBody().toString();
  }
}

module.exports = WebData;
