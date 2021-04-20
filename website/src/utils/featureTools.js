// Copyright 2021 HGTP Capstone Team at the University of Utah:
// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

import axios from 'axios';

export const deleteFeature = async (featureRequestId) => {
  await axios.delete(`/feature-request/${featureRequestId}`);
};

export const getCompletedFeatures = async () => {
  const response = await axios.get('/feature-requests?status=completed');
  return response.data;
};

export const getPlannedFeatures = async () => {
  const response = await axios.get('/feature-requests?status=planned');
  return response.data;
};

export const getRequestedFeatures = async () => {
  const response = await axios.get('/feature-requests?status=unplanned');
  return response.data;
};

export const removeVoteForFeature = async (featureRequestId) => {
  await axios.delete(`/feature-request/vote/${featureRequestId}`);
};

export const requestFeature = async ({ description, tags }) => {
  await axios.post('/feature-request', {
    description,
    tags,
  });
};

export const updateFeatureStatus = async (featureRequestId, status) => {
  await axios.put('/feature-request', { featureRequestId, status });
};

export const searchFeatures = async () => {};

export const voteForFeature = async (featureRequestId) => {
  await axios.post('/feature-request/vote', { featureRequestId });
  // TODO: If status is 201, then a vote was added. If status was 204, then the vote was already there.
  // return true if added vote, false if already voted for.
};
