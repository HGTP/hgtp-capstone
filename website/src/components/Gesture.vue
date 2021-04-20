<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div id="gesture">
    <div
      id="gesture-circle"
      class="d-flex align-items-center justify-content-center"
      :style="style"
      @click="$bvModal.show(bvModalId)"
    >
      <font-awesome-icon :icon="currentFaIcon" id="gesture-icon" size="3x" />
    </div>
    <h3 id="gesture-text" class="text-center">{{ name }}</h3>

    <b-modal v-bind:id="bvModalId" hide-footer>
      <template #modal-title>
        Pick a phone action for gesture {{ name }}
      </template>
      <div class="d-block text-center">
        <b-form-select
          v-model="selected"
          :options="availablePhoneActions"
        ></b-form-select>
      </div>
      <b-button
        class="mt-3"
        block
        variant="primary"
        @click="updatePhoneAction(() => $bvModal.hide(bvModalId))"
      >
        Submit
      </b-button>
      <b-button
        class="mt-3"
        block
        variant="danger"
        @click="resetPhoneAction(() => $bvModal.hide(bvModalId))"
      >
        Reset
      </b-button>
    </b-modal>
  </div>
</template>

<script>
import {
  getIconNameFromPhoneAction,
  phoneActions,
  resetPhoneAction,
  updatePhoneAction,
} from '@/utils/gestureTools.js';

const defaultFaIcon = 'question';

export default {
  name: 'Gesture',
  computed: {
    bvModalId() {
      return `gesture-bv-modal-${this.name}`;
    },
    style() {
      return `background: ${this.color}`;
    },
  },
  data() {
    return {
      availablePhoneActions: [],
      currentFaIcon: defaultFaIcon,
      selected: null,
    };
  },
  created() {
    for (const phoneAction of Object.values(phoneActions)) {
      this.availablePhoneActions.push(phoneAction);
    }
    this.selected = this.phoneAction;
    this.currentFaIcon = this.faIcon;
  },
  methods: {
    updatePhoneAction: async function(done) {
      await updatePhoneAction(this.name, this.selected, this.preset);
      this.currentFaIcon = getIconNameFromPhoneAction(this.selected);
      done();
    },
    resetPhoneAction: async function(done) {
      await resetPhoneAction(this.name, this.preset);
      this.currentFaIcon = defaultFaIcon;
      done();
    },
  },
  props: {
    color: String,
    faIcon: {
      type: String,
      default: defaultFaIcon,
    },
    name: String,
    phoneAction: String,
    preset: String,
  },
};
</script>

<style scoped>
#gesture-circle {
  border-radius: 50%;
  /* Each comma-separated shadow is given these values: h-offset, v-offset, blur, spread, color */
  box-shadow: 2px 2px 6px 0 rgba(0, 0, 0, 0.1),
    -2px -2px 6px 0 rgba(0, 0, 0, 0.1);
  left: 50%;
  height: 150px;
  position: relative;
  top: 0;
  transform: translateX(-50%);
  width: 150px;
}
#gesture-circle:hover {
  opacity: 0.75;
  filter: alpha(
    opacity=75
  ); /* IE8 and lower (https://developer.mozilla.org/en-US/docs/Web/CSS/opacity) */
  cursor: pointer;
}
#gesture-text {
  left: 50%;
  position: relative;
  top: 5px;
  transform: translateX(-50%);
  /* https://developer.mozilla.org/en-US/docs/Web/CSS/user-select */
  user-select: none;
  -moz-user-select: none;
  -webkit-user-select: none;
  -ms-user-select: none;
}
</style>
