'use strict';

const { Model } = require('sequelize');

module.exports = (sequelize, DataTypes) => {
  class GestureSetting extends Model {
    /**
     * Helper method for defining associations.
     * This method is not a part of Sequelize lifecycle.
     * The `models/index` file will call this method automatically.
     */
    static associate(models) {
      models.GestureSetting.belongsTo(models.Preset);
    }
  }
  GestureSetting.init({
    gesture: {
      allowNull: false,
      type: DataTypes.STRING,
    },
    phoneAction: DataTypes.STRING,
    presetId: DataTypes.INTEGER,
  }, {
    sequelize,
    modelName: 'GestureSetting',
  });
  return GestureSetting;
};
