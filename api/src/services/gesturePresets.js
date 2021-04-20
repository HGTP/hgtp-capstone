'use strict';

// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

const {sequelize} = require('../../models');
const {GestureSetting, Preset} = sequelize;

/**
 * Deletes a user's gesture setting.
 * 
 * @param {String} userId
 * @param {String} presetName
 * @param {String} gesture
 */
const deleteGestureSetting = async (userId, {gesture, presetName}) => {
  const {id: presetId} = await Preset.findOne({where: {userId, name: presetName}});
  await GestureSetting.upsert({gesture, phoneAction: null, presetId}, {
    fields: ['phoneAction'],
  });
};

/**
 * Gets the phone action a user has assigned to the given gesture.
 * 
 * @param {String} userId
 * @param {String} presetName
 * @param {String} gesture
 * @return {String}
 */
const getGestureSetting = async (userId, {gesture, presetName}) => {
  const result = await GestureSetting.findAll({
    attributes: ['phoneAction'],
    where: {
      presetId: [sequelize.literal(`SELECT id FROM Presets WHERE name='${presetName}' AND userId='${userId}'`)],
      gesture: gesture,
    },
  });
  const phoneAction = (result[0]) ? result[0].phoneAction : '';
  return phoneAction;
};

/**
 * Gets all the gesture settings for the given user.
 * 
 * @param {String} userId
 * @return {Promise}
 */
const getUserGestureSettings = async (userId, presetName) => {
  const entries = await GestureSetting.findAll({
    attributes: ['gesture', 'phoneAction'],
    include: [Preset],
    where: {
      presetId: [sequelize.literal(`SELECT id FROM Presets WHERE userId='${userId}' AND name = '${presetName}'`)],
    },
  });
  return entries.map((entry) => {
    return {
      gesture: entry.gesture,
      phoneAction: entry.phoneAction,
      presetName: entry.Preset.name,
    };
  });
};

/**
 * Helper function essentially for upserting a preset.
 * 
 * @param {String} userId 
 * @param {String} presetName 
 * @returns 
 */
const _findOrCreatePreset = async (userId, presetName) => {
  const result = await Preset.findOne({where: {userId, name: presetName}});
  if (!result) {
    const preset = await Preset.create({
      userId,
      name: presetName,
    });
    return preset.id;
  } else {
    return result.id;
  }
};

/**
 * Updates the phone action for the given gesture.
 * 
 * @param {String} userId
 * @param {String} gesture 
 * @param {String} phoneAction
 */
const updateGestureSetting = async (userId, {gesture, presetName}, {phoneAction}) => {
  const presetId = await _findOrCreatePreset(userId, presetName);
  await GestureSetting.upsert({gesture, phoneAction, presetId}, {
    fields: ['phoneAction'],
  });
};

module.exports = {
  deleteGestureSetting,
  getGestureSetting,
  getUserGestureSettings,
  updateGestureSetting,
};
