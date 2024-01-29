const PROXY_CONFIG = [
  {
    context: [
      "/api/folder",
    ],
    target: "https://localhost:7251",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
