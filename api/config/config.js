'use strict';

require('dotenv').config();
const fs = require('fs');
const path = require('path');

const config = {
  username: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  database: process.env.DB_NAME,
  host: process.env.DB_HOST,
  dialect: 'mariadb',
  dialectOptions: {
    timezone: 'Etc/GMT+0',
  },
  timezone: 'Etc/GMT+0',
};

if (process.env.NODE_ENV === 'production') {
  config.dialectOptions.ssl = {
    ca: fs.readFileSync(path.join(__dirname, 'BaltimoreCyberTrustRoot.crt.pem')),
  }
}

// Sequelize requires them by environment, but we'll be passing in the same code
// as the environment variables are what will define the values.
module.exports = {
  development: config,
  production: config,
};
