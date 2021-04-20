'use strict';

// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

const {sequelize} = require('../../models');
const {FeatureRequest, FeatureRequestVote} = sequelize;

/**
 * Adds the user's vote to the feature request.
 * 
 * @param {String} userId
 * @param {String} featureRequestId
 */
const addFeatureRequestVote = async (userId, {featureRequestId}) => {
  const featureRequest = await FeatureRequest.findOne({where: {id: featureRequestId}});
  return FeatureRequestVote.create({
    featureRequestId: featureRequest.id,
    userId,
  });
};

/**
 * Creates a new feature request.
 * 
 * @param {String} requestorId 
 * @param {String} description
 * @param {Array} tags 
 * @return {Promise}
 */
const createFeatureRequest = (requestorId, {description, tags}) => {
  return FeatureRequest.create({
    description,
    requestorId,
    tags: tags.join(','),
  });
};

/**
 * Deletes all votes for a given feature.
 * 
 * @param {String} featureRequestId 
 */
const _deleteFeatureRequestVotes = async (featureRequestId) => {
  await FeatureRequestVote.destroy({
    where: {
      featureRequestId,
    },
  });
};

/**
 * Deletes the feature request.
 * 
 * @param {String} featureRequestId
 */
 const deleteFeatureRequest = async ({featureRequestId}) => {
   await _deleteFeatureRequestVotes(featureRequestId);
   return FeatureRequest.destroy({
     where: {
       id: featureRequestId,
     },
   });
};

/**
 * Gets all the feature requests.
 * 
 * @param {String} userId
 * @param {String} status (optional)
 */
const getFeatureRequests = async (userId, status) => {
  const options = {
    include: FeatureRequestVote,
    where: (status) ? {status} : undefined,
  };
  const result = await FeatureRequest.findAll(options);
  return result.map((featureRequest) => {
    let userVotedForThisFeature = false;
    featureRequest.FeatureRequestVotes.forEach((vote) => {
      if (vote.userId === userId) {
        userVotedForThisFeature = true;
      }
    });
    return {
      createdAt: featureRequest.createdAt,
      description: featureRequest.description,
      id: featureRequest.id,
      requestorId: featureRequest.requestorId,
      status: featureRequest.status,
      tags: featureRequest.tags.split(','),
      votes: featureRequest.FeatureRequestVotes.length,
      userVotedForThisFeature,
    };
  });
};

/**
 * Removes a user's vote on the feature request.
 * 
 * @param {String} userId
 * @param {String} featureRequestId
 */
const removeFeatureRequestVote = async (userId, {featureRequestId}) => {
  return FeatureRequestVote.destroy({
    where: {
      featureRequestId,
      userId,
    },
  });
};

/**
 * Updates the status on the feature request.
 * 
 * @param {String} featureRequestId
 * @param {String} newStatus
 */
const updateFeatureRequestStatus = async ({featureRequestId, status}) => {
  const featureRequest = await FeatureRequest.findOne({where: {id: featureRequestId}});
  if (featureRequest) {
    featureRequest.status = status;
    return featureRequest.save();
  }
};

module.exports = {
  addFeatureRequestVote,
  createFeatureRequest,
  deleteFeatureRequest,
  getFeatureRequests,
  removeFeatureRequestVote,
  updateFeatureRequestStatus,
};
