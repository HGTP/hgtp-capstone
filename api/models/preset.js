'use strict';

const { Model } = require('sequelize');

module.exports = (sequelize, DataTypes) => {
  class Preset extends Model {
    /**
     * Helper method for defining associations.
     * This method is not a part of Sequelize lifecycle.
     * The `models/index` file will call this method automatically.
     */
    static associate(models) {
      models.Preset.hasMany(models.GestureSetting, {
        foreignKey: 'presetId',
      });
    }
  }
  Preset.init({
    name: {
      allowNull: false,
      type: DataTypes.STRING,
    },
    userId: {
      allowNull: false,
      type: DataTypes.STRING,
    },
  }, {
    sequelize,
    modelName: 'Preset',
  });
  return Preset;
};
