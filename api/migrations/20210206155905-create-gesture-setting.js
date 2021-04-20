'use strict';

const GESTURE_SETTINGS_TABLE_NAME = 'GestureSettings';
const PRESETS_TABLE_NAME = 'Presets';

const GESTURE_SETTINGS_GESTURE_PRESETID_CONSTRAINT = 'gesture-presetid_uq';
const GESTURE_SETTINGS_PRESETID_CONSTRAINT = 'presetid-fk';

module.exports = {
  up: async (queryInterface, Sequelize) => {
    await queryInterface.createTable(GESTURE_SETTINGS_TABLE_NAME, {
      id: {
        allowNull: false,
        autoIncrement: true,
        primaryKey: true,
        type: Sequelize.INTEGER,
      },
      gesture: {
        allowNull: false,
        type: Sequelize.STRING,
      },
      phoneAction: {
        type: Sequelize.STRING,
      },
      presetId: {
        allowNull: false,
        type: Sequelize.INTEGER,
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

    await queryInterface.addConstraint(GESTURE_SETTINGS_TABLE_NAME, {
      fields: ['gesture', 'presetId'],
      name: GESTURE_SETTINGS_GESTURE_PRESETID_CONSTRAINT,
      type: 'unique',
    });

    await queryInterface.addConstraint(GESTURE_SETTINGS_TABLE_NAME, {
      fields: ['presetId'],
      name: GESTURE_SETTINGS_PRESETID_CONSTRAINT,
      references: {
        table: PRESETS_TABLE_NAME,
        field: 'id',
      },
      type: 'foreign key',
    });
  },

  // Add Sequelize as second parameter if needed.
  down: async (queryInterface) => {
    await queryInterface.dropTable(GESTURE_SETTINGS_TABLE_NAME);
  },
};
