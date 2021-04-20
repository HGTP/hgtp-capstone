<template>
  <!--
    Copyright 2021 HGTP Capstone Team at the University of Utah: 
    Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
    Licensed under the MIT license. Read the project readme for details.
  -->
  <div id="preset">
    <div
      class="d-flex align-items-center justify-content-center"
      id="preset-circle"
      :style="circleStyle"
      @click="clicked"
      @mouseover="setHoveringStatus(true)"
      @mouseleave="setHoveringStatus(false)"
    >
      <h3 class="text-center" id="preset-text" v-show="hovering">
        {{ name }}
      </h3>
      <font-awesome-icon
        :icon="faIcon"
        id="preset-icon"
        size="5x"
        style="color: #2971a4;"
        v-show="!hovering"
      />
    </div>
  </div>
</template>

<script>
export default {
  name: 'Preset',
  computed: {
    circleStyle() {
      const color = this.selected ? '#F8F8F8' : '#ffffff';
      return `background: ${color}`;
    },
  },
  data: function() {
    return {
      hovering: false,
    };
  },
  methods: {
    /**
     * This lets us specify specifically when/where we want the on click event to be triggered.
     * Doing this is more accurate than using @click.native in the parent, because using that registers
     * links from the root div, rather than specifically (in this case) the circle.
     */
    clicked: function(event) {
      this.$emit('click', this.page, event);
    },
    setHoveringStatus: function(value) {
      this.hovering = value;
    },
  },
  props: {
    faIcon: String,
    name: String,
    selected: Boolean,
  },
};
</script>

<style scoped>
#preset-circle {
  border-radius: 50%;
  /* Each comma-separated shadow is given these values: h-offset, v-offset, blur, spread, color */
  box-shadow: 2px 2px 6px 0 rgba(0, 0, 0, 0.1),
    -2px -2px 6px 0 rgba(0, 0, 0, 0.1);
  left: 50%;
  height: 200px;
  position: relative;
  top: 0;
  transform: translateX(-50%);
  width: 200px;
}
#preset-circle:hover {
  opacity: 0.75;
  filter: alpha(
    opacity=75
  ); /* IE8 and lower (https://developer.mozilla.org/en-US/docs/Web/CSS/opacity) */
  cursor: pointer;
}
#preset-text {
  /* display: none; */
  transform: translateY(25%);
  /* https://developer.mozilla.org/en-US/docs/Web/CSS/user-select */
  user-select: none;
  -moz-user-select: none;
  -webkit-user-select: none;
  -ms-user-select: none;
}
</style>
