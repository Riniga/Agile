const openbrowser = require('open');
const express = require('express')
const app = express()
const port = 3053

app.use(express.static(__dirname + '/public'));

app.listen(port, () => {
  console.log(`Server listening at http://localhost:${port}`)
    openbrowser("http://localhost:3053")
})
