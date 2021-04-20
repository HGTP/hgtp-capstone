'use strict';

const { Model } = require('sequelize');

module.exports = (sequelize, DataTypes) => {
  class FeatureRequest extends Model {
    /**
     * Helper method for defining associations.
     * This method is not a part of Sequelize lifecycle.
     * The `models/index` file will call this method automatically.
     */
    static associate(models) {
      models.FeatureRequest.hasMany(models.FeatureRequestVote, {
        foreignKey: 'featureRequestId',
      });
    }
  }

  FeatureRequest.init({
    description: {
      allowNull: false,
      type: DataTypes.STRING,
    },
    requestorId: {
      allowNull: false,
      type: DataTypes.STRING,
    },
    status: {
      defaultValue: 'unplanned',
      type: DataTypes.ENUM,
      values: ['unplanned', 'planned', 'completed'],
    },
    tags: {
      allowNull: true,
      type: DataTypes.STRING,
    },
  }, {
    sequelize,
    modelName: 'FeatureRequest',
  });
  return FeatureRequest;
};
