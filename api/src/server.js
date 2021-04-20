'use strict';

require('dotenv').config();
const express = require('express');
const swaggerUi = require('swagger-ui-express');
const swaggerDocument = require('./swagger.json');
const errorHandler = require('./errors');
const bodyParser = require('body-parser');
const cors = require('cors');

// Setting up express API.
const app = express();
app.use(cors());
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerDocument));
app.use(bodyParser.json({strict: true}));
app.use(errorHandler);

// Setting up routes.
require('./routes')(app);
app.get('/', (req, res) => {
  res.redirect('/api-docs');
});
app.get('/liveness', (req, res) => {
  res.sendStatus(200);
});
app.get('/readiness', (req, res) => {
  res.sendStatus(200);
});

// Starting API.
app.listen(process.env.PORT, () => {
  console.log(`Serving API at http://localhost:${process.env.PORT}`);
});
