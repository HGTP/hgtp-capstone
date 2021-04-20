'use strict';

const FEATURE_REQUESTS_TABLE_NAME = 'FeatureRequests';

module.exports = {
  up: async (queryInterface, Sequelize) => {
    await queryInterface.createTable(FEATURE_REQUESTS_TABLE_NAME, {
      id: {
        allowNull: false,
        autoIncrement: true,
        primaryKey: true,
        type: Sequelize.INTEGER
      },
      description: {
        allowNull: false,
        type: Sequelize.STRING,
      },
      requestorId: {
        allowNull: false,
        type: Sequelize.STRING,
      },
      status: {
        defaultValue: 'unplanned',
        type: Sequelize.ENUM,
        values: ['unplanned', 'planned', 'completed'],
      },
      tags: {
        allowNull: true,
        type: Sequelize.STRING,
      },
      createdAt: {
        allowNull: false,
        type: Sequelize.DATE
      },
      updatedAt: {
        allowNull: false,
        type: Sequelize.DATE
      }
    });
  },
  
  // Add Sequelize as second parameter if needed.
  down: async (queryInterface) => {
    await queryInterface.dropTable(FEATURE_REQUESTS_TABLE_NAME);
  }
};