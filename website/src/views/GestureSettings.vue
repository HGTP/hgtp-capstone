<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div class="home">
    <b-container>
      <b-row>
        <b-col>
          <h2 class="text-center mt-4 mb-5">Presets</h2>
        </b-col>
      </b-row>
      <b-row>
        <b-col v-for="preset in presets" :key="preset.name">
          <Preset v-bind="preset" @click="selectPreset(preset.name)"></Preset>
        </b-col>
      </b-row>
      <span v-show="selectedPreset">
        <b-row>
          <b-col>
            <h2 class="text-center my-5">Gestures</h2>
          </b-col>
        </b-row>
        <b-row>
          <b-col v-for="gesture in presetGestures" :key="gesture.phoneAction">
            <Gesture v-bind="gesture"></Gesture>
          </b-col>
        </b-row>
      </span>
    </b-container>
  </div>
</template>

<script>
import Gesture from '@/components/Gesture.vue';
import Preset from '@/components/Preset.vue';

import { getPresetGestures } from '@/utils/presetTools.js';

export default {
  name: 'Home',
  components: {
    Gesture,
    Preset,
  },
  created() {
    this.presets.push({
      faIcon: 'map-marked-alt',
      name: 'GPS',
      selected: false,
    });
    this.presets.push({
      faIcon: 'music',
      name: 'Music',
      selected: false,
    });
    this.presets.push({
      faIcon: 'phone',
      name: 'Phone',
      selected: false,
    });
  },
  data: function() {
    return {
      selectedPreset: false,
      presets: [],
      presetGestures: [],
    };
  },
  methods: {
    /**
     * Updates the selected preset, along with the associated gestures.
     *
     * @param {String} name
     */
    selectPreset: async function(name) {
      if (name) {
        this.selectedPreset = true;
        this.presets.forEach((preset) => {
          preset.selected = name === preset.name;
          if (name === preset.name) {
            preset.selected = true;
          }
        });

        this.presetGestures.splice(0, this.presetGestures.length);
        const allPresetGestures = await getPresetGestures(name);
        allPresetGestures.forEach((gesture) => {
          this.presetGestures.push(gesture);
        });
      }
    },
  },
};
</script>
