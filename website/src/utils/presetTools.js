// Copyright 2021 HGTP Capstone Team at the University of Utah:
// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

import axios from 'axios';
import { gestureColors, getIconNameFromPhoneAction } from './gestureTools';

const getGestureColor = (gesture) => {
  switch (gesture) {
    case 'UP':
      return gestureColors.UP;
    case 'DOWN':
      return gestureColors.DOWN;
    case 'LEFT':
      return gestureColors.LEFT;
    case 'RIGHT':
      return gestureColors.RIGHT;
    default:
      return 'white';
  }
};

/**
 * Populates any missing gestures. Preserves the order from UP to RIGHT.
 *
 * @param {String} name
 */
const populateMissingGestures = (existingGestures, presetName) => {
  const gestures = [];
  const indexToGesture = ['UP', 'DOWN', 'LEFT', 'RIGHT'];
  for (let index = 0; index < 4; index++) {
    if (!gestures[index]) {
      gestures.push({
        color: gestureColors[indexToGesture[index]],
        name: indexToGesture[index],
        preset: presetName,
      });
    }
  }

  const addPhoneAction = (gesture, index) => {
    gestures[index].faIcon = getIconNameFromPhoneAction(gesture.phoneAction);
    gestures[index].phoneAction = gesture.phoneAction;
  };

  existingGestures.forEach((gesture) => {
    switch (gesture.name) {
      case 'UP':
        addPhoneAction(gesture, 0);
        break;
      case 'DOWN':
        addPhoneAction(gesture, 1);
        break;
      case 'LEFT':
        addPhoneAction(gesture, 2);
        break;
      case 'RIGHT':
        addPhoneAction(gesture, 3);
        break;
      default:
        break;
    }
  });

  return gestures;
};

export const getPresetGestures = async (presetName) => {
  const response = await axios.get(
    `/user/gesture-settings?presetName=${presetName}`
  );
  let result = response.data.map((setting) => {
    return {
      color: getGestureColor(setting.gesture),
      name: setting.gesture,
      phoneAction: setting.phoneAction,
      preset: setting.presetName,
    };
  });
  if (result.length < 6) {
    result = populateMissingGestures(result, presetName);
  }
  return result;
};
