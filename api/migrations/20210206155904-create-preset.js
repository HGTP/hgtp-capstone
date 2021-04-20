'use strict';

const PRESETS_TABLE_NAME = 'Presets';
const PRESETS_USER_PRESETNAME_CONSTRAINT = 'user-presetname_uq';

module.exports = {
  up: async (queryInterface, Sequelize) => {
    await queryInterface.createTable(PRESETS_TABLE_NAME, {
      id: {
        allowNull: false,
        autoIncrement: true,
        primaryKey: true,
        type: Sequelize.INTEGER,
      },
      name: {
        allowNull: false,
        type: Sequelize.STRING,
      },
      userId: {
        allowNull: false,
        type: Sequelize.STRING,
      },
      createdAt: {
        allowNull: false,
        type: Sequelize.DATE,
      },
      updatedAt: {
        allowNull: false,
        type: Sequelize.DATE,
      },
    });

    await queryInterface.addConstraint(PRESETS_TABLE_NAME, {
      fields: ['name', 'userId'],
      name: PRESETS_USER_PRESETNAME_CONSTRAINT,
      type: 'unique',
    });
  },
  
  // Add Sequelize as second parameter if needed.
  down: async (queryInterface) => {
    await queryInterface.dropTable(PRESETS_TABLE_NAME);
  },
};
