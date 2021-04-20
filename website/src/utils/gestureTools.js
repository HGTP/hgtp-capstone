// Copyright 2021 HGTP Capstone Team at the University of Utah:
// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

import axios from 'axios';

export const gestureColors = {
  UP: '#2971a4',
  DOWN: '#2971a4',
  LEFT: '#2971a4',
  RIGHT: '#2971a4',
};

export const phoneActions = {
  // ACCEPT_CALL: 'Accept Call',
  // DECLINE_CALL: 'Decline Call',
  DO_NOT_DISTURB: 'Turn On/Off Do Not Disturb',
  INCREASE_VOLUME: 'Volume Up',
  LOWER_VOLUME: 'Volume Down',
  NEXT_GPS_DIRECTION: 'Next GPS Direction',
  PAUSE_MUSIC: 'Pause',
  PLAY_MUSIC: 'Play',
  READ_TEXT_ALOUD: 'Read Recent Text Aloud',
  SKIP: 'Skip',
};

export const getIconNameFromPhoneAction = (phoneAction) => {
  switch (phoneAction) {
    // case phoneActions.ACCEPT_CALL:
    //   return 'phone';
    // case phoneActions.DECLINE_CALL:
    //   return 'phone-slash';
    case phoneActions.DO_NOT_DISTURB:
      return 'moon';
    case phoneActions.INCREASE_VOLUME:
      return 'volume-up';
    case phoneActions.LOWER_VOLUME:
      return 'volume-down';
    case phoneActions.NEXT_GPS_DIRECTION:
      return 'location-arrow';
    case phoneActions.PAUSE_MUSIC:
      return 'pause';
    case phoneActions.PLAY_MUSIC:
      return 'play';
    case phoneActions.READ_TEXT_ALOUD:
      return 'sms';
    case phoneActions.SKIP:
      return 'forward';
    default:
      return 'question';
  }
};

export const updatePhoneAction = (gesture, phoneAction, presetName) => {
  return axios.put(`/preset/${presetName}/${gesture}`, { phoneAction });
};

export const resetPhoneAction = (gesture, presetName) => {
  return axios.delete(`/preset/${presetName}/${gesture}`);
};
