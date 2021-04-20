'use strict';

// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

const {featureRequestsService: service} = require('../services');

/**
 * Handles DELETE /feature-requests/:featureRequestId.
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
 const deleteFeatureRequest = async (req, res) => {
  try {
    await service.deleteFeatureRequest(req.params);
    res.sendStatus(204);
  } catch (err) {
    console.log(err);
    res.sendStatus(500);
  }
};

/**
 * Handles DELETE /feature-requests/votes/:featureRequestId
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
 const deleteFeatureRequestVote = async (req, res) => {
  try {
    await service.removeFeatureRequestVote(req.user.email, req.params);
    res.sendStatus(204);
  } catch (err) {
    console.log(err);
    res.sendStatus(500);
  }
};

/**
 * Handles GET /feature-requests?status=:status
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
const getFeatureRequests = async (req, res) => {
  try {
    const result = await service.getFeatureRequests(req.user.email, req.query.status);
    res.status(200).send(result);
  } catch (err) {
    console.log(err);
    res.sendStatus(500);
  }
};

/**
 * Handles POST /feature-requests/votes
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
 const postFeatureRequest = async (req, res) => {
  try {
    await service.createFeatureRequest(req.user.email, req.body);
    res.sendStatus(204);
  } catch (err) {
    console.log(err);
    res.sendStatus(500);
  }
};

/**
 * Handles POST /feature-requests
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
const postFeatureRequestVote = async (req, res) => {
  try {
    await service.addFeatureRequestVote(req.user.email, req.body);
    res.sendStatus(201);
  } catch (err) {
    if (err.name === 'SequelizeUniqueConstraintError') {
      res.sendStatus(204);
    } else {
      console.log(err);
      res.sendStatus(500);
    }
  }
};

/**
 * Handles PUT /feature-requests
 * 
 * Only intended for updating the status.
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
const putFeatureRequest = async (req, res) => {
  try {
    await service.updateFeatureRequestStatus(req.body);
    res.sendStatus(204);
  } catch (err) {
    console.log(err);
    res.sendStatus(500);
  }
};

module.exports = (app, authGuard) => {
  app.delete('/feature-request/:featureRequestId', authGuard(['Gestr Admins']), deleteFeatureRequest);
  app.delete('/feature-request/vote/:featureRequestId', authGuard(['Everyone']), deleteFeatureRequestVote);
  app.get('/feature-requests', authGuard(['Everyone']), getFeatureRequests);
  app.post('/feature-request', authGuard(['Everyone']), postFeatureRequest);
  app.post('/feature-request/vote', authGuard(['Everyone']), postFeatureRequestVote);
  app.put('/feature-request', authGuard(['Gestr Admins']), putFeatureRequest);
};
