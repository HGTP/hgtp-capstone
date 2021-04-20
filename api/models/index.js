'use strict';

const fs = require('fs');
const path = require('path');
const Sequelize = require('sequelize');
const basename = path.basename(__filename);

const configByEnv = require('../config/config.js');
const config = (process.env.NODE_ENV === 'production') ? configByEnv.production : configByEnv.development;
const sequelize = new Sequelize(config.database, config.username, config.password, config);

// Include all the models in this folder.
fs.readdirSync(__dirname)
  .filter(file => {
    return (file.indexOf('.') !== 0) && (file !== basename) && (file.slice(-3) === '.js');
  })
  .forEach(file => {
    const Model = require(path.join(__dirname, file));
    const model = Model(sequelize, Sequelize.DataTypes);
    sequelize[model.name] = model;
  });

Object.keys(sequelize).forEach(modelName => {
  if (sequelize[modelName].associate) {
    sequelize[modelName].associate(sequelize);
  }
});

module.exports = {
  sequelize,
  Sequelize,
};
