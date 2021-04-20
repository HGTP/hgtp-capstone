'use strict';

// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

const {gesturePresetsService: service} = require('../services');

/**
 * Handles DELETE /presets/:presetName/:gesture requests.
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
const deleteGestureSetting = async (req, res) => {
  try {
    await service.deleteGestureSetting(req.user.email, req.params);
    res.sendStatus(204);
  } catch (err) {
    console.log(err);
    res.sendStatus(500);
  }
};

/**
 * Handles GET /presets/:presetName/:gesture requests.
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
const getGestureSetting = async (req, res) => {
  try {
    const phoneAction = await service.getGestureSetting(req.user.email, req.params);
    res.status(200).send({
      gesture: req.params.gesture,
      phoneAction: phoneAction ? phoneAction : '',
    });
  } catch (err) {
    console.log(err);
    res.sendStatus(500);
  }
};

/**
 * Handles GET /user/gesture-settings requests.
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
const getUserGestureSettings = async (req, res) => {
  try {
    const result = await service.getUserGestureSettings(req.user.email, req.query.presetName);
    res.status(200).send(result);
  } catch (err) {
    console.log(err);
    res.sendStatus(500);
  }
};

/**
 * Handles PUT /presets/:presetName/:gesture requests.
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
const putGestureSetting = async (req, res) => {
  try {
    await service.updateGestureSetting(req.user.email, req.params, req.body);
    res.sendStatus(204);
  } catch (err) {
    console.log(err);
    res.sendStatus(500);
  }
};

module.exports = (app, authGuard) => {
  app.delete('/preset/:presetName/:gesture', authGuard(['Everyone']), deleteGestureSetting);
  app.get('/preset/:presetName/:gesture', authGuard(['Everyone']), getGestureSetting);
  app.get('/user/gesture-settings', authGuard(['Everyone']), getUserGestureSettings);
  app.put('/preset/:presetName/:gesture', authGuard(['Everyone']), putGestureSetting);
};
