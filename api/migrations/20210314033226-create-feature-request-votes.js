'use strict';

const FEATURE_REQUEST_VOTES_TABLE_NAME = 'FeatureRequestVotes';
const FEATURE_REQUESTS_TABLE_NAME = 'FeatureRequests';

const FEATURE_REQUEST_VOTES_FEATURE_REQUEST_ID_USER_ID = 'featurerequestid-userid_uq';
const FEATURE_REQUEST_VOTES_FEATURE_REQUEST_ID_CONSTRAINT = 'featurerequestid-fk';

module.exports = {
  up: async (queryInterface, Sequelize) => {
    await queryInterface.createTable(FEATURE_REQUEST_VOTES_TABLE_NAME, {
      id: {
        allowNull: false,
        autoIncrement: true,
        primaryKey: true,
        type: Sequelize.INTEGER
      },
      featureRequestId: Sequelize.INTEGER,
      userId: {
        allowNull: false,
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

    await queryInterface.addConstraint(FEATURE_REQUEST_VOTES_TABLE_NAME, {
      fields: ['featureRequestId', 'userId'],
      name: FEATURE_REQUEST_VOTES_FEATURE_REQUEST_ID_USER_ID,
      type: 'unique',
    });

    await queryInterface.addConstraint(FEATURE_REQUEST_VOTES_TABLE_NAME, {
      fields: ['featureRequestId'],
      name: FEATURE_REQUEST_VOTES_FEATURE_REQUEST_ID_CONSTRAINT,
      references: {
        table: FEATURE_REQUESTS_TABLE_NAME,
        field: 'id',
      },
      type: 'foreign key',
    });
  },

  // Add Sequelize as second parameter if needed.
  down: async (queryInterface) => {
    await queryInterface.dropTable(FEATURE_REQUEST_VOTES_TABLE_NAME);
  }
};