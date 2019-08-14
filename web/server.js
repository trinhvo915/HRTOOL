var express = require("express");
var path = require("path");
var https = require("https");
var http = require("http");
var fs = require("fs");

var app = express();

app.use(express.static(__dirname + "/build"));

app.get("*", function(request, response) {
  response.sendFile(path.resolve(__dirname, "build/index.html"));
});

var httpServer = http.createServer(app);

httpServer.listen(1975);
