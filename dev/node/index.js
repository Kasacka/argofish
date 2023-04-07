const http = require("http");
const server = http.createServer((req, res) => {
    const date = new Date();
    const payload = {
        date: date.toJSON(),
        version: process.env.SERVICE_VERSION
    };
    res.end(JSON.stringify(payload));
});
server.listen(process.env.SERVICE_PORT || 8080);