'use strict';

const { Model } = require('sequelize');

module.exports = (sequelize, DataTypes) => {
  class FeatureRequestVote extends Model {
    /**
     * Helper method for defining associations.
     * This method is not a part of Sequelize lifecycle.
     * The `models/index` file will call this method automatically.
     */
    static associate(models) {
      models.FeatureRequestVote.belongsTo(models.FeatureRequest);
    }
  }

  FeatureRequestVote.init({
    featureRequestId: DataTypes.INTEGER,
    userId: {
      allowNull: false,
      type: DataTypes.STRING,
    },
  }, {
    sequelize,
    modelName: 'FeatureRequestVote',
  });
  return FeatureRequestVote;
};
